namespace Butterfly
{
    /// <summary>
    /// Описывает способ доставки входных данных.
    /// </summary>
    public interface IInput<InputType>
    {
        /// <summary>
        /// Принимает входные данные <typeparamref name="InputType"/>.
        /// </summary>
        void ToInput(InputType pValue);
    }
    /// <summary>
    /// Описывает способ доставки входных данных.
    /// </summary>
    public interface IInput<ParamType1, ParamType2>
    {
        /// <summary>
        /// Принимает входные данные <typeparamref name="ParamType1"/>, <typeparamref name="ParamType2"/> .
        /// </summary>
        /// <param name="pValue1"></param>
        /// <param name="pValue2"></param>
        void ToInput(ParamType1 pValue1, ParamType2 pValue2);
    }
    /// <summary>
    /// Описывает способ доставки входных данных.
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Принимает входные данные без явного указания типа.
        /// </summary>
        void ToInput<ParamValueType>(ParamValueType pValue);
    }
}
