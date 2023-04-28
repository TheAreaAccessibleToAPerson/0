namespace Butterfly.system.objects.main.objects.manager.events
{
    public class Object<ParamType>
    {
        private global::System.Action<ParamType>[] ActionArray = new System.Action<ParamType>[0];

        public void Add(global::System.Action<ParamType> pAction)
        {
            ActionArray = Hellper.ExpendArray(ActionArray, pAction);
        }

        public void Run(ParamType pValue)
        {
            for (int i = 0; i < ActionArray.Length; i++)
            {
                ActionArray[i].Invoke(pValue);
            }
        }
    }

    
}
