namespace Butterfly.system.objects.main.information
{
    public class Header : Informing
    {
        public Header(IInforming pInforming) 
            : base("HeaderInformation", pInforming) { }

        public struct Data
        {
            public const string INDEPENDENT = "Independent";

            public const string System = "System";
            public const string CONTROLLER = "Controller";
            public const string HANDLER = "Handler";
            public const string LOCAL_OBJECT = "LocalObject";

            public const string Node = "Node";
            public const string Branch = "Branch";

            public const string PublicHandler = "PublicHandler";
            public const string PrivateHandler = "PrivateHandler";
        }

        private bool Independent;

        public global::System.Type SystemType { private set; get; }

        public string Type { private set; get; }

        /// <summary>
        /// Имя обьекта.
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Имя для директории.
        /// </summary>
        public string NameTheDirectory { private set; get; }

        /// <summary>
        /// Хранит абривиатуры обьекта используемого в системе и место положение обьекта в системе.
        /// Используется для вывода сообщения в консоль.
        /// </summary>
        public string ExplorerFullInformation { private set; get; }

        /// <summary>
        /// Хранит место положение обьекта относительно все системы.
        /// </summary>
        public string Explorer { private set; get; }

        /// <summary>
        /// Type.FullName;
        /// </summary>
        public string FullName { private set; get; }

        public bool IsSystemController() => Data.System == Type;
        public bool IsHandler() => Data.HANDLER == Type;
        public bool IsPublicHandler() => HandlerType == Data.PublicHandler;
        public bool IsPrivateHandler() => HandlerType == Data.PrivateHandler;
        public bool IsController() => Data.CONTROLLER == Type;

        public bool IsNodeObject() => TypeObject == Data.Node;

        public bool IsBranchObject() => TypeObject == Data.Branch;

        public bool IsIndependent() => Independent == true;

        private string TypeObject;

        private string HandlerType = "";

        public void NodeDefine(string pDirectory, global::System.Type pType, information.Node pParentNodeInformation, string pKeyObject)
        {
            TypeObject = Data.Node;

            Define(pDirectory, pType, pParentNodeInformation, pKeyObject);
        }

        public void BranchDefine(string pDirectory, global::System.Type pType, information.Node pParentNodeInformation, string pKeyObject)
        {
            TypeObject = Data.Branch;

            Define(pDirectory, pType, pParentNodeInformation, pKeyObject);

            if (IsIndependent())
                Exception(Ex.HeaderManager.x10001, Name);
        }

        private void Define(string pDirectory, global::System.Type pType, information.Node pParentNodeInformation, string pKeyObject)
        {
            SystemType = pType;

            Name = pType.Name;
            FullName = pType.FullName;

            string p = pType.FullName.Remove(0, pType.Namespace.Length + 1);

            // Получаем все типы дочерних классов, а самым первым записываем наш текущий.
            var allClass = Hellper.ExpendArray(System.Reflection.Assembly.GetAssembly(typeof(main.Object)).GetTypes(), pType);

            global::System.Type[] types = new global::System.Type[] { pType };
            while (true)
            {
                global::System.Type lastBaseType = types[types.Length - 1].BaseType;
                if (lastBaseType == null)
                {
                    break;
                }
                else
                    types = Hellper.ExpendArray(types, lastBaseType);
            }
            
            for (int i = types.Length - 1; i > 0; i--)
            {
                string baseType = types[i].FullName.Remove(0, types[i].Namespace.Length + 1);

                if (baseType.Contains("[")) baseType = baseType.Remove(baseType.IndexOf("["));

                if (baseType.Contains(Data.System))
                {
                    Type = Data.System;
                    Independent = true;
                    pDirectory = "";
                }
                else if (baseType.Contains(Data.HANDLER))
                {
                    Type = Data.HANDLER;

                    if (pKeyObject == manager.handlers.Private.KEY)
                        HandlerType = Data.PrivateHandler;
                    else
                        HandlerType = Data.PublicHandler;
                }
                else if (baseType.Contains(Data.CONTROLLER))
                {
                    Type = Data.CONTROLLER;

                    if (baseType.Contains(Data.INDEPENDENT))
                    {
                        Independent = true;
                    }
                }
                else if (baseType.Contains(Data.LOCAL_OBJECT))
                {
                    Type = Data.LOCAL_OBJECT;
                }
            }

            // Получаем информацию обовсех джинериках.
            global::System.Type[] genericTypeArray = pType.GetGenericArguments();
            // Создаем красивое имя для директории.
            {
                if (genericTypeArray.Length > 0)
                {
                    p = p.Remove(p.IndexOf("["));
                    //System.Console.WriteLine(p);

                    // Если класс является вложеным.
                    if (p.Contains("+")) // Element`1+Element2`2+Element3`1
                    {
                        // Обьекты.
                        string[] u = p.Split("+"); // new string[] = { Element`1, Element2`2, Element3`1 };
                        int i = 0; // Индекс для движения по обьектам.
                        int f = 0; // Индекс для движения по джинерикам.
                        while (i < u.Length)
                        {
                            // Проверяем есть ли у обьекта джинерики.
                            if (u[i].Contains("`"))
                            {
                                // Получаем имя обьекта и количесво джинериков.
                                string[] r = u[i].Split("`"); // new string[] = { Element, 1 };
                                int countGeneric = global::System.Convert.ToInt32(r[1]);
                                NameTheDirectory += r[0]; // Element

                                NameTheDirectory += "<";
                                for (int m = 0; m < countGeneric; m++)
                                {
                                    string genericName = genericTypeArray[f].ToString().Remove(0, genericTypeArray[f++].Namespace.Length + 1);
                                    NameTheDirectory += $"{genericName}";

                                    if ((m + 1) < countGeneric) NameTheDirectory += ", ";
                                }
                                NameTheDirectory += ">";
                            }
                            else
                            {
                                NameTheDirectory += u[i];
                            }

                            if ((i + 1) < u.Length) NameTheDirectory += ".";

                            i++;
                        }
                    }
                    else
                    {
                        //System.Console.WriteLine(p);
                        string[] r = p.Split("`"); // new string[] = { Element, 1 };

                        NameTheDirectory += r[0];

                        NameTheDirectory += "<";
                        for (int i = 0; i < genericTypeArray.Length; i++)
                        {
                            NameTheDirectory += genericTypeArray[i].ToString().Remove(0, genericTypeArray[i].Namespace.Length + 1);

                            if ((i + 1) < genericTypeArray.Length) NameTheDirectory += ", ";
                        }
                        NameTheDirectory += ">";
                    }
                }
                else
                {
                    if (p.Contains("+"))
                    {
                        string[] u = p.Split("+");

                        for (int i = 0; i < u.Length; i++)
                        {
                            NameTheDirectory += u[i];

                            if ((i + 1) < u.Length) NameTheDirectory += ".";
                        }
                    }
                    else
                    {
                        NameTheDirectory = p;
                    }
                }
            }

            string info = "";
            if (IsIndependent() && IsSystemController())
                info = "[I][S]";
            else if (IsIndependent() && IsController())
                info = "[I][C]";
            else if (IsBranchObject() && IsHandler())
            {
                if (IsPrivateHandler())
                    info = "[R][H]";
                else if (IsPublicHandler())
                    info = "[U][H]";
            }
            else if (IsBranchObject() && IsController())
                info = "[B][C]";
            else if (IsNodeObject())
                info = "[N][C]";
            else
                info = "";

            {
                string count = "";
                string[] u = pDirectory.Split("/");
                for (int i = 0; i < u.Length; i++) count += " ";

                if (Data.System == Type)
                {
                    Explorer = NameTheDirectory;
                    ExplorerFullInformation = info + NameTheDirectory;
                }
                else
                {
                    // В системном обьекте нам не нежуно выводить ID от родителя, так как его нету.
                    if (IsSystemController())
                    {
                        Explorer = count + pDirectory + "/" + NameTheDirectory;
                        ExplorerFullInformation = info + pDirectory + "/" + NameTheDirectory;
                    }
                    else
                    {
                        Explorer = $"{count}{pDirectory}[{pParentNodeInformation.ID}]/{NameTheDirectory}";
                        ExplorerFullInformation = $"{info}{count}{pDirectory}[{pParentNodeInformation.ID}]/{NameTheDirectory}";
                    }
                }

            }
        }
    }
}
