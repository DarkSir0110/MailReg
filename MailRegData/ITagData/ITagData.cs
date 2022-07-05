using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData.ITagData
{
    public interface ITagData
    {
        Task<List<String>> GetAllAsync();
        Task<Guid> AddAsync(String name);
        Task DeleteAsync(String name);
    }
}
