namespace Butterfly.system.objects.main.activity.description
{
    /// <summary>
    /// Описывает управление жизнеными процессами обьекта.
    /// </summary>
    public interface ILife
    {
        void Construction();
        void Dependency();

        void Start();
        void ContinueStart();
        
        /// <summary>
        /// Описывает метод запускающий метод Stop() в LifeActivity в котором вызовется абстрактный метод Stop() и останавливает потоки.
        /// Если это Node обьект(через который Branch обьекты подписываются в пуллы, и на глобальные обьекты),
        /// то он запустит процедуру отписки после чего вызовет метод ContrinueStop().
        /// Если это Branch обьект, то просто вызовем ему метод ContinueStop(). 
        /// </summary>
        void Stop();
        /// <summary>
        /// 
        /// </summary>
        void ContinueStop();
    }
}
