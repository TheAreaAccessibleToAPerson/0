namespace Butterfly.system.objects.main.information.description.access.get
{
    /// <summary>
    /// Описывает методы для получения общей информации о текущем обьекте.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Уникальный ID обьекта.
        /// </summary>
        /// <returns></returns>
        public ulong GetID();

        /// <summary>
        /// Уникальный ID всего узла.
        /// </summary>
        /// <returns></returns>
        public ulong GetNodeID();

        /// <summary>
        /// Номер вложености узла в контексте всей системы.
        /// </summary>
        /// <returns></returns>
        public int GetAttachmentNodeNumberInSystem();

        /// <summary>
        /// Номер вложенести обьекта в нутри узла.
        /// </summary>
        /// <returns></returns>
        public int GetAttackmentNumberObjectInNode();

        /// <summary>
        /// Ключ по которому создовался обьект. Под данным ключом у родительского обьекта записан текущий обьект.
        /// </summary>
        /// <returns></returns>
        public string GetKey();
    }
}
