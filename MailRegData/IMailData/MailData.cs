using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;



namespace MailRegData
{
    public class MailData : IMailData
    {
        private readonly MailMapper _mapper = new MailMapper();


        private void AddEmailTags(Guid mailId, Task<Guid[]> tagIds, SqlTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {

            var @params = new SqlParameter[] { new SqlParameter("@id", id) };
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            SqlCommand command = ServiceHelper.CreateProcedureCommand("sp_delete_email", connection, @params);

            await ServiceHelper.ExecuteAsync(command);
        }

        public Task<List<Mail>> GetAllAsync()
        {
            var @params = new SqlParameter[0];
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            using SqlCommand command = ServiceHelper.CreateProcedureCommand("sp_get_emails", connection, @params);

            return ServiceHelper.GetItemsAsync(command, _mapper);
        }

        public async Task<Mail> GetAsync(Guid id)
        {
            var @params = new SqlParameter[] { new SqlParameter("@id", id) };
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            using SqlCommand command = ServiceHelper.CreateProcedureCommand("sp_get_email", connection, @params);

            return await ServiceHelper.GetItemAsync(command, _mapper);
        }

        public async Task<List<Mail>> GetByForPeriodAsync(DateTime start, DateTime end)
        {
            if (start > end) throw new Exception("Invalid dates order.");

            var @params = new SqlParameter[]
            {
                new SqlParameter("@start_date", start.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@end_date", end.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            using SqlCommand command =
                ServiceHelper.CreateProcedureCommand("sp_get_emails_for_period", connection, @params);

            return await ServiceHelper.GetItemsAsync(command, _mapper);
        }

        public async Task<List<Mail>> GetByRecipientAsync(string recipient)
        {
            var @params = new SqlParameter[] { new SqlParameter("@recipient", recipient) };
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            using SqlCommand command = ServiceHelper.CreateProcedureCommand("sp_get_emails_by_recipient", connection, @params);

            return await ServiceHelper.GetItemsAsync(command, _mapper);
        }

        public async Task<List<Mail>> GetBySenderAsync(string sender)
        {
            var @params = new SqlParameter[] { new SqlParameter("@sender", sender) };
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            using SqlCommand command = ServiceHelper.CreateProcedureCommand("sp_get_emails_by_sender", connection, @params);

            return await ServiceHelper.GetItemsAsync(command, _mapper);
        }

        public Task<List<Mail>> GetByTagAsync(string tag)
        {
            var @params = new SqlParameter[] { new SqlParameter("@tag", tag) };
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            using SqlCommand command =
                ServiceHelper.CreateProcedureCommand("sp_get_emails_by_tag", connection, @params);

            return ServiceHelper.GetItemsAsync(command, _mapper);
        }

        public async Task UpdateAsync(Mail mail, List<String> tags)
        {
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            await connection.OpenAsync();
            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                Mail old = await GetAsync(mail.Id);

                await ProcessTags(mail.Id, tags, JsonConvert.DeserializeObject<List<String>>(old.Tags), transaction);

                await UpdateEmailData(mail, transaction);

                transaction.Commit();
            }
            catch
            {

            }
        }

        private async Task UpdateEmailData(Mail mail, SqlTransaction transaction)
        {
            var procedure = "sp_update_email";
            var @params = new SqlParameter[]
            {
                new SqlParameter("@id", mail.Id),
                new SqlParameter("@reg_date", mail.DateReg.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@sender", mail.Sender.ToLower()),
                new SqlParameter("@recipient", mail.Recipient.ToLower()),
                new SqlParameter("@name", mail.Name),
                new SqlParameter("@content", mail.Content),
                new SqlParameter("@tags", mail.Tags)
            };

            using SqlCommand command =  ServiceHelper.CreateProcedureCommand(procedure, transaction, @params);
           
            await ServiceHelper.ExecuteAsync(command);
        }

        private static async Task ProcessTags(Guid mailId, IReadOnlyCollection<String> newTags,
            IReadOnlyCollection<String> oldTags, SqlTransaction transaction)
        {
            var oldTagSet = new HashSet<String>();
            var newTagSet = new HashSet<String>();

            foreach (String tag in oldTags) oldTagSet.Add(tag);
            foreach (String tag in newTags) newTagSet.Add(tag);

            var tagsToAdd = new List<String>(newTags.Where(t => !oldTagSet.Contains(t)));
            var tagsToDelete = new List<String>(oldTags.Where(t => !newTagSet.Contains(t)));

            var tagsToAddIds = await GetTagIds(tagsToAdd, transaction);
            var tagsToDeleteIds = await GetTagIds(tagsToDelete, transaction);

            await AddEmailTags(mailId, tagsToAddIds, transaction);
            await DeleteEmailTags(mailId, tagsToDeleteIds, transaction);
        }

        private async Task<Mail> GetAsync(Guid id, SqlTransaction transaction)
        {
            var @params = new SqlParameter[] { new SqlParameter("@id", id) };
            using SqlCommand command = ServiceHelper.CreateProcedureCommand("sp_get_email", transaction, @params);

            return await ServiceHelper.GetItemAsync(command, _mapper);
        }

        /// <summary>
        /// Returns tag GUIDs for a specific email as a part of insert/update transaction.
        /// </summary>
        /// <param name="tags">Email tag names collection.</param>
        /// <param name="transaction">Transaction to attach possible insert action.</param>
        private static async Task<Guid[]> GetTagIds(IReadOnlyCollection<String> tags, SqlTransaction transaction)
        {
            if (tags == null || tags.Count == 0) 
                return new Guid[0];

            return await Task.WhenAll(tags.Select(t => GetTagId(t, transaction)));
        }

        /// <summary>
        /// For existing tag just returns its GUID.
        /// For new tag adds the one to database as a part of transaction and returns its GUID.
        /// </summary>
        /// <param name="name">Tag name.</param>
        /// <param name="transaction">Transaction to attach possible insert action.</param>
        private static async Task<Guid> GetTagId(String name, SqlTransaction transaction)
        {
            var procedure = "sp_get_tag_id_or_add";
            var param = new SqlParameter("@name", name.ToLower());
            using SqlCommand command = ServiceHelper.CreateProcedureCommand(procedure, transaction, param);

            return await ServiceHelper.AddItemAsync(command);
        }

        /// <summary>
        /// Adds email entity to database as a part of transaction and returns its GUID.
        /// </summary>
        /// <param name="mail">Email data transfer object.</param>
        /// <param name="transaction">Transaction to attach insert action.</param>
        private static async Task<Guid> AddEmail(Mail mail, SqlTransaction transaction)
        {
            var procedure = "sp_add_email";
            var @params = new SqlParameter[]
            {
                new SqlParameter("@registration_date", mail.DateReg.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("@sender", mail.Sender.ToLower()),
                new SqlParameter("@recipient", mail.Recipient.ToLower()),
                new SqlParameter("@subject", mail.Name),
                new SqlParameter("@text", mail.Content),
                new SqlParameter("@tags", mail.Tags)
            };

            using SqlCommand command = ServiceHelper.CreateProcedureCommand(procedure, transaction, @params);

            return await ServiceHelper.AddItemAsync(command);
        }

        /// <summary>
        /// Updates email data in database as a part of transaction.
        /// </summary>
        /// <param name="mail">Email data transfer object.</param>
        /// <param name="transaction">Transaction to attach update action.</param>
      

        /// <summary>
        /// Adds email-tag relation for each email tag as a part of transaction.
        /// </summary>
        private static Task AddEmailTags(Guid mailId, IEnumerable<Guid> tagIds, SqlTransaction transaction)
        {
            return Task.WhenAll(tagIds.Select(id => AddEmailTag(mailId, id, transaction)));
        }

        /// <summary>
        /// Deletes email-tag relation for each email tag as a part of transaction.
        /// Deletes tag entity if there is no more email-tag relations for a specific tag id.
        /// </summary>
        private static Task DeleteEmailTags(Guid mailId, IEnumerable<Guid> tagIds, SqlTransaction transaction)
        {
            return Task.WhenAll(tagIds.Select(id => DeleteEmailTag(mailId, id, transaction)));
        }

        private static async Task<Guid> AddEmailTag(Guid mailId, Guid tagId, SqlTransaction transaction)
        {
            var procedure = "sp_add_email_tag";
            var @params = new SqlParameter[]
                {new SqlParameter("@email_id", mailId), new SqlParameter("@tag_id", tagId)};

            using SqlCommand command = ServiceHelper.CreateProcedureCommand(procedure, transaction, @params);

            return await ServiceHelper.AddItemAsync(command);
        }

        private static async Task DeleteEmailTag(Guid mailId, Guid tagId, SqlTransaction transaction)
        {
            var procedure = "sp_delete_email_tag";
            var @params = new SqlParameter[]
                {new SqlParameter("@email_id", mailId), new SqlParameter("@tag_id", tagId)};

            using SqlCommand command = ServiceHelper.CreateProcedureCommand(procedure, transaction, @params);

            await ServiceHelper.ExecuteAsync(command);
        }

        Task<Mail> IMailData.UpdateAsync(Mail mail, List<string> tags)
        {
            throw new NotImplementedException();
        }

        public async Task<Mail> AddAsync(Mail mail, List<string> tags)
        {
            using var connection = new SqlConnection(ServiceHelper.ConnectionString);
            await connection.OpenAsync();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using var tagIds = GetTagIds(tags, transaction);

                Guid mailId = await AddEmail(mail, transaction);

                AddEmailTags(mailId, tagIds, transaction);

                transaction.Commit();

                return await mailId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error was occured. Transaction declined.", ex);
            }
        }
    }


    
}
