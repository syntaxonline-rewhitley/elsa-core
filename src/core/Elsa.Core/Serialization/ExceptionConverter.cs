using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Elsa.Serialization
{
    public class ExceptionConverter : JsonConverter<Exception>
    {
        
        public override void WriteJson(JsonWriter writer, Exception value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }
        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override Exception ReadJson(JsonReader reader, Type objectType, Exception existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            //reader will throw exception if not read to end
            var jObject = JObject.Load(reader);
            var ex = System.Text.Json.JsonSerializer.Deserialize<Exception>(jObject.ToString());
            
            //to maintain backwards compatibility with code prior to commit, will continue to ignore exception
            return default;

        }
    }
}
