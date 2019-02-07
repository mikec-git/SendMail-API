using Newtonsoft.Json;

namespace SendMail.Services.HelperClasses
{
    public static class EmailSenderHelper
    {
        public static T DeepCopy<T>(T objectToClone)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objectToClone));
        }
    }
}
