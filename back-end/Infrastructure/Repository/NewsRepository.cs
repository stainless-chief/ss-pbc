﻿using Abstractions.IRepository;
using Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly DataContext _context;

        public NewsRepository(DataContext context)
        {
            _context = context;
        }

        public Task<News[]> GetNews()
        {
            return _context.News.Select
                (x => new News
                {
                    Content = x.Content,
                    Id = x.Id,
                    PosterUrl = x.PosterUrl,
                    Title = x.Title,
                    Version = x.Version
                }
                ).ToArrayAsync();
        }
    }
}
