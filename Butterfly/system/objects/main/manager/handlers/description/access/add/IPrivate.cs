namespace Butterfly.system.objects.main.manager.handlers.description.access.add
{
    /// <summary>
    /// Реализуем методы для создания приватных обработчиков.
    /// </summary>
    public interface IPrivate
    {
        /// <summary>
        /// Добавить приватный обработчик, после этого вы несможете получить к нему прямой доступ.
        /// </summary>
        /// <typeparam name="PrivateHandlerType"></typeparam>
        /// <returns></returns>
        PrivateHandlerType Add<PrivateHandlerType>(global::System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent)
            where PrivateHandlerType : Object, handler.description.IRestream, IInput, handler.description.IContinueInterrupting, new();

        PrivateHandlerType Add<PrivateHandlerType>()
            where PrivateHandlerType : Object, handler.description.IRestream, IInput, handler.description.IContinueInterrupting, new();

        /// <summary>
        /// Добавить приватный обработчик, после этого вы несможете получить к нему прямой доступ.
        /// </summary>
        /// <typeparam name="PrivateHandlerType"></typeparam>
        /// <returns></returns>
        PrivateHandlerType Add<PrivateHandlerType>(PrivateHandlerType pPrivateHandler)
            where PrivateHandlerType : Object, IInput, new();
    }
}
