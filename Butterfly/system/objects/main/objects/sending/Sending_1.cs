namespace Butterfly.system.objects.main.objects.sending
{
    public enum InformingType
    {
        /// <summary>
        /// Обьект подписан и начал свою работу.
        /// </summary>
        EndSubscribe = 1,

        /// <summary>
        /// Обькт окончил свою работу и был отписан.
        /// </summary>
        EndUnsubscribe = 2,

        /// <summary>
        /// Обьект изменил свою позицию.
        /// </summary>
        ChangeOfIndex = 4
    }

    public class Object<ValueType> : Informing, IInput<ValueType>, 
        objects.description.access.get.IInformationCreatingObject, description.registration.ISending
    {
        /// <summary>
        /// Уникальный ID обьекта Sending
        /// </summary>
        public readonly ulong ID;

        /// <summary>
        /// Имя под которым мы создали текущюю рассылку.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Указывает на метод отвечающий за процесс работы.
        /// Стандартный метод DefaultInput. Поток который будет вводить данные на рассылку
        /// возмет на себя всю работу. Если данный обьект поставить в пулл, то будет
        /// указан метод Add хранилища. После пулл извлекет от туда данные и разашлет их.
        /// </summary>
        public global::System.Action<ValueType> Action;

        /// <summary>
        /// Хранит ID пространсва(NodeObject) в нутри которого был создан данный обьект.
        /// </summary>
        public readonly ulong CreatorNodeObjectID;
        /// <summary>
        /// Хранит ID обьекта в нутри которого был создан данный обьект.
        /// </summary>
        public readonly ulong CreatorObjectID;
        /// <summary>
        /// Хранит Explorer обьекта в нутри которого был создан данный обьект.
        /// </summary>
        public readonly string CreatorObjectExplorer;
        
        private readonly object ActionRunLocker = new object();
        private readonly object SubscribeUnsubscribeLocker = new object();

        private readonly object ProcessSubscribeUnsubscribeLocker = new object();
        private bool IsProcessSubscribeUnsubscribe = false;

        /// <summary>
        /// Хранилище подписываемых/регистрируемых обьектов.
        /// 1) ID подписываемого Node обьека в нутри которого был создан ListinerSending.
        /// 2) Index в массиве класса subscribe.objects.Global в нутри которого хранятся данные подписываемого ListinerSending.
        /// 3) Метод описывающий ввод данных в ListinerSending.
        /// 4) Метод предназначеный для информирования subscribe.objects.Global. 
        ///     4.1) Первым параметром принимает данные описаные во 2) пункте.
        ///     4.2) Вторым параметром принимает Range массива в котором зарегестрировался или куда переместился в классе Listiner(текущем).
        ///     4.3) Третим параметом принимает индекс в массиве в котором зарегестрировался или куда переместился в классе Listiner(текущем).
        /// 5) 
        /// </summary>
        private readonly collections.safe.Values<ulong, int, object, global::System.Action<ulong, InformingType, int, int, int>> SubscribeObjectValues
            = new collections.safe.Values<ulong, int, object, global::System.Action<ulong, InformingType, int, int, int>>();

        /// <summary>
        /// Хранилище отписываемых обьектов.
        /// 1) Принимает ID подписываемого Node обьека в нутри которого был создан ListinerSending.
        /// 2) Index в массиве класса subscribe.objects.Global в нутри которого хранятся данные подписываемого ListinerSending.
        /// 3) Принимает Range массива в котором зарегестрировался или куда переместился в классе Listiner(текущем).
        /// 4) Индекс в массиве в котором зарегестрировался или куда переместился в классе Listiner(текущем).
        /// 5) 
        /// </summary>
        private readonly collections.safe.Values<ulong, int, int, int> UnsubscribeObjectValues
            = new collections.safe.Values<ulong, int, int, int>();

        private readonly manager.Clients<ValueType> ClientsManager;

        public Object(string pName, IInforming pInforming, string pCreatorObjectExplorer, ulong pCreatorNodeObjectID, ulong pCreatorObjectID, int pArrayLength) 
            : base("Sending_1", pInforming)
        {
            Action = DefaultInput;

            ID = Object<byte>.CreatingID();

            ClientsManager = new manager.Clients<ValueType>(pInforming, pArrayLength);

            CreatorObjectExplorer = pCreatorObjectExplorer;
            CreatorNodeObjectID = pCreatorNodeObjectID;
            CreatorObjectID = pCreatorObjectID;
            
        }

        void description.registration.ISending.Subscribe(ulong pIDObject, int pIndexInSendingManager, object pInputObject,
            global::System.Action<ulong, InformingType, int, int, int> pToInformingObject)
        {
            if (global::System.Threading.Monitor.TryEnter(ActionRunLocker))
            {
                // Нам удалось захватить локер рассылающий сообщения.

                // Попробуем захватить локер отвечающий за регестрацию клинтов...
                if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
                {
                    // Произведем регистрацию самостоятельно из системного потока.
                    ClientsManager.Subscribe(pIDObject, pIndexInSendingManager, pInputObject, pToInformingObject);

                    global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
                }
                else
                {
                    // Это означает что уже происходит регистрация клинтов из потока которые передает сюда данные.
                    // Дождемся пока она закончится и зарегестрируемя из системного потока.
                    lock (SubscribeUnsubscribeLocker)
                    {
                        ClientsManager.Subscribe(pIDObject, pIndexInSendingManager, pInputObject, pToInformingObject);
                    }
                }

                global::System.Threading.Monitor.Exit(ActionRunLocker);
            }
            else if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
            {
                lock (ProcessSubscribeUnsubscribeLocker)
                {
                    if (IsProcessSubscribeUnsubscribe == false)
                    {
                        SubscribeObjectValues.Add(pIDObject, pIndexInSendingManager, pInputObject, pToInformingObject);

                        global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);

                        return;
                    }
                    else
                    {
                        lock (SubscribeUnsubscribeLocker)
                        {
                            ClientsManager.Subscribe(pIDObject, pIndexInSendingManager, pInputObject, pToInformingObject);
                        }
                    }
                }

                global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
            }
            else
            {
                lock (SubscribeUnsubscribeLocker)
                {
                    ClientsManager.Subscribe(pIDObject, pIndexInSendingManager, pInputObject, pToInformingObject);
                }
            }
        }

        void description.registration.ISending.Unsubscribe(ulong pIDObject, int pIndexInSendingManager, int pIndexRangeInArray, int pIndexPositionInArray)
        {
            if (global::System.Threading.Monitor.TryEnter(ActionRunLocker))
            {
                // Нам удалось захватить локер рассылающий сообщения.

                // Попробуем захватить локер отвечающий за регестрацию клинтов...
                if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
                {
                    // Произведем регистрацию самостоятельно из системного потока.
                    ClientsManager.Unsubscribe(pIDObject, pIndexInSendingManager, pIndexRangeInArray, pIndexPositionInArray);

                    global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
                }
                else
                {
                    // Это означает что уже происходит регистрация клинтов из потока которые передает сюда данные.
                    // Дождемся пока она закончится и зарегестрируемя из системного потока.
                    lock (SubscribeUnsubscribeLocker)
                    {
                        ClientsManager.Unsubscribe(pIDObject, pIndexInSendingManager, pIndexRangeInArray, pIndexPositionInArray);
                    }
                }

                global::System.Threading.Monitor.Exit(ActionRunLocker);
            }
            else if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
            {
                lock (ProcessSubscribeUnsubscribeLocker)
                {
                    if (IsProcessSubscribeUnsubscribe == false)
                    {
                        UnsubscribeObjectValues.Add(pIDObject, pIndexInSendingManager, pIndexRangeInArray, pIndexPositionInArray);

                        global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);

                        return;
                    }
                    else
                    {
                        lock (SubscribeUnsubscribeLocker)
                        {
                            ClientsManager.Unsubscribe(pIDObject, pIndexInSendingManager, pIndexRangeInArray, pIndexPositionInArray);
                        }
                    }
                }

                global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
            }
            else
            {
                lock (SubscribeUnsubscribeLocker)
                {
                    ClientsManager.Unsubscribe(pIDObject, pIndexInSendingManager, pIndexRangeInArray, pIndexPositionInArray);
                }
            }
        }
    

        void IInput<ValueType>.ToInput(ValueType pValue)
        {
            Action(pValue);
        }

        private void DefaultInput(ValueType pValue)
        {
            lock (ProcessSubscribeUnsubscribeLocker) IsProcessSubscribeUnsubscribe = false;

            lock (ActionRunLocker)
            {
                ClientsManager.ActionRun(pValue);   
            }

            lock (SubscribeUnsubscribeLocker)
            {
                lock (ProcessSubscribeUnsubscribeLocker) IsProcessSubscribeUnsubscribe = true;

                if (UnsubscribeObjectValues.ExtractAll(out ulong[] after_oIDUnsubscribeObjects, out int[] after_oUnsubscribeIndexObjectsInSendingManager,
                                out int[] after_oUnsubscribeRangeObjects, out int[] after_oUnsubscribeIndexObjects))
                {
                    ClientsManager.Unsubscribe(after_oIDUnsubscribeObjects, after_oUnsubscribeIndexObjectsInSendingManager, after_oUnsubscribeRangeObjects, after_oUnsubscribeIndexObjects);
                }

                if (SubscribeObjectValues.ExtractAll(out ulong[] after_oIDSubscribeObjects, out int[] after_oSubscribeIndexObjectsInSendingManager,
                    out object[] after_oInputSubscribeObjects, out global::System.Action<ulong, InformingType, int, int, int>[] after_oActionInformingSubscribeObjects))
                {
                    ClientsManager.Subscribe(after_oIDSubscribeObjects, after_oSubscribeIndexObjectsInSendingManager, after_oInputSubscribeObjects, after_oActionInformingSubscribeObjects, true, pValue);
                }
            }
        }

        public string GetExplorerObject()
        {
            return CreatorObjectExplorer;
        }

        public ulong GetIDNodeObject()
        {
            return CreatorNodeObjectID;
        }

        public ulong GetIDObject()
        {
            return CreatorObjectID;
        }

        private static ulong SendingID = 0;
        private static object LockerSendingID = new object();
        private static ulong CreatingID()
        {
            lock(LockerSendingID)
            {
                return SendingID++;
            }
        }
    }
}
