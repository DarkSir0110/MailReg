using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MailReg
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IServiceMailReg" в коде и файле конфигурации.
    [ServiceContract]
    public interface IServiceMailReg
    {
        //connect in programm
        [OperationContract]
        int Connect();

        //add atribute

        //read list mails






    }
}
