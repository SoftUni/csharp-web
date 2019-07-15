using System.ComponentModel.DataAnnotations;

namespace Stopify.Data.Models
{
    public class BaseModel<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
