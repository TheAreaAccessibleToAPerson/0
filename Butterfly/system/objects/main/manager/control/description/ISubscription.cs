namespace Butterfly.system.objects.main.manager.control.description
{
    public interface ISubscription
    {
        /// <summary>
        /// Добовляем Actions через котороый нужно будет подписаться и отписаться.
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pSubscriveAction"></param>
        public void AddSubscribeAndUnsubscribe(SubscriptionType pType, global::System.Action pSubscriveAction, global::System.Action pUnubscriveAction);

        /// <summary>
        /// Сообщаем что подписание закончилось, и нужно закончить запуск обьекта.
        /// </summary>
        /// <param name="pType"></param>
        public void EndSubscribe(SubscriptionType pType);

        /// <summary>
        /// Сообщаем что отподписание закончилось, и нужно подписать следующий Action.
        /// Ответсвеность за сообщении об окончании отподписания лежит на обьекте который хранит все логику
        /// своей области задач.
        /// </summary>
        /// <param name="pType"></param>
        public void EndUnsubscribe(SubscriptionType pType);
    }
}
