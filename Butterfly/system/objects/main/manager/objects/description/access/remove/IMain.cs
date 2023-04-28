namespace Butterfly.system.objects.main.manager.objects.description.access.remove
{
    public interface IMain
    {
        /// <summary>
        /// Удаляет обьект по ключу и типу.
        /// </summary>
        /// <param name="pKey"></param>
        public void Remove<MainObjectType>(string pKey);

        /// <summary>
        /// Удаляет обьект по ключу и типу.
        /// </summary>
        /// <param name="pKey"></param>
        public void Remove(string pKey, global::System.Type pType);
    }
}
