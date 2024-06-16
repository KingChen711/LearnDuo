using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorDemand.Data.Dtos.Slot
{
    public class SlotDto
    {
        public int Id { get; set; }

        public Guid SlotId { get; set; }

        public string SlotName { get; set; } = null!;

        public float Duration { get; set; }

        public TimeOnly Time { get; set; }
        
        public string? SlotDesc { get; set; }
    }
}
