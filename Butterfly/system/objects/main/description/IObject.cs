namespace Butterfly.system.objects.main.description
{
    public interface IObject
    {
        /// <summary>
        /// Описывает работу с пулом.
        /// </summary>
        public interface Poll
        {
            /// <summary>
            /// Подписывает в пуллы System.Action.
            /// </summary>
            public void Subscribe(System.Action pAction, Poll pPollType, uint pPollSize, uint pTimeDelay, string pPollName);
        }
    }
}
