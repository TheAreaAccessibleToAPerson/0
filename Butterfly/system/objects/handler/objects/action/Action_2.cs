namespace Butterfly.system.objects.handler.objects.action
{
    class Object<ParamType1, ParamType2> : IInput<ParamType1, ParamType2>
    {
        private readonly global::System.Action<ParamType1, ParamType2> Action, Event;

        public Object(global::System.Action<ParamType1, ParamType2> pAction)
        {
            Action = pAction;
        }

        public void ToInput(ParamType1 pValue1, ParamType2 pValue2)
        {
            Action.Invoke(pValue1, pValue2);
        }
    }
}
