using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailRegData
{
    public class Mail
    {
        public Guid Id { get; }

        public String Name { get; }

        public DateTime DateReg { get; }

        public String Recipient { get; }

        public String Sender { get; }

        public String Content { get; }

        /// <summary>
        /// Serialized list of string.
        /// </summary> 
        
        public String Tags { get; }

        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="dateReg"></param>
        /// <param name="recipient"></param>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        /// <param name="Tags">Serialized list of string.</param> 
        
        public Mail(Guid id, String name, DateTime dateReg, String recipient, String sender,String context,String tags)
        {
            Id= id;
            Name= name;
            DateReg= dateReg;
            Recipient= recipient;
            Sender= sender;
            Content= context;
            Tags = tags;
              
        }
    }
}
