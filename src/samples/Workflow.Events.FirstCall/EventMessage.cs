using GreenPipes.Internals.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Workflow.Events.FirstCall
{
    public class EventMessage
    {
        public string TypeName { get; set; }
        public string Message { get; set; }

        public static EventMessage CreateMessage<TEventType> (TEventType evt)
        {
            var message = JsonConvert.SerializeObject(evt);

            return new EventMessage()
            {
                TypeName = evt.GetType().AssemblyQualifiedName,
                Message = message
            };
        }
    }
}
