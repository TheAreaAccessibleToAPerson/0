namespace Butterfly.system.objects.main.objects.description.access.get
{
    /// <summary>
    /// Описывает метод получения Explorer и ID пространсва в нутри которого был создан обьект.
    /// </summary>
    public interface IInformationCreatingObject
    {
        /// <summary>
        /// Возращает Explorer обьекта в нутри которого был создан данный обьект.
        /// </summary>
        /// <returns></returns>
        public string GetExplorerObject();
        /// <summary>
        /// Возращает ID пространсва(NodeObject) в нутри которого был создан данный обьект.
        /// </summary>
        /// <returns></returns>
        public ulong GetIDNodeObject();

        /// <summary>
        /// Возращает ID обьекта в нутри которого был создан данный обьект.
        /// </summary>
        /// <returns></returns>
        public ulong GetIDObject();
    }
}
