namespace Butterfly.system.objects.SYSTEM.objects.description.access
{
    /// <summary>
    /// Описывает возможные способы доступа к системным возможностям.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Добавляет Action на запуск.
        /// </summary>
        /// <param name="pAction"></param>
        public void AddActionInvoke(global::System.Action pAction);

        /// <summary>
        /// Регистрируемся на подписку/отписку в пуллы.
        /// </summary>
        public void AddTicketsToThePoll(poll.data.ticket.Struct[] pPollTickets);

        public void Destroy() { }
    }
}
