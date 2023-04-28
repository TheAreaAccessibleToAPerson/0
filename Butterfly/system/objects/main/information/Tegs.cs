namespace Butterfly.system.objects.main.information
{
    public class Tegs : Informing, description.access.add.ITegs
    {
        private readonly information.State StateInformation;

        private string[] Values = null;

        public Tegs(IInforming pInforming, information.State pStateInformation) : base("TegsInformation", pInforming) 
        {
            StateInformation = pStateInformation;
        }

        void description.access.add.ITegs.AddTegs(params string[] pTegs)
        {
            if (StateInformation.IsCreating || StateInformation.IsOccurrence)
            {
                if (Values == null) Values = new string[0];

                for (int i = 0; i < Values.Length; i++)
                {
                    for (int u = 0; u < pTegs.Length; u++)
                        if (Values[i] == pTegs[u])
                            Exception(Ex.Tegs.x10001, Values[i]);
                }

                Values = Hellper.ConcatArray(Values, pTegs);
            }
            else
                Exception(Ex.Tegs.x10002);
        }

        public bool IsTeg(string pTeg)
        {
            if (Values == null) return false;

            foreach (string teg in Values)
                if (teg == pTeg) return true;

            return false;
        }
    }
}
