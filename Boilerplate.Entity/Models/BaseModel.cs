using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Boilerplate.Entity.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
    }
}
