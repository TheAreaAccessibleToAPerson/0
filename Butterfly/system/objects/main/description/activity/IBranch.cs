namespace Butterfly.system.objects.main.description.activity
{
    /// <summary>
    /// Описавает споботы указания в родительских обьектах что их Branch обьекты полностью 
    /// прекратили свою работу.
    /// </summary>
    public interface IBranch
    {
        /// <summary>
        /// Описывает метод c помощью которого дочерний обьект укажит что один из Приватных обработчиков 
        /// прекратил свою работу.
        /// </summary>
        public void RemovePrivateHandler();

        /// <summary>
        /// Описывает метод c помощью которого дочерний обьект укажит что один из Публичных обработчиков 
        /// прекратил свою работу.
        /// </summary>
        public void RemovePublicHandler();

        /// <summary>
        /// Описывает метод c помощью которого дочерний обьект укажит что один из BranchMainObject
        /// прекратил свою работу.
        /// </summary>
        public void RemoveBranchMainObject();
    }
}
