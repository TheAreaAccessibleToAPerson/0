namespace Butterfly.system.objects.collections.safe
{
    class String
    {
        private readonly object Locker;

        public string Value { private set; get; } = "";

        public String(string pValue)
        {
            Value = pValue;
            Locker = new object();
        }

        public String(string pValue, ref object pLockerObject)
        {
            Value = pValue;
            Locker = pLockerObject;
        }

        public string Get()
        {
            return Value;
        }

        public string Set(string pValue)
        {
            lock (Locker)
            {
                return Value = pValue;
            }
        }

        public bool Compare(string pValue)
        {
            lock (Locker)
            {
                if (pValue == Value)
                    return true;
                return false;
            }
        }

        public bool Replace(string pReplaceValue, params string[] pValueArray)
        {
            lock (Locker)
            {
                foreach (string value in pValueArray)
                {
                    if (value == Value)
                    {
                        Value = pReplaceValue;

                        return true;
                    }
                }

                return false;
            }

        }
    }
}
