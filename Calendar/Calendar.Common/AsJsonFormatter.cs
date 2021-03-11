using Newtonsoft.Json;

namespace Calendar.Common
{
    public sealed class AsJsonFormatter
    {
        private readonly object _value;

        public AsJsonFormatter(object value)
        {
            _value = value;
        }

        public static AsJsonFormatter Create(object value) //Abstract class? (think, no need for now)
        {
            return new AsJsonFormatter(value);
        }

        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(_value);
            }
            catch
            {
                return "!!!ERRROR SERIALIZING TO LOGGER!!!!";
            }
        }
    }
}