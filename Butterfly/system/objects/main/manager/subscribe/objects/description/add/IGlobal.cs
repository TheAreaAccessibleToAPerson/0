namespace Butterfly.system.objects.main.manager.subscribe.objects.description.add
{
    /// <summary>
    /// Описывает регистрацию на подписку в глобальные обьекты.
    /// </summary>
    public interface IGlobal
    {
        /// <summary>
        /// Регистрирует подписку обьекта прослушивающего рассылку сообщений.
        /// </summary>
        /// <typeparam name="SendingValueType"></typeparam>
        /// <param name="pGlobalSendingMessageObject"></param>
        /// <param name="pListenerSendingMessageObject"></param>
        public void RegisterListenerObjectInSendingMessageObject<SendingValueType>
            (main.objects.sending.Object<SendingValueType> pGlobalSendingMessageObject,
                main.objects.sending.listener.Object<SendingValueType> pListenerSendingMessageObject);
    }
}
