﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models
{
    [Table("introduction")]
    [ExcludeFromCodeCoverage]
    internal class Introduction
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("poster_description")]
        public string? PosterDescription { get; set; }

        [Column("poster_url")]
        public string? PosterUrl { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("version")]
        public long Version { get; set; }

        public ICollection<IntroductionToExternalUrl> ExternalUrls { get; set; } = new List<IntroductionToExternalUrl>();
    }
}
