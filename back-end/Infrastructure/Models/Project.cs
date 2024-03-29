﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models
{
    [Table("project")]
    [ExcludeFromCodeCoverage]
    internal class Project
    {
        [Key]
        [Column("id")]
        public Guid? Id { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("description_short")]
        public string DescriptionShort { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("poster_description")]
        public string? PosterDescription { get; set; }

        [Column("poster_url")]
        public string? PosterUrl { get; set; }

        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [Column("version")]
        public long Version { get; set; }

        public Category? Category { get; set; }

        public ICollection<ProjectToExternalUrl> ExternalUrls { get; set; } = new List<ProjectToExternalUrl>();
    }
}
