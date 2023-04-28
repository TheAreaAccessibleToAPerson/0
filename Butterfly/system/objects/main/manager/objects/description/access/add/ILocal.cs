namespace Butterfly.system.objects.main.manager.objects.description.access.add
{
    public interface ILocal
    {
        public LocalObjectType TryAdd<LocalObjectType>(string pObjectName = "") where LocalObjectType : ILocalObject, new();
    }
}
