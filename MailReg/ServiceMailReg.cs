using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MailReg
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceMailReg" в коде и файле конфигурации.
    public class ServiceMailReg : IServiceMailReg
    {
        public int Connect()
        {
            throw new NotImplementedException();
        }

        public void DoWork()
        {
        }
    }
}
