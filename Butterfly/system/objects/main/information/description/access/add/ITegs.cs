namespace Butterfly.system.objects.main.information.description.access.add
{
    /// <summary>
    /// Описываем способ добовления тегов.
    /// </summary>
    public interface ITegs
    {
        /// <summary>
        /// Добовляет теги в information.Tegs.
        /// </summary>
        /// <param name="pTegs"></param>
        public void AddTegs(params string[] pTegs);
    }
}
