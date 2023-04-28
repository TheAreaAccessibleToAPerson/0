namespace Butterfly.system.objects.main.description.activity
{
    
    /// <summary>
    /// Описывает способы создания и уничтожения узла.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Запускает создания узла.
        /// </summary>
        void Create();

        /// <summary>
        /// Уничтожение узла. Уничтожение узла происходит из ближайшего Independent узла обьекта.Сам узел может является Independenct обьектом.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Описывает метод который вызвавит жизненый цикл в Stopping для NodeОбьекта.
        /// Так как уничтожение узла начинается с Independent обьекта, то текущий узел может не является таким.
        /// </summary>
        void SetStateStopping();
    }
}
