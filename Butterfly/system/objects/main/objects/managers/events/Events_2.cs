namespace Butterfly.system.objects.main.objects.manager.events
{
    public class Object<ParamType1, ParamType2>
    {
        private global::System.Action<ParamType1, ParamType2>[] ActionArray = new System.Action<ParamType1, ParamType2>[0];

        public void Add(global::System.Action<ParamType1, ParamType2> pAction)
        {
            ActionArray = Hellper.ExpendArray(ActionArray, pAction);
        }

        public void Run(ParamType1 pValue1, ParamType2 pValue2)
        {
            for (int i = 0; i < ActionArray.Length; i++)
            {
                ActionArray[i].Invoke(pValue1, pValue2);
            }
        }
    }
}
