namespace Butterfly.system.objects.main.local.description.access.set
{
    public interface IValue
    {
        /// <summary>
        /// Пытается задать значение обекту, если значение уже задано вернет false, иначе true;
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public bool TrySet(object pValue);

        /// <summary>
        /// Проверяем пустое ли локальное значение.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty();

        /// <summary>
        /// Получить тип ожидаемого локального значения.
        /// </summary>
        /// <returns></returns>
        public string GetValueType();
    }
}
