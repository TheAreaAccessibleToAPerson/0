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
        IInput<string[]> sendingMessageInput;

        protected override void Construction()
        {
            // Рассылка сообщений всем подписавшимся.
            sendingMessageInput = sending_message<string[]>();

            listen_message<int>()
                .output_to((errorLocalValue) =>
                {
                    // Созданый обьект в методе Configurate() проверим переданые в него локальные 
                    // данные и пощитал что такое значение не премлемо для нормальной работы.
                    // Пересоздадим обьект с другими локальными данными.
                    // В данный момент реализована ошибка если создастся обьект со схожим именем.
                    // Поэтому изменим его имя.
                    create_object<TestController2>(u++.ToString(), errorLocalValue + 100);
                });

            echo<string>()
                .output_to((value, returnResult) =>
                {
                    Console($"Получение значение в echo:{value} от клиента с ID:{returnResult.GetObjectID()}.");

                    //Вышлем в ответ 
                    returnResult.To("Отразившийся echo.");
                }, 1000, 10); // Подпишимся в пулл. Данный метод продолжит свое выполнение в другом потоке.
                              // данный пулл будет расчитан на 1000 участников. При переполении пула создастся новый
                              // и если все клинты отпишутся из него отчего он окажется пустым, пул уничтожится.
                              // В данный момент не релизовано равномерное распределение клинтов между пулами.

            echo<string, int>() // Сдесь примим данные из echo.
                .output_to((value, returnResult) => 
                { 
                    if (value == "Input1")
                    {
                        returnResult.To(1);
                    }
                    else if (value == "Input2")
                    {
                        returnResult.To(2);
                    }
                });

        }

        
        void Start()
        {
            // Создадим клиeнтов, эмитируем их подключение. Данный метод предназначен для тестирования
            // отдельных частей системы. Но если сильно хочется можно и реализовать на нем что либо.
            add_thread("CreatingController", CreatingControllers, 1, Thread.Priority.Lowest);
        }

        int u = 0;
        void CreatingControllers() // Создадим билибирдень.
        {
            if (u < 55) // Создадим 50 индивидуальных обьектов. При вызове метода destroy() в нем или его дочерних обьектах
                        // произойдет уничтожение плодь до данного обьекта TestController2 ...
            {
                for (int i = 0; i < 555; i++)
                {
                    // так же передадим ему локальное значение.
                    create_object<TestController2>(u++.ToString(), i + 100);
                }
                
            }

            // Будем инкрементировать каждый шаг.
            u++;

            if (u > 2) // так же создадим простых обьектов, при их уничтжение иничтожатся все обьекты до ближайшего Independent
                         // обьекта, который в данный момент является сама система.
            {
                // Создадим обьект который уничтожит все.
                create_object<TestController3>(u++.ToString());
            }

            sendingMessageInput.ToInput(new string[] { "Input1", "Input2", "Input3", "Input3" });
        }
    }

    public class TestController2 : Controller.Local.value<int>.Independent
    {
        IInput<int> sendMesageErrorConfigurate;

        protected override void Construction()
        {
            send_message<TestController, int>(ref sendMesageErrorConfigurate);

            // Подпишимся на рассылку сообщений, это можно было бы сделать
            // из самого обработчика, но в данном контексте обработчик используется
            // по назначению что бы продемонстрировать синхроную и асинхроную работу.
            listening_messages<TestController, string[]>()
                .output_to<TestHandler1>(1000, 10) // Вывеод перехватит отдельный поток.
                    .output_to<int>((number) =>
                    {
                        //Console(number.ToString());
                    });
        }

        void Start()
        {

        }

        void Configurate() // Даный метод вызовется после метода Construction().
        {
            if (localValue == 120 || localValue == 130 || localValue == 150)
            {
                // Если локальное значение равно 120 тогда уничтожим данный обьект.
                // Его метод Start не вызовется. 
                // Eсли обьект не удалось создать оповестим об этом его родителя и он создаст новый обьект и передаст туда
                // отлитный от текущих локальные данные.
                sendMesageErrorConfigurate.ToInput(localValue);
                // После чего уничтожим обьект.
                destroy();
                // Вызовется метод Stop...
            }
            
            if (localValue == 220)
            {
                Console("Пересоздался наш ранее уничтоженый обьект.");
            }
        }

        void Stop()
        {
        }
    }

    public class TestHandler1 : Handler<string[], int> // Данный обьект разрабатывается для синхроной и асинхроной абработки массивов, но можно и просто использовать. 
    {
        // Каждый input примит входные данные в текущий обработчик.
        protected override void Construction()
        {
            input_to((values) => { return values[0]; })
                .async_output_to_echo<TestController, int>()    // После отправки echo цепочка событий в текущем input_to прервется
                    .await(l_obj<LocalObjectTest>().Add)        // и за окончание данной цепочки будет отвечать поток который пришлет сюда данные.
                        .output_to(output);                     // Не дожидаясь ответа продолжатся выполнятся следующие input_to в текущем потоке.
                                                                // (ранее при создании данного обработчкика мы выставиили все его input_to в отдельный поток)

            input_to((values) => { return values[1]; })
                .output_to_echo<TestController, int>()          // После того как будет отправлена сообщение цепочка событий во всем обработчике прервется.
                    .output_to(l_obj<LocalObjectTest>().Add)    // А продолжется только после того как придет ответ и текущий input_to закончит свою
                        .output_to(output);                     // работу. За продолжение работы не только этой но и последующих цепочек отвечает поток
                                                                // который пришлет ответ на запрос.

            // async_input_to_echo(); так же можем переоправить все входные параметры в эхо.
            // Если мы работаем с протаколами или просто выделили поток под обработку определеного вида массивов можно выстать туда массив
            // и уже после закончить работу с ним сдесь. 
            // Возможно гдето я упистил реализацию интерфейса но во всем проекте весте output_to и input_to можно прехватить
            // В отдельным пулл потоков.
            input_to((values) => {  }, 1000, 10, "POLL");    // Теперь эти 3 метода будут работать в одном пуле.
            input_to((values) => {  }, 1000, 10, "POLL");    // Текущий поток будет доставлять данные до данного места
            input_to((values) => {  }, 1000, 10, "POLL");    // и за дальнейшую их работу будет отвечать пулл под именем "POLL".

            input_to((values) => { return values[1]; })
                .output_to_echo<TestController, int>()          
                    .output_to(l_obj<LocalObjectTest>().Add, 1000, 10, "POLL") // Как было было сказано ранее когда доставляется ответ из echo запроса, закончить цепочку событий    
                        .output_to(output);                                    // Должен поток который отвечает за доставку данных.
                                                                               // Но мы можем перехватить данные ... например в пулл потока "POLL".

            // Как можно заметить я везде выставляю одинаковый данные для создоваемого пулла. Его максимальное количесво учасников и timeDelay, к сажелению я еще ненашел более красивого
            // решения. Одно решение это повторно реализовать все интерфейсы но без этих двух параметров.

            // Так же можно указать вывод данных .output_to(output) а иммено куда этот вывод должен вывести данные. Опять же не реализован внутрений send_message.
            // Только вывод в echo или другой action или func или обработчик. В данном примере мы реализовали вывод с помощью внешнего вызова .output_to в классе TestController2.
            // поэтому метод .output_to(output) выведет результат туда.
            //async_output_to_echo<TestController, int>()
            //      .await((value) => { Console("!!!!"); });
        }
    }

    public class TestHandler2 : Handler<string[], int>
    {
        protected override void Construction()
        {
        }
    }

    public class LocalObjectTest : ILocalObject, ILocalObject.Start.After
    {
        private readonly object Locker = new object();

        private int i;

        public int Add(int number)
        {
            lock(Locker) return i += number;
        }

        void ILocalObject.Start.After.Start()
        {
            
        }
    }

    // Данный обьект уничтожит все и систему в том числе.
    public class TestController3 : Controller
    {
        protected override void Construction()
        {
            //add_handler<Handler1, int>("HANDLER");
        }
        void Start()
        {
            destroy();
        }
    }

    public class Handler1 : Handler<int>
    {
        protected override void Construction()
        {
            
        }
        void Start()
        {
            deferred_create_object<TestController5>("KDJFKDJ");
        }
    }

    public class TestController5 : Controller
    {
        protected override void Construction()
        {
        }

        void Start()
        {
            deferred_create_object<TestController6>("KDJFKDJ");
        }
    }

    public class TestController6 : Controller
    {
        protected override void Construction()
        {
        }

        int u = 0;
        void Start()
        {
            deferred_create_object<TestController7>(u++.ToString());
            deferred_create_object<TestController7>(u++.ToString());
            deferred_create_object<TestController7>(u++.ToString());
            deferred_create_object<TestController7>(u++.ToString());
            deferred_create_object<TestController7>(u++.ToString());
            deferred_create_object<TestController7>(u++.ToString());
            deferred_create_object<TestController7>(u++.ToString());

            destroy();
        }
    }

    public class TestController7 : Controller
    {
        protected override void Construction()
        {
            obj<TestController8>(1.ToString());
            obj<TestController8>(2.ToString());
            obj<TestController8>(3.ToString());
            obj<TestController8>(4.ToString());
            obj<TestController8>(5.ToString());
        }

        void Configurate()
        {
            destroy();
        }
    }

    public class TestController8 : Controller
    {
        IInput<int> input1, input2, input3, input4;
        protected override void Construction()
        {
            add_handler<TestHandler77, int>(ref input1, "TestHandler1");
            add_handler<TestHandler77, int>(ref input2, "TestHandler2");
            add_handler<TestHandler77, int>(ref input3, "TestHandler3");
            add_handler<TestHandler77, int>(ref input4, "TestHandler4");
        }

        void Stop()
        {
            destroy();
        }
    }

    public class TestHandler77 : Handler<int, int>
    {
        protected override void Construction()
        {
            input_to(output);
        }

        void Start()
        {
            destroy();
        }
    }
}
