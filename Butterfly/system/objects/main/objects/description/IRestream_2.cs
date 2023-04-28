namespace Butterfly.system.objects.main.objects.description
{
    /// <summary>
    /// Описывает способы перенаправления данных.
    /// </summary>
    /// <typeparam name="ParamType1"></typeparam>
    /// <typeparam name="ParamType2"></typeparam>
    public interface IRestream<ParamType1, ParamType2>
    {
        /// <summary>
        /// Перенаправляет/Дублирует данные <typeparamref name="ParamType1"/>, <typeparamref name="ParamType2"/> в <paramref name="pAction"/>
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="pPollSize"></param>
        /// <param name="pTimeDelay"></param>
        /// <returns></returns>
        public IRestream<ParamType1, ParamType2> output_to(global::System.Action<ParamType1, ParamType2> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");
    }
}
