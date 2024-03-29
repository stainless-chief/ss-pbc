﻿using Abstractions.Cache;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.RepositoryPrivate;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesPrivate
{
    public class PrivateProjectRepository : IPrivateProjectRepository
    {
        private readonly DataContext _context;
        private readonly IDataCache _cache;

        public PrivateProjectRepository(DataContext context, IDataCache cache)
        {
            _context = context;
            _cache = cache;
        }


        public async Task<bool> DeleteAsync(Project project)
        {
            await _cache.FlushAsync(CachedItemType.Categories);
            await _cache.FlushAsync(CachedItemType.ProjectsPreview);

            var dbItem = await _context.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);

            CheckBeforeDelete(dbItem, project);

            _context.Projects.Remove(dbItem);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Project> SaveAsync(Project project)
        {
            await _cache.FlushAsync(CachedItemType.Categories);
            await _cache.FlushAsync(CachedItemType.ProjectsPreview);

            if (project.Id == null)
            {
                return await CreateAsync(project);
            }

            return await UpdateAsync(project);
        }


        private async Task<Project> CreateAsync(Project project)
        {
            CheckBeforeCreate(project);

            var dbItem = AbstractionsConverter.ToProject(project);
            await _context.Projects.AddAsync(dbItem);
            await _context.SaveChangesAsync();

            var result = await _context.Projects
                                       .AsNoTracking()
                                       .Include(x => x.Category)
                                       .Include(x => x.ExternalUrls)
                                       .ThenInclude(x => x.ExternalUrl)
                                       .FirstAsync(x => x.Id == dbItem.Id);

            return DataConverter.ToProject(result);
        }

        private async Task<Project> UpdateAsync(Project project)
        {
            var dbItem = _context.Projects
                                 .Include(x => x.Category)
                                 .Include(x => x.ExternalUrls)
                                 .ThenInclude(x => x.ExternalUrl)
                                 .FirstOrDefault(x => x.Id == project.Id);

            CheckBeforeUpdate(dbItem, project);

            Merge(dbItem, project);

            await _context.SaveChangesAsync();

            return DataConverter.ToProject(dbItem);
        }


        private void Merge(Models.Project dbProject, Project project)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Code == project.Category!.Code);
            if (category == null)
            {
                throw new InconsistencyException(string.Format(Resources.TextMessages.CategoryDoesNotExist, project.Category!.Code));
            }

            dbProject.Category = category;
            dbProject.CategoryId = category.Id!.Value;
            dbProject.Code = project.Code;
            dbProject.Description = project.Description;
            dbProject.DescriptionShort = project.DescriptionShort;
            dbProject.Name = project.Name;
            dbProject.PosterDescription = project.PosterDescription;
            dbProject.PosterUrl = project.PosterUrl;
            dbProject.ReleaseDate = project.ReleaseDate;
            dbProject.Version++;

            Merge(dbProject, project.ExternalUrls);
        }

        private void Merge(Models.Project dbItem, IEnumerable<ExternalUrl> newExternalUrls)
        {
            var toRemove = dbItem.ExternalUrls.Where(eu => !newExternalUrls.Any(x => eu.ExternalUrlId == x.Id)).ToList();
            foreach (var item in toRemove)
            {
                dbItem.ExternalUrls.Remove(item);

                _context.ExternalUrls.Remove(item.ExternalUrl);
                _context.ProjectExternalUrls.Remove(item);
            }

            var toAdd = newExternalUrls.Where(x => x.Id == null);
            foreach (var item in toAdd)
            {
                var add = AbstractionsConverter.ToProjectExternalUrl(item);

                _context.ExternalUrls.Add(add.ExternalUrl);
                dbItem.ExternalUrls.Add(add);
            }

            var toUpdate = newExternalUrls.Where(x => x.Id.HasValue);
            foreach (var item in toUpdate)
            {
                var upd = dbItem.ExternalUrls.FirstOrDefault(x => x.ExternalUrlId == item.Id!.Value);

                upd!.ExternalUrl!.DisplayName = item.DisplayName;
                upd.ExternalUrl.Url = item.Url;
                upd.ExternalUrl.Version++;
            }
        }

        private static void CheckBeforeDelete(Models.Project dbItem, Project project)
        {
            if (project.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, project.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, project.GetType().Name)
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

        }

        private void CheckBeforeCreate(Project project)
        {
            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Code))
                    );
            }

            if (string.IsNullOrEmpty(project.Description))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Description))
                    );
            }

            if (string.IsNullOrEmpty(project.DescriptionShort))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.DescriptionShort))
                    );
            }

            if (project.Category == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Category))
                    );
            }

            if (!_context.Categories.Any(x => x.Id == project.Category.Id))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CategoryDoesNotExist, project.Category.Code)
                    );
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Name))
                    );
            }

            if (_context.Projects.Any(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, nameof(project.Code))
                    );
            }

            foreach (var item in project.ExternalUrls ?? new List<ExternalUrl>())
            {
                if (string.IsNullOrEmpty(item.DisplayName))
                {
                    throw new InconsistencyException
                    (
                       string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, $"{nameof(item.DisplayName)} of {nameof(ExternalUrl)}")
                    );
                }

                if (string.IsNullOrEmpty(item.Url))
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, $"{nameof(item.Url)} of {nameof(ExternalUrl)}")
                    );
                }
            }
        }

        private void CheckBeforeUpdate(Models.Project dbItem, Project project)
        {
            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, project.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Code))
                    );
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, nameof(project.Name))
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

            if (dbItem.Code != project.Code && _context.Projects.Any(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, nameof(project.Code))
                    );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = project.ExternalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                {
                    continue;
                }

                if (item!.ExternalUrl!.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, nameof(updated))
                    );
                }
            }

            foreach (var item in project.ExternalUrls ?? new List<ExternalUrl>())
            {
                if (string.IsNullOrEmpty(item.DisplayName))
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, $"{nameof(item.DisplayName)} of {nameof(ExternalUrl)}")
                    );
                }

                if (string.IsNullOrEmpty(item.Url))
                {
                    throw new InconsistencyException
                    (
                       string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, $"{nameof(item.Url)} of {nameof(ExternalUrl)}")
                    );
                }
            }
        }
    }
}
