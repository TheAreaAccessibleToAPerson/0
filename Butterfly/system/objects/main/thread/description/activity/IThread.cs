namespace Butterfly.system.objects.main.thread.description.activity
{
    /// <summary>
    /// Описываем запуск и остановку thread.
    /// </summary>
    public interface IThread
    {
        void Start();
        void Stop();
        void TaskStop();
    }
}
