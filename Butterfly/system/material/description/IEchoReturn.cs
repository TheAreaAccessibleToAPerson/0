namespace Butterfly
{
    /// <summary>
    /// Данный интерфес реализует отправку эхо братно в прослушивающий .await с явным указанием типа.
    /// </summary>
    public interface IEchoReturn<ValueType>
    {
        /// <summary>
        /// Отправить ответ.
        /// </summary>
        /// <param name="pValue"></param>
        public void To(ValueType pValue);
        /// <summary>
        /// Отправить ответ безопасно, с проверкой на состояние ожидающего.
        /// </summary>
        /// <param name="pValue"></param>
        public void SafeTo(ValueType pValue);

        /// <summary>
        /// Получает уникальный id echo.
        /// </summary>
        /// <returns></returns>
        public ulong GetID();

        /// <summary>
        /// Уникальный id обьекта использующий echo.
        /// </summary>
        /// <returns></returns>
        public ulong GetObjectID();

        /// <summary>
        /// Уникальный номер узла использующий echo.
        /// </summary>
        /// <returns></returns>
        public ulong GetNodeObjectID();

        /// <summary>
        /// Номер вложености узла обьекта в контексте всей системы использующий echo.
        /// </summary>
        /// <returns></returns>
        public int GetObjectAttachmentNodeNumberInSystem();

        /// <summary>
        /// Номер вложенести обьекта в нутри узла использующий echo.
        /// </summary>
        /// <returns></returns>
        public int GetObjectAttackmentNumberObjectInNode();
    }
}
