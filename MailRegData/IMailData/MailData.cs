using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData
{
    public class MailData : IMailData
    {
        private readonly MailMapper _mailMapper = new MailMapper();

        public Task<Mail> AddAsync(Mail mail, List<string> tags)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Mail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Mail> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Mail>> GetByForPeriodAsync(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<List<Mail>> GetByRecipientAsync(string recipient)
        {
            throw new NotImplementedException();
        }

        public Task<List<Mail>> GetBySenderAsync(string sender)
        {
            throw new NotImplementedException();
        }

        public Task<List<Mail>> GetByTagAsync(string tag)
        {
            throw new NotImplementedException();
        }

        public Task<Mail> UpdateAsync(Mail mail, List<string> tags)
        {
            throw new NotImplementedException();
        }
    }
}
