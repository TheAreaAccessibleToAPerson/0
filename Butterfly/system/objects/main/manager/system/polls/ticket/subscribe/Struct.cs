namespace Butterfly.system.objects.main.manager.system.polls.ticket.subscribe
{
    public struct Struct
    {
        public global::System.Action Action;
        public uint Size;
        public uint TimeDelay;
        public string Name;

        public Struct(global::System.Action pAction, uint pSize, uint pTimeDelay, string pName)
        {
            Action = pAction;
            Size = pSize;
            TimeDelay = pTimeDelay;
            Name = pName;
        }
    }
}
