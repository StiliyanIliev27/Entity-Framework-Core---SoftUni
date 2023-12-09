﻿using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models
{
    public class Boardgame
    {
        public Boardgame()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.BoardgameNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.BoardgameRatingMinLength, ValidationConstants.BoardgameRatingMaxLength)]
        public double Rating { get; set; }

        [Required]
        [MaxLength(ValidationConstants.BoardgameMaxYearPublished)]
        public int YearPublished { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(CreatorId))]
        public int CreatorId { get; set; }
        public virtual Creator Creator { get; set; }
        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}