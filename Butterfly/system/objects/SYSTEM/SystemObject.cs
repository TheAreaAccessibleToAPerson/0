namespace Butterfly.system.objects.SYSTEM
{
    public sealed class system<ObjectType> : objects.main.SystemObject<ObjectType> where ObjectType: main.Object, new()
    {
        void Start() => deferred_create_object<ObjectType>("");
    }  
}
