namespace Butterfly.system.objects.main.objects.sending.manager
{
    public class Clients<ValueType> : main.Informing
    {
        /// <summary>
        /// ID зарегетрированого обьекта.
        /// </summary>
        private ulong[][] SubscribeIDObjects;
        /// <summary>
        /// Ссылка на прием данных обьекта.
        /// </summary>
        private IInput<ValueType>[][] InputObjectRun;
        /// <summary>
        /// Информируем обьект о конце регистрации и о текущем месте нахождения.
        /// После окончания регистрации и при перемещении тоже информируем обьект.
        /// Первым пораметром передаем номер регистрационого билета(Номер в массиве).
        /// Втором параметром номер ранга в массиве.
        /// Третим параметром номер индекса в массиве.
        /// </summary>
        private global::System.Action<ulong, InformingType, int, int, int>[][] ToInformingObjectsRun;
        /// <summary>
        /// Индекс в массиве класса subscribe.objects.Global где хранятся созданые ListinerSending.
        /// </summary>
        private int[][] IndexInListingSendingMenager;

        /// <summary>
        /// Размерность массива.
        /// </summary>
        private readonly int ArrayLength;

        // Пустой слот.
        private int EmptySlotRange = 0;
        private int EmptySlotIndex = 0;

        public Clients(IInforming pInforming, int pArrayLength)
            :base("Sending_1/ClientsManager", pInforming)
        {
            ArrayLength = pArrayLength;

            SubscribeIDObjects = new ulong[1][] { new ulong[pArrayLength] };
            IndexInListingSendingMenager = new int[1][] { new int[pArrayLength] };
            InputObjectRun = new IInput<ValueType>[1][] { new IInput<ValueType>[pArrayLength] };
            ToInformingObjectsRun = new global::System.Action<ulong, InformingType, int, int, int>[1][] 
                { new global::System.Action<ulong, InformingType, int, int, int>[pArrayLength] };
        }

        public void ActionRun(ValueType pValue)
        {
            for (int range = 0; range <= EmptySlotRange; range++)
            {
                for (int index = 0; index < EmptySlotIndex; index++)
                {
                    //Console($"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!EmptySlotInput{EmptySlotIndex} Range:{range}, Index:{index}.");
                    InputObjectRun[range][index].ToInput(pValue);
                }
            }
        }

        public void Subscribe(ulong[] pIDSubscribeObjects, int[] pSubscribeIndexObjectsInSendingManager,
                object[] pInputObjects, global::System.Action<ulong, InformingType, int, int, int>[] pActionInformingSubscribeObjects, 
                bool pIsAfter = false, ValueType pLastValue = default) // Если обьект был подписан после только что разоланых сообщений, то после того как мы его подпишем отправим сообщение для него.
        {
            for (int i = 0; i < pIDSubscribeObjects.Length; i++)
            {
                Subscribe(pIDSubscribeObjects[i], pSubscribeIndexObjectsInSendingManager[i], pInputObjects[i],
                    pActionInformingSubscribeObjects[i], false);
            }
        }

        public void Subscribe(ulong pIDSubscribeObject, int oSubscribeIndexObjectInSendingManager,
                object pInputObject, global::System.Action<ulong, InformingType, int, int, int> pActionInformingSubscribeObject,
                bool pIsAfter = false, ValueType pLastValue = default) // Если обьект был подписан после только что разоланых сообщений, то после того как мы его подпишем отправим сообщение для него.
        {
            IInput<ValueType> input = null;

            if (pInputObject is IInput<ValueType> inputObjectReduse)
            {
                input = inputObjectReduse;
            }
            else
                Exception(Ex.SendingMessage.x10001, pInputObject.GetType().FullName, typeof(IInput<ValueType>).FullName);

            // Если массивы где хранятся данные об ListinerSending обьектах заполнен,
            // то переключимся на следующий range, если его нету, то создадим новый.
            if (EmptySlotIndex == ArrayLength)
            {
                if ((EmptySlotRange + 1) < SubscribeIDObjects.Length) // Переключаемся на следующий range.
                {
                    EmptySlotRange++;
                }
                else if ((EmptySlotRange + 1) == SubscribeIDObjects.Length) // Создаем новый.
                {
                    SubscribeIDObjects = Hellper.ExpendRange(SubscribeIDObjects, ArrayLength);
                    InputObjectRun = Hellper.ExpendRange(InputObjectRun, ArrayLength);
                    ToInformingObjectsRun = Hellper.ExpendRange(ToInformingObjectsRun, ArrayLength);
                    IndexInListingSendingMenager = Hellper.ExpendRange(IndexInListingSendingMenager, ArrayLength);

                    // Переключимся на новый range.
                    EmptySlotRange++;
                }

                // Выставим индекс текущего индеса в ноль, в начало массива в только что созданом range.
                EmptySlotIndex = 0;
            }

            SubscribeIDObjects[EmptySlotRange][EmptySlotIndex] = pIDSubscribeObject;
            InputObjectRun[EmptySlotRange][EmptySlotIndex] = input;
            ToInformingObjectsRun[EmptySlotRange][EmptySlotIndex] = pActionInformingSubscribeObject;
            IndexInListingSendingMenager[EmptySlotRange][EmptySlotIndex] = oSubscribeIndexObjectInSendingManager;

            // Проинформируем клинта об окончании регистрации, он подписан.
            // Вышелм ему обратно индекс в массиве где он хранится в subscibe.objects.Global
            // И место хранения его данных в текущем классе.
            pActionInformingSubscribeObject.Invoke(pIDSubscribeObject, InformingType.EndSubscribe, oSubscribeIndexObjectInSendingManager, 
                EmptySlotRange, EmptySlotIndex);

            SystemInformation($"Обьект с ID:{pIDSubscribeObject} был подписан в позицию Range:{EmptySlotRange} Index:{EmptySlotIndex}.");

            if (pIsAfter) // Если данный клинт был подписан после разоланых сообщений, то продублируем недавнее сообщение.
            {
                //InputObjectRun[EmptySlotRange][EmptySlotIndex].ToInput(pLastValue);
            }
 
            EmptySlotIndex++;
        }

        public void Unsubscribe(ulong[] pIDUnsubscribeObjects, int[] pUnsubscribeIndexObjectsInSendingManager,
                int[] pUnsubscribeRangeObjects, int[] pUnsubscribeIndexObjects)
        {
            for (int i = 0; i < pIDUnsubscribeObjects.Length; i++)
            {
                Unsubscribe(pIDUnsubscribeObjects[i], pUnsubscribeIndexObjectsInSendingManager[i], pUnsubscribeRangeObjects[i], pUnsubscribeIndexObjects[i]);
            }
        }

        public void Unsubscribe(ulong pIDUnsubscribeObject, int pUnsubscribeIndexObjectInSendingManager,
                int pUnsubscribeRangeObject, int pUnsubscribeIndexObject)
        {
            // Если обьект все еще хранится в текущем месте.
            if (pIDUnsubscribeObject == SubscribeIDObjects[pUnsubscribeRangeObject][pUnsubscribeIndexObject])
            {
                
                // Из одного Node обьекта может быть несколько подписчиков.
                // Поэтому проверим его место положение менеджере подписок.
                if (pUnsubscribeIndexObjectInSendingManager == IndexInListingSendingMenager[pUnsubscribeRangeObject][pUnsubscribeIndexObject])
                {
                    
                    // Сообщаем обьекту.
                    ToInformingObjectsRun[pUnsubscribeRangeObject][pUnsubscribeIndexObject].Invoke(pIDUnsubscribeObject, InformingType.EndUnsubscribe,
                        pUnsubscribeIndexObjectInSendingManager, pUnsubscribeRangeObject, pUnsubscribeIndexObject);

                    SystemInformation($"Обьект с ID:{pIDUnsubscribeObject} был отпитсан с позиции Range:{pUnsubscribeRangeObject}, Index:{pUnsubscribeIndexObject}");

                    // Проверим небыл ли текущий обьект крйним.
                    if (EmptySlotRange == 0 && EmptySlotIndex == 0 && pUnsubscribeIndexObject == 0)
                    {
                        EmptySlot(pUnsubscribeRangeObject, pUnsubscribeIndexObject);

                        SystemInformation("Текущий обьект являлся единсвеным и последним.");

                        EmptySlotIndex--;
                       
                        return; // Данный клиент был единсвеный.
                    }
                    else if (pUnsubscribeIndexObject > (EmptySlotIndex - 1) && pUnsubscribeRangeObject == EmptySlotRange)
                    {                       
                        EmptySlot(EmptySlotRange, EmptySlotIndex - 1);

                        SystemInformation($"Позиция Range:{EmptySlotRange}, Index:{EmptySlotIndex - 1} была отчищена.");
                        
                        EmptySlotIndex--;

                        return; // Данный клиент был единсвеный.
                    }
                    else
                    {
                        // Меняем значение крайнего пустого слота.

                        // Если index указывает на 0 и унас больше одного ранга, то его нужно вернуть в значение равное длине массива,
                        // а значение ранга дикрементировать.
                        // Если же index больше нуля, то просто декриментируем значение index.

                        if (EmptySlotIndex > 0 && EmptySlotIndex <= ArrayLength)
                        {
                            EmptySlotIndex--;
                        }
                        else if (EmptySlotIndex == 0)
                        {
                            EmptySlotIndex = ArrayLength - 1;

                            if (EmptySlotRange > 0)
                            {
                                EmptySlotRange--;
                            }
                        }
                        else if (EmptySlotIndex == ArrayLength)
                        {
                        }
                    }


                    // Текущий обьект не указывает на конечное занчение.
                    // Записываем крайний обьект на только что освободившиеся место.
                    SubscribeIDObjects[pUnsubscribeRangeObject][pUnsubscribeIndexObject] = SubscribeIDObjects[EmptySlotRange][EmptySlotIndex];
                    InputObjectRun[pUnsubscribeRangeObject][pUnsubscribeIndexObject] = InputObjectRun[EmptySlotRange][EmptySlotIndex];
                    ToInformingObjectsRun[pUnsubscribeRangeObject][pUnsubscribeIndexObject] = ToInformingObjectsRun[EmptySlotRange][EmptySlotIndex];
                    IndexInListingSendingMenager[pUnsubscribeRangeObject][pUnsubscribeIndexObject] = IndexInListingSendingMenager[EmptySlotRange][EmptySlotIndex];

                    SystemInformation($"Обьект с ID:{SubscribeIDObjects[EmptySlotRange][EmptySlotIndex]} был перемещен с позиции" +
                        $"Range:{EmptySlotRange} Index:{EmptySlotIndex} в Range{pUnsubscribeRangeObject} Index{pUnsubscribeIndexObject}.");

                    // Сообщаем клиенту его новое место.
                    ToInformingObjectsRun[pUnsubscribeRangeObject][pUnsubscribeIndexObject].Invoke(pIDUnsubscribeObject, InformingType.ChangeOfIndex,
                        pUnsubscribeIndexObjectInSendingManager, pUnsubscribeRangeObject, pUnsubscribeIndexObject);

                    // Отчищаем данный из крайнего слота, который мы только что переместили.
                    EmptySlot(EmptySlotRange, EmptySlotIndex);

                    SystemInformation($"Позиция Range:{EmptySlotRange}, Index:{EmptySlotIndex} была отчищена.");

                    SystemInformation($"Свободный слот для записи Range:{EmptySlotRange} Index:{EmptySlotIndex}");
                }
            }
        }

        private void EmptySlot(int pRangeSlot, int pIndexSlot)
        {
            // Очищаем все данные об обьекте.
            SubscribeIDObjects[pRangeSlot][pIndexSlot] = ulong.MaxValue;
            InputObjectRun[pRangeSlot][pIndexSlot] = null;
            ToInformingObjectsRun[pRangeSlot][pIndexSlot] = null;
            IndexInListingSendingMenager[pRangeSlot][pIndexSlot] = int.MaxValue;
        }
    }
}
