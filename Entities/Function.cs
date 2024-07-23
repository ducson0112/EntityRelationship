using System.Text.Json.Serialization;

namespace EntityRelationship.Entities
{
    public class Function
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Role> Roles { get; set; }
    }
}
