namespace Butterfly.system.objects.main.manager.objects.description.access.set
{
    /// <summary>
    /// Описываем методы передачи глобальных данных.
    /// </summary>
    interface IShared
    {
        /// <summary>
        /// Принимает глобальные обьекты от родителя.
        /// </summary>
        /// <param name="pValues"></param>
        void GlobalSet(System.Collections.Generic.Dictionary<string, object> pValues);
    }
}
