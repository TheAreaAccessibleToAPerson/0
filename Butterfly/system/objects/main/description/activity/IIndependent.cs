namespace Butterfly.system.objects.main.description.activity
{
    /// <summary>
    /// Описывает активность Independent обьекта.
    /// </summary>
    public interface IIndependent
    {
        /// <summary>
        /// Начинаем процедуру остановки Independent обьекта.
        /// </summary>
        public void NodeObjectStop();

        /// <summary>
        /// После вызова IIndependent.IndependentObjectStop() во всех Node обьектах будет вызывается
        /// данный метод, до того момента пока не будут найдены и поставлены все крайние Node обьекты
        /// в жизненое состояние остановки.
        /// </summary>
        public void BackgroundObjectStop();
    }
}
