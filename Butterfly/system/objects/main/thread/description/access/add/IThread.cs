namespace Butterfly.system.objects.main.thread.description.access.add
{
    /// <summary>
    /// Описывает методы с помощью которых можно добавить новый thread.
    /// </summary>
    public interface IThread
    {
        void Add(global::System.Action action, string pName, int pThreadTimeDelay, int pCountThread = 1);
    }
}
