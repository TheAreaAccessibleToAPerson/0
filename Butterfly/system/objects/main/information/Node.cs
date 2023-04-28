namespace Butterfly.system.objects.main.information
{
    public class Node
    {
        /// <summary>
        /// Имя ключа от колекции в родительском классе где хранится данный обьект.
        /// </summary>
        public readonly string KeyObject;

        /// <summary>
        /// Уникальный ключ для обьекта.
        /// </summary>
        public readonly ulong ID;

        /// <summary>
        /// Уникальный ключ для всего узла.
        /// </summary>
        public readonly ulong NodeID;

        /// <summary>
        /// Номер вложености данного узла в контексте всей системы.
        /// </summary>
        public readonly int AttachmentNodeNumberInSystem;

        /// <summary>   
        /// Номер вложености обьекта в нутри узла.
        /// </summary>
        public readonly int AttackmentNumberObjectInNode;

        /// <summary>
        /// Массив который хранит все NodeObject.ID всех родительских обьектов.
        /// </summary>
        public readonly ulong[] IDNodeParents;

        /// <summary>
        /// Обьект который является узлом(ControllerObject)
        /// </summary>
        public readonly Object NodeObject; // Обьект Controller который представляет узел.
        /// <summary>
        /// Текущий обьект.
        /// </summary>
        public readonly Object CurrentMainObject; // Текущий MainObject.
        /// <summary>
        /// Родительский обьект.
        /// </summary>
        public readonly Object ParentMainObject; // Родительский MainObject.
        /// <summary>
        /// Ближайший индивидуальный обьект. Если обьект уничтожается, то уничтожение начинается с этого места.
        /// </summary>
        public readonly Object NearNodeIndependentObject; // Ближайший индивидуальный обьект, начиная с этого обьекта уничтожается все что ниже.
        /// <summary>
        /// Доступ к системным возможностям.
        /// </summary>
        public readonly SYSTEM.objects.description.access.ISystem SystemAccess; // Системный MainObject.

        /// <summary>
        /// Конструтор для обьекта Controller(узла).
        /// </summary>
        /// <param name="pCurrentMainObject"></param>
        /// <param name="pParentMainObject"></param>
        /// <param name="pIndependentObject"></param>
        /// <param name="pSystemAccess"></param>
        /// <param name="pKeyObject"></param>
        /// <param name="pAttachmentNodeNumberInSystem"></param>
        public Node(Object pCurrentMainObject, Object pParentMainObject, Object pIndependentObject,
            SYSTEM.objects.description.access.ISystem pSystemAccess, string pKeyObject, int pAttachmentNodeNumberInSystem,
            ulong[] pNodeIDParents)
        {
            NodeObject = pCurrentMainObject;
            CurrentMainObject = pCurrentMainObject;
            NearNodeIndependentObject = pIndependentObject;
            ParentMainObject = pParentMainObject;
            SystemAccess = pSystemAccess;

            KeyObject = pKeyObject;
            AttackmentNumberObjectInNode = 0;
            AttachmentNodeNumberInSystem = pAttachmentNodeNumberInSystem;
            IDNodeParents = pNodeIDParents;

            ID = Node.CreatingUniqueID();

            NodeID = Node.CreatingUniqueNodeID();
        }

        /// <summary>
        /// Конструктор для веток.
        /// </summary>
        /// <param name="pCurrentMainObject"></param>
        /// <param name="pParentMainObject"></param>
        /// <param name="pIndependentObject"></param>
        /// <param name="pSystemAccess"></param>
        /// <param name="pKeyObject"></param>
        /// <param name="pUniqueNodeKey"></param>
        /// <param name="pAttachmentNodeNumberInSystem"></param>
        /// <param name="pAttackmentNumberObjectInNode"></param>
        public Node(Object pNodeObject, Object pCurrentMainObject, Object pParentMainObject, Object pIndependentObject,
            SYSTEM.objects.description.access.ISystem pSystemAccess, string pKeyObject, ulong pUniqueNodeKey, 
            int pAttachmentNodeNumberInSystem, int pAttackmentNumberObjectInNode, ulong[] pNodeIDParents)
        {
            NodeObject = pNodeObject;

            CurrentMainObject = pCurrentMainObject;
            NearNodeIndependentObject = pIndependentObject;
            ParentMainObject = pParentMainObject;
            SystemAccess = pSystemAccess;

            KeyObject = pKeyObject;
            AttackmentNumberObjectInNode = pAttackmentNumberObjectInNode;
            AttachmentNodeNumberInSystem = pAttachmentNodeNumberInSystem;
            IDNodeParents = pNodeIDParents;

            ID = Node.CreatingUniqueID();
            NodeID = pUniqueNodeKey;
        }

        private static ulong IndexUniqueID = 0;
        private static object UniqueKeyLocker = new object();
        private static ulong CreatingUniqueID()
        {
            lock (UniqueKeyLocker)
            {
                return IndexUniqueID++;
            }
        }

        private static ulong IndexUniqueNodeID = 0;
        private static object UniqueNodeLocker = new object();
        private static ulong CreatingUniqueNodeID()
        {
            lock (UniqueNodeLocker)
            {
                return IndexUniqueNodeID++;
            }
        }
    }
}
