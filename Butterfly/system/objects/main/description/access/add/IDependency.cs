namespace Butterfly.system.objects.main.description.access.add
{
    /// <summary>
    /// Вызывать в стандартном контрукторе.
    /// Описывает метод добавления зависимостей. В конструкторе в конце цепочки результат может быть перенаправлен в
    /// обьект которые еще не был создан. Поэтому мы лишь записываем данный метод в System.Action. И после того как Constructio()
    /// завершил своб работу и все обьекты были созданы запустятся зависимости. 
    /// </summary>
    public interface IDependency
    {
        /// <summary>
        /// Добовляем зависимости в стандартном методе Construction(), которые настроятся после окончания системного метода Construction().
        /// </summary>
        /// <param name="pFunc"></param>
        void Add(global::System.Action pAction);
    }
}
