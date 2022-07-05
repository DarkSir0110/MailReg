using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData
{
    public class Tag
    {
        public Guid Id { get;  }

        public String Name { get; }

        public Tag(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
