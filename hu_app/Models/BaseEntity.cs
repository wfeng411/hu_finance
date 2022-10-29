using System.Text.Json.Serialization;

namespace hu_app.Models
{
    public class BaseEntity : BaseModel
    {
        [JsonIgnore]
        public bool IsDeleted { get; set; }
    }
}
