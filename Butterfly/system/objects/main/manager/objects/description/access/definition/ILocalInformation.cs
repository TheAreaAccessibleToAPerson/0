namespace Butterfly.system.objects.main.manager.objects.description.access.definition
{
    /// <summary>
    /// Взаимосвязь с информационным пространсвом в котором обьект используется прямо сейчас.
    /// </summary>
    public interface ILocalInformation
    {
        void Define(IInforming pInformingMainObject);
    }
}
