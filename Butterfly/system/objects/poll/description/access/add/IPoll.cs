namespace Butterfly.system.objects.poll.description.access.add
{
    /// <summary>
    /// Описывает способ добавления в пулы.
    /// </summary>
    public interface IPoll
    {
        public void Add(global::System.Action pAction, int pSize, int pTimeDelay, string pName);
    }
}
