namespace Butterfly.system.objects.collections.safe
{
    public class Values<ValueType1, ValueType2, ValueType3, ValueType4>
    {
        private readonly global::System.Collections.Generic.List<ValueType1> ValueList1
            = new global::System.Collections.Generic.List<ValueType1>();

        private readonly global::System.Collections.Generic.List<ValueType2> ValueList2
            = new global::System.Collections.Generic.List<ValueType2>();

        private readonly global::System.Collections.Generic.List<ValueType3> ValueList3
            = new global::System.Collections.Generic.List<ValueType3>();

        private readonly global::System.Collections.Generic.List<ValueType4> ValueList4
            = new global::System.Collections.Generic.List<ValueType4>();

        private readonly object Locker = new object();

        public int Count { private set { } get { return ValueList1.Count; } }

        public void Add(ValueType1 pValue1, ValueType2 pValue2, ValueType3 pValue3, ValueType4 pValue4)
        {
            lock (Locker)
            {
                ValueList1.Add(pValue1);
                ValueList2.Add(pValue2);
                ValueList3.Add(pValue3);
                ValueList4.Add(pValue4);
            }
        }

        public bool ExtractAll(out ValueType1[] oResult1, out ValueType2[] oResult2, out ValueType3[] oResult3, out ValueType4[] oResult4)
        {
            oResult1 = null;
            oResult2 = null;
            oResult3 = null;
            oResult4 = null;

            bool result = false;
            {
                if (ValueList1.Count == 0)
                {
                    result = false;
                }
                else
                {
                    lock (Locker)
                    {
                        if (ValueList1.Count == 0)
                        {
                            result = false;
                        }
                        else
                        {
                            oResult1 = ValueList1.ToArray();
                            oResult2 = ValueList2.ToArray();
                            oResult3 = ValueList3.ToArray();
                            oResult4 = ValueList4.ToArray();


                            ValueList1.Clear();
                            ValueList2.Clear();
                            ValueList3.Clear();
                            ValueList4.Clear();

                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private int FailedAccesses = 0;

        public bool TryExtractAll(out ValueType1[] oResult1, out ValueType2[] oResult2, out ValueType3[] oResult3, out ValueType4[] oResult4)
        {
            oResult1 = null;
            oResult2 = null;
            oResult3 = null;
            oResult4 = null;

            bool result = false;
            {
                if (ValueList1.Count == 0)
                {

                }
                else
                {
                    if (System.Threading.Monitor.TryEnter(Locker))
                    {
                        if (ValueList1.Count == 0)
                        {

                        }
                        else
                        {
                            oResult1 = ValueList1.ToArray();
                            oResult2 = ValueList2.ToArray();
                            oResult3 = ValueList3.ToArray();
                            oResult4 = ValueList4.ToArray();

                            ValueList1.Clear();
                            ValueList2.Clear();
                            ValueList3.Clear();
                            ValueList4.Clear();

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
