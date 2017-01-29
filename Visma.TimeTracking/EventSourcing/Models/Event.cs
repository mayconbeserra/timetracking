using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visma.TimeTracking.EventSourcing.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        public string StreamId { get; set; }

        [Required]
        public string CommitId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Payload { get; set; }

        public long Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatorId { get; set; }
    }
}