namespace Butterfly
{
    class Program
    {
        static void Main(string[] args)
        {
            // Данный обьект не может быть (Independent)индивидуальным, и неявно является частью системы.
            // при его уничтожении система завершит свою работу.
            Butterfly.fly<TestController>();
        }
    }

    public class TestController : Controller.Thread
    {
        IInput<string> inputSendingMessage;
        IInput<string> inputSendingMessageToVIPClients;
        IInput<string> inputSendingMessageToDefaultClients;
        IInput<string> inputRequestInformation;

        protected override void Construction()
        {
            listen_message<string>("Client is creating.")
                .output_to((message) => { Console($"Обьект {message} отчитался о том что он был создан."); }, 1, 5, "Poll:Listen");

            inputSendingMessage =  sending_message<string>("All clients sending message.");
            inputSendingMessageToVIPClients = sending_message<string>("VIP clients sending message.");
            inputSendingMessageToDefaultClients = sending_message<string>("Default clients sending message.");

            inputRequestInformation = sending_message<string>();

            listen_echo<string>()
                .output_to(l_obj<ClientsInformation>().ReceiveEcho, 1, 11); // Входные данные перехватываются в отдельный поток.
        }

        void Start()
        {
            Console("\n\n\n\n\n Здравствуй. В данной системе заложена неплохая как мне кажется архетектура, но из за того что это первый проект такого плана," +
                "сдесь хватает плохого кода, я всеволишь совершенсвую свои навыки. В данном примере нету главной особености это системы , она работает, но еще не тестировалась," +
                "так что будем считать что ее нету. Если есть какие то советы, или просто обратный отклик dima1994zzz@gmail.com, заранее спасибо.\n\n\n\n\n");

            add_thread("Process", Process, 111, Thread.Priority.Normal);
        }

        global::System.Collections.Generic.Dictionary<string, Client> Clients 
            = new global::System.Collections.Generic.Dictionary<string, Client>();

        void Process()
        {
            Console($"Создать клинта: 1");
            Console($"Уничтожить клинта:2");
            Console($"Разаслать сообщения всем клинтам: 3");
            Console($"Разаслать сообщения всем VIP клинтам: 4");
            Console($"Разаслать сообщения всем Default клинтам: 5");
            Console($"Разослать всем клинтам задание узнать актуальную информацию для своей группы: 6");
            Console($"Создать обьект который не сможет запустится: 11");
            Console($"Уничтожить систему: 111");

            string processType = System.Console.ReadLine();

            switch(processType)
            {
                case "1":

                    string creatingClientName = ReadLine("Введите имя клиента:");

                    if (Clients.ContainsKey(creatingClientName)) Clients.Remove(creatingClientName);

                    while (true)
                    {
                        string groupName = ReadLine($"Введите номер группы в которую вы хотите поместить клиeнта. \n1)Default \n2)VIP \n");

                        if (groupName == "1")
                        {
                            Clients.Add(creatingClientName, create_object<Client>(creatingClientName, "Default"));
                            break;
                        }
                        else if (groupName == "2")
                        {
                            Clients.Add(creatingClientName, create_object<Client>(creatingClientName, "VIP"));
                            break;
                        }
                        else
                            Console($"Вы ввели некоректный номер группы, {groupName} данного номера группы не сущесвует.");
                    }
                    
                    break;

                case "2":

                    string deleteClientName = ReadLine("Введите имя клиента:");

                    if (try_delete_object<Client>(deleteClientName))
                    {
                        Clients.Remove(deleteClientName);

                        Console("delete client....");
                    }
                    else
                        Console($"Клиента с именем {deleteClientName} не сущесвует.");

                    break;

                case "3":

                    string message1 = ReadLine("Введите сообщение которое нужно разослать всем клиетам:");
                    inputSendingMessage.ToInput(message1);

                    break;
                case "4":

                    string message2 = ReadLine("Введите сообщение которое нужно разослать VIP клиетам:");
                    inputSendingMessageToVIPClients.ToInput(message2);

                    break;
                case "5":

                    string message3 = ReadLine("Введите сообщение которое нужно разослать Default клиетам:");
                    inputSendingMessageToDefaultClients.ToInput(message3);

                    break;
                case "6":

                    inputRequestInformation.ToInput("info");

                    break;
                case "11":
                    create_object<ClientDestroying>("");
                    break;
                case "111":

                    SystemInformation("ReadLine заблокировало поток незабудь нажать Enter.", System.ConsoleColor.Red);
                    destroy();

                    break;
            }
        }
    }

    public class Client : Controller.Local.value<string>.Independent
    {
        IInput<string> inputSendMessage;

        protected override void Construction()
        {
            send_message<string>(ref inputSendMessage, "Client is creating.");

            listening_messages<string>("All clients sending message.")
                .output_to((message) => Console(message));

            listening_messages<TestController, string>()
                .output_to((message) =>
                {
                    if (message == "info")
                        return localValue;

                    return "";
                })
                .output_to_echo<TestController, string>()
                    .output_to((message) => { Console($"Текущeq актуальной информацией для {GetKey()} является {message}."); });


            if (localValue == "VIP")
            {
                listening_messages<string>("VIP clients sending message.")
                    .output_to(MessageProcessing)
                        .output_to(ShowMessage);
            }
            else if (localValue == "Default")
            {
                listening_messages<string>("Default clients sending message.")
                    .output_to(MessageProcessing)
                        .output_to(ShowMessage);
            }
        }

        void Start()
        {
            SystemInformation($"Client [{GetKey()}] creating.", System.ConsoleColor.Cyan);

            inputSendMessage.ToInput($"Key:{GetKey()}, Group:{localValue}");
        }

        string MessageProcessing(string message)
        {
            if (localValue == "VIP")
                return $"Получено сообщение для VIP : {message}";
            else if (localValue == "Default")
                return $"Получено сообщение для Default : {message}";

            return "";
        }

        void ShowMessage(string message)
        {
            Console(message);
        }
    }

    public class ClientsInformation : ILocalObject, ILocalObject.ReceiveEcho<string, string>
    {
        string VIPInformation = "[ВНИМАНИЕ ИНФОРМАЦИЯ: Ты VIP пользователь!!!!!!!!!!!!!]";
        string DefaultInformation = "[ВНИМАНИЕ ИНФОРМАЦИЯ: Ты простой пользователь!!!!!!!!!!!!!]";

        public void ReceiveEcho(string value, IEchoReturn<string> send)
        {
            if (value == "VIP")
            {
                send.To(VIPInformation);
            }
            else if (value == "Default")
            {
                send.To(DefaultInformation);
            }
        }
    }

    public class ClientDestroying : Controller.Independent
    {
        protected override void Construction()
        {
            add_handler<TestHandler, string[]>();
        }

        void Start()
        {

        }

        void Configurate()
        {
            Console("В конфигурационом методе был запущен процес уничтожения, от чего не будет запущен метод Start(), но будет запущен метод Stop().");
            destroy();
        }

        void Stop()
        {

        }
    }

    public class TestHandler : Handler<string[]>
    {
        protected override void Construction()
        {
        }

        void Start()
        {}
    }
}
