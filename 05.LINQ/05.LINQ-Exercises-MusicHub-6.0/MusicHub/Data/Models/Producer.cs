﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Producer
    {
        public Producer()
        {
            Albums = new HashSet<Album>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.ProducerNameMaxLength)]
        public string Name { get; set; } = null!;
        public string? Pseudonym  { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}
