namespace Butterfly.system.objects.main.manager.objects.description.access.get
{
    /// <summary>
    /// Описывает методы получения общих обьектов. Global and node object.
    /// </summary>
    public interface IShared
    {
        /// <summary>
        /// Получить глобальный обьект по ключу <paramref name="pKey"/>.
        /// </summary>
        /// <param name="pKey"></param>
        /// <returns></returns>
        bool GlobalTryGet(string pKey, out object oObjectValue);
    }
}
