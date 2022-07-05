using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData
{
    public interface IMailData
    {
        Task<Mail> GetAsync(Guid id);  
        Task<List<Mail>> GetAllAsync();
        Task<Mail> AddAsync(Mail mail, List<String> tags);
        Task<Mail> UpdateAsync(Mail mail, List<String> tags);
        Task DeleteAsync(Guid id);

        Task<List<Mail>> GetByTagAsync(String tag);
        Task<List<Mail>> GetBySenderAsync(String sender);
        Task<List<Mail>> GetByRecipientAsync(String recipient);
        Task<List<Mail>> GetByForPeriodAsync(DateTime start, DateTime end);
    }
}
