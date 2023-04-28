namespace Butterfly
{
    public class Hellper
    {
        public static string GetName(System.Reflection.MethodInfo pMethodInfo)
        {
            string result = pMethodInfo.Name;

            System.Reflection.ParameterInfo[] parametrs = pMethodInfo.GetParameters();

            if (parametrs.Length > 0)
            {
                result += "(";

                for (int i = 0; i < parametrs.Length; i++)
                {
                    result += parametrs[i].ParameterType;
                    result += " " + parametrs[i].Name;

                    if (i + 1 < parametrs.Length) result += ", ";
                }

                result += ")";
            }

            return result;
        }

        /// <summary>
        /// Обьеденяем массивы.
        /// </summary>
        /// <param name="pArray1"></param>
        /// <param name="pArray2"></param>
        /// <returns></returns>
        public static ulong[] ConcatArray(ulong[] pArray1, ulong[] pArray2)
        {
            ulong[] result = new ulong[pArray1.Length + pArray2.Length];

            pArray1.CopyTo(result, 0);
            pArray2.CopyTo(result, pArray1.Length);

            return result;
        }

        public static ValueType[] ConcatArray<ValueType>(ValueType[] pArray1, ValueType[] pArray2)
        {
            ValueType[] result = new ValueType[pArray1.Length + pArray2.Length];

            pArray1.CopyTo(result, 0);
            pArray2.CopyTo(result, pArray1.Length);

            return result;
        }

        public static ulong[] ExpendArray(ulong[] pArray1, ulong pValue)
        {
            ulong[] result = new ulong[pArray1.Length + 1];

            pArray1.CopyTo(result, 0);
            result[pArray1.Length] = pValue;

            return result;
        }

        public static ValueType[] ExpendArray<ValueType>(ValueType[] pArray1, ValueType pValue)
        {
            ValueType[] result = new ValueType[pArray1.Length + 1];

            pArray1.CopyTo(result, 0);
            result[pArray1.Length] = pValue;

            return result;
        }

        public static ValueType[][] ExpendRange<ValueType>(ValueType[][] pArray, int pArrayLength)
        {
            ValueType[][] result = new ValueType[pArray.Length + 1][];

            for (int i = 0; i < pArray.Length; i++)
            {
                result[i] = pArray[i];
            }

            result[pArray.Length] = new ValueType[pArrayLength];

            return result;
        }

        public static bool GetSystemMethod(string pMethodName, System.Type pType, out System.Reflection.MethodInfo oSystemMethod)
        {
            oSystemMethod = pType.GetMethod(pMethodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public);

            if (oSystemMethod == null)
                return false;
            else
                return true;
        }
    }
}
