namespace Butterfly.system.objects.collections.safe
{
    public class Values<ValueType>
    {
        private readonly global::System.Collections.Generic.List<ValueType> ValueList 
            = new global::System.Collections.Generic.List<ValueType>();

        private readonly object Locker = new object();

        public int Count { get { lock (Locker) { return ValueList.Count; } } }

        public void Add(ValueType pValue)
        {
            lock (Locker)
            {
                ValueList.Add(pValue);
            }
        }

        /// <summary>
        /// Извлекает значения в <paramref name="oResult"/>.
        /// </summary>
        /// <param name="oResult"><typeparamref name="ValueType"/></param>
        /// <returns>Возращает true если были извлечены значение и false если значений нету.</returns>
        public bool ExtractAll(out ValueType[] oResult)
        {
            oResult = null;

            if (ValueList.Count == 0)
            {
                return false;
            }
            else
            {
                lock (Locker)
                {
                    if (ValueList.Count == 0)
                    {
                        //...
                    }
                    else
                    {
                        oResult = ValueList.ToArray();

                        ValueList.Clear();

                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryExtractAll(out ValueType[] oResult)
        {
            oResult = null;

            bool result = false;
            {
                if (ValueList.Count == 0)
                {
                    //...
                }
                else
                {
                    if (System.Threading.Monitor.TryEnter(Locker))
                    {
                        if (ValueList.Count == 0)
                        {
                            //...
                        }
                        else
                        {
                            oResult = ValueList.ToArray();

                            ValueList.Clear();

                            result = true;
                        }

                        System.Threading.Monitor.Exit(Locker);
                    }
                }
            }
            return result;
        }
    }
}

