using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visma.TimeTracking.EventSourcing.Models
{
    public class Stream
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ConcurrencyCheck]
        public long Version { get; set; }
    }
}