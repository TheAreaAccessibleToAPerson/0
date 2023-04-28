namespace Butterfly.system.objects.handler.description
{
    /// <summary>
    /// Зарегестрировать работу приема данных в пуллы.
    /// </summary>
    public interface IRegisterInPoll
    {
        public void RegisterInPoll(int pSizePoll, int pTimeDelay, string pName);
    }
}
