﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorDemand.Data.Dtos.Subject
{
    public class SubjectDto
    {
        public int Id { get; set; }

        public Guid SubjectId { get; set; }

        public string SubjectCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Image { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;


        //public ICollection<TeachingScheduleDto> TeachingSchedules { get; set; } = [];
    }
}