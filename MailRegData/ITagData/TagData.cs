using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData.ITagData
{
    public class TagData : ITagData

    {
        public Task<Guid> AddAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task ITagData.DeleteAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
