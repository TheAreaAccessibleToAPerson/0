namespace Butterfly.system.objects.main.local.value
{
    public abstract class Object : main.Object
    {
        public abstract class Value<ValueType> : Object, description.access.set.IValue
        {
            public ValueType localValue { private set; get; }

            private bool IsEmpty = true;

            bool description.access.set.IValue.TrySet(object pObjectValue)
            {
                if (IsEmpty != true) return false;

                if (pObjectValue is ValueType localObjectValueReduse)
                {
                    localValue = localObjectValueReduse;

                    return true;
                }
                else
                    Exception(Ex.LocalValue.x10002, pObjectValue.GetType().FullName, typeof(ValueType).FullName);

                return false;
            }

            bool description.access.set.IValue.IsEmpty() => localValue == null;

            string description.access.set.IValue.GetValueType() => typeof(ValueType).FullName;
        }
    }

    namespace thread
    {
        public abstract class Object : main.thread.Object
        {
            public abstract class Value<ValueType> : Object, description.access.set.IValue
            {
                public ValueType localValue { private set; get; }

                private bool IsEmpty = true;

                bool description.access.set.IValue.TrySet(object pObjectValue)
                {
                    if (IsEmpty != true) return false;

                    if (pObjectValue is ValueType localObjectValueReduse)
                    {
                        localValue = localObjectValueReduse;
                        
                        return true;
                    }
                    else
                    {
                        
                        Exception(Ex.LocalValue.x10002, pObjectValue.GetType().FullName, typeof(ValueType).FullName);
                    }
                        

                    return false;
                }

                bool description.access.set.IValue.IsEmpty() => localValue == null;

                string description.access.set.IValue.GetValueType() => typeof(ValueType).FullName;
            }
        }
    }
}
