using System.Text.Json;

namespace ClientLibrary.Helpers
{
    public static class Serializations
    {
        /// <summary>
        /// Converts a generic object to a JSON string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modeloObject"></param>
        /// <returns></returns>
        public static string SerializeObj<T>(T modeloObject) => JsonSerializer.Serialize(modeloObject);

        /// <summary>
        /// Converts a JSON string to a generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString)!;
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString)!;
    }
}
