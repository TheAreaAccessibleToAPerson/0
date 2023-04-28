namespace Butterfly.system.objects.main.description.definition
{
    /// <summary>
    /// Описывает определение узла и ветки.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Определяем данные для нового узла.
        /// </summary>
        void NodeDefine(Object pParentObject, Object pIndependentObject, SYSTEM.objects.description.access.ISystem pSystemAccess,
            System.Collections.Generic.Dictionary<string, object> pGlobalObjects, string pKeyObject, int pAttachmentNodeNumberInSystem,
            ulong[] pNodeIDParents);

        /// <summary>
        /// Используется для определения инфомации при создании обработчиков.
        /// Вся дополнительная информация и необходимые обьекты будут позже доставлены из обьекта преставляющего
        /// узел от которого выстраиваются ветки.
        /// </summary>
        public void BranchDefine(Object pNodeObject, Object pParentObject, Object pIndependentObject, SYSTEM.objects.description.access.ISystem pSystemAccess,
            System.Collections.Generic.Dictionary<string, object> pGlobalObjects,
            string pKeyObject, ulong pUniqueNodeKey, int pAttackmentNumberObjectInNode, int pAttachmentNodeNumberInSystem,
            ulong[] pNodeIDParents);
    }
}
