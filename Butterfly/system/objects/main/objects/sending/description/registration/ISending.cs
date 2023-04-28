namespace Butterfly.system.objects.main.objects.sending.description.registration
{
    /// <summary>
    /// Описывает способ подписки на Sending.
    /// </summary>
    public interface ISending
    {
        /// <summary>
        /// Выставить обьект на подписку.
        /// </summary>
        /// <param name="pIDObject">ID подписываемого ListinerSending.</param>
        /// <param name="pListinerIndexInSubscribeManager">Индекс в массиве который находится в SubscribeManager.</param>
        /// <param name="pInputObject">Ссылка на ввод данных в ListinerSending.</param>
        /// <param name="pToInformingObject">Ссылка на метод который принимает информационые данные.</param>
        public void Subscribe(ulong pIDObject, int pListinerIndexInSubscribeManager, object pInputObject,
            global::System.Action<ulong, InformingType, int, int, int> pToInformingObject);

        /// <summary>
        /// Выставить обьект на отписку.
        /// </summary>
        /// <param name="pIDObject">ID отписываемого обьекта.</param>
        /// <param name="pIndexInSendingMessage">Index в массиве SendingMessage</param>
        /// <param name="pEmptySlotRang">Ранг в многомерном массиве в котором обьект предположительно находится.</param>
        /// <param name="pEmptySlotIndex">Индекс в многомерном массиве в котором обьект предположительно находится.</param>
        public void Unsubscribe(ulong pIDObject, int pIndexInSendingMessage, int pEmptySlotRang, int pEmptySlotIndex);
    }
}
