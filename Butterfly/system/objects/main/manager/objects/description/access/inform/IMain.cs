namespace Butterfly.system.objects.main.manager.objects.description.access.inform
{
    /// <summary>
    /// Описывает способы информирования обьекта.
    /// </summary>
    public interface IMain
    {
        /// <summary>
        /// Информирует родительский обьект о том что обьект был создан.
        /// И его можно уничтожить.
        /// </summary>
        public void InformCreateChildrenObjectToParentObject();
    }
}
