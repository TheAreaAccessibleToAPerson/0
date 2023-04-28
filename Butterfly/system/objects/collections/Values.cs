using System;
using System.Collections.Generic;
using System.Text;

namespace Butterfly.system.objects.collections
{
    public class Values<ValueType>
    {
        private readonly global::System.Collections.Generic.List<ValueType> ValueList
            = new global::System.Collections.Generic.List<ValueType>();

        public int Count { get { return ValueList.Count; } }

        public void Clear()
        {
            ValueList.Clear();
        }

        public void Add(ValueType pValue)
        {
            ValueList.Add(pValue);
        }

        /// <summary>
        /// Извлекает значения в <paramref name="oResult"/>.
        /// </summary>
        /// <param name="oResult"><typeparamref name="ValueType"/></param>
        /// <returns>Возращает true если были извлечены значение и false если значений нету.</returns>
        public bool ExtractAll(out ValueType[] oResult)
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
                    oResult = ValueList.ToArray();

                    ValueList.Clear();

                    result = true;
                }
            }
            return result;
        }
    }
}
