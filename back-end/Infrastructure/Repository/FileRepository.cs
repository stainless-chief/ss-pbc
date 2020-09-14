﻿using Abstractions.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;


        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Save(IFormFile file)
        {
            if (file == null || file.Length == 0) //TODO: max file size
                throw new Exception("Empty file"); //TODO: custom exception

            var folderName = _configuration.GetSection("Location:FileStorage").Get<string>();
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var filename = GenerateFileName() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(pathToSave, filename);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filename;
        }


        private static string GenerateFileName()
        {
            //TODO: re-do
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        }
    }
}