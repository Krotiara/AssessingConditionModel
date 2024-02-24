using ASMLib.DynamicAgent;

namespace Agents.API.Entities.AgentsSettings
{
    public class Property : IProperty
    {

        public Property(string name, string type, object value, string description = null)
        {
            Name = name;
            Type = type;
            Value = value;
            Description = description;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public object Value { get; set; }
    }
}

