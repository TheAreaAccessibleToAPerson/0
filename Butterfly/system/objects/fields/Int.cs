namespace Butterfly.system.objects.fields
{
    public class Int
    {
        private int Value = 0;

        public void Increment() => Value++;
        public void Decriment() => Value--;

        public int Get() => Value;

        public void Set(int pValue) => Value = pValue;
    }
}
