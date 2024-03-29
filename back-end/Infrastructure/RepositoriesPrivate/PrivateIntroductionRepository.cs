﻿using Abstractions.Cache;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.RepositoryPrivate;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesPrivate
{
    public class PrivateIntroductionRepository : IPrivateIntroductionRepository
    {
        private readonly DataContext _context;
        private readonly IDataCache _cache;

        public PrivateIntroductionRepository(DataContext context, IDataCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Introduction> SaveAsync(Introduction item)
        {
            var dbItem = await _context.Introductions
                                       .Include(x => x.ExternalUrls)
                                       .ThenInclude(x => x.ExternalUrl)
                                       .OrderBy(x => x.Id)
                                       .FirstOrDefaultAsync();

            CheckBeforeUpdate(dbItem, item);

            Merge(dbItem, item);
            await _context.SaveChangesAsync();

            var resultItem = DataConverter.ToIntroduction(dbItem);
            await _cache.SaveAsync(resultItem);

            return resultItem;
        }


        private void Merge(Models.Introduction dbItem, Introduction newItem)
        {
            dbItem.Title = newItem.Title;
            dbItem.Content = newItem.Content;
            dbItem.PosterDescription = newItem.PosterDescription;
            dbItem.PosterUrl = newItem.PosterUrl;
            dbItem.Version++;


            Merge(dbItem, newItem.ExternalUrls);
        }

        private void Merge(Models.Introduction dbItem, IEnumerable<ExternalUrl> newExternalUrls)
        {
            var toRemove = dbItem.ExternalUrls.Where(eu => !newExternalUrls.Any(x => eu.ExternalUrlId == x.Id)).ToList();
            foreach (var item in toRemove)
            {
                dbItem.ExternalUrls.Remove(item);

                _context.ExternalUrls.Remove(item.ExternalUrl);
                _context.IntroductionExternalUrls.Remove(item);
            }

            var toAdd = newExternalUrls.Where(x => x.Id == null);
            foreach (var item in toAdd)
            {
                var add = AbstractionsConverter.ToIntroductionExternalUrl(item);

                _context.ExternalUrls.Add(add.ExternalUrl);
                dbItem.ExternalUrls.Add(add);
            }

            var toUpdate = newExternalUrls.Where(x => x.Id.HasValue);
            foreach (var item in toUpdate)
            {
                var upd = dbItem.ExternalUrls.First(x => x.ExternalUrlId == item!.Id!.Value);

                upd!.ExternalUrl!.DisplayName = item.DisplayName;
                upd.ExternalUrl.Url = item.Url;
                upd.ExternalUrl.Version++;
            }

        }


        private static void CheckBeforeUpdate(Models.Introduction dbItem, Introduction introduction)
        {
            if (dbItem.Version != introduction.Version)
            {
                throw new InconsistencyException
                (
                    string.Format(Resources.TextMessages.ItemWasAlreadyChanged, introduction.GetType().Name)
                );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = introduction.ExternalUrls?.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                {
                    continue;
                }

                if (item!.ExternalUrl!.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, updated.GetType().Name)
                    );
                }
            }
        }
    }
}
