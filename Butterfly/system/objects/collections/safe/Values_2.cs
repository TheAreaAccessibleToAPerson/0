namespace Butterfly.system.objects.collections.safe
{
    public class Values<ValueType1, ValueType2>
    {
        private readonly global::System.Collections.Generic.List<ValueType1> ValueList1
            = new global::System.Collections.Generic.List<ValueType1>();

        private readonly global::System.Collections.Generic.List<ValueType2> ValueList2
            = new global::System.Collections.Generic.List<ValueType2>();

        private readonly object Locker = new object();

        public void Add(ValueType1 pValue1, ValueType2 pValue2)
        {
            lock (Locker)
            {
                ValueList1.Add(pValue1);
                ValueList2.Add(pValue2);
            }
        }

        /// <summary>
        /// Извлекает значения в <paramref name="oResult1"/> и в <paramref name="oResult2"/>.
        /// </summary>
        /// <param name="oResult1"><typeparamref name="ValueType1"/></param>
        /// <param name="oResult2"><typeparamref name="ValueType2"/></param>
        /// <returns>Возращает true если были извлечены значение и false если значений нету.</returns>
        public bool ExtractAll(out ValueType1[] oResult1, out ValueType2[] oResult2)
        {
            oResult1 = null;
            oResult2 = null;

            bool result = false;
            {
                if (ValueList1.Count == 0)
                {
                    //...
                }
                else
                {
                    lock (Locker)
                    {
                        if (ValueList1.Count == 0)
                        {
                            //...
                        }
                        else
                        {
                            ValueType1[] array1 = new ValueType1[ValueList1.Count];
                            ValueType2[] array2 = new ValueType2[ValueList2.Count];
                            {
                                for (int i = 0; i < ValueList1.Count; i++)
                                {
                                    array1[i] = ValueList1[i];
                                    array2[i] = ValueList2[i];
                                }
                            }

                            oResult1 = array1;
                            oResult2 = array2;

                            ValueList1.Clear();
                            ValueList2.Clear();

                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}
