namespace Butterfly.system.objects.main
{
    /// <summary>
    /// Работа с информационым пространсвом.
    /// </summary>
    public interface IInforming
    {
        /// <summary>
        /// Ошибка времени сборки.
        /// </summary>
        public void Exception(string pMessage, params string[] pParams);
        /// <summary>
        /// Вывод в консоль.
        /// </summary>
        public void Console(string pMessage);
        /// <summary>
        /// Следует вызываеть если вы реализовали возможность востановление работы.
        /// </summary>
        public void RuntimeError(string pMessage);
        /// <summary>
        /// Системная информация о жизни обьектов в нутри MainObject.
        /// </summary>
        public void SystemInformation(string pMessage, global::System.ConsoleColor pColor = global::System.ConsoleColor.Gray);
    }

    /// <summary>
    /// Информирование через MainObject.
    /// Console - вывод в консоль.
    /// Exception - ошибка времени сборки.
    /// RutimeError - ошибка времени выполнения.
    /// SystemInformation - системная информация.
    /// </summary>
    public class Informing
    {
        private readonly IInforming InfomingMainObject;

        private readonly string Name;


        public Informing(string pName, IInforming pMainObject)
        {
            InfomingMainObject = pMainObject;

            Name = pName;
        }

        public void Console(string pMessage)
        {
            InfomingMainObject.Console($"{Name}->{pMessage}");
        }

        public void Exception(string pMessage, params string[] pParams)
        {
            InfomingMainObject.Exception($"{Name}->{pMessage}", pParams);
        }

        public void RuntimeError(string pMessage)
        {
            InfomingMainObject.RuntimeError($"{Name}->{pMessage}");
        }

        public void SystemInformation(string pMessage, System.ConsoleColor pColor = System.ConsoleColor.Green)
        {
            InfomingMainObject.SystemInformation($"{Name}->{pMessage}", pColor);
        }
    }
}
