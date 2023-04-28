namespace Butterfly
{
    public class Butterfly
    {
        public static void fly<ObjectType>() where ObjectType : system.objects.main.Object, new()
        {
            system.objects.SYSTEM.objects.description.activity.ISystem systemObject 
                = new system.objects.SYSTEM.system<ObjectType>();

            systemObject.Start();
        }
    }
}
