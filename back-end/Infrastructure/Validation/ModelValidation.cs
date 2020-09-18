﻿using Abstractions.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure.Validation
{
    /* Yes, the methods are almost the same and the errors are almost the same.
     * Yes, copy-pasted code all over the place.
     * 
     * But from the business stand point, the entities are different; the use-cases are different.
     * 
     * And if (or when) the validation will change, 
     * it will be more simple and more safe to change behavior for single use-case for a single entity,
     * rather than rewriting big and weird generic method
     */

    static class ModelValidation
    {
        //Introduction
        public static void Check(DataModel.Introduction dbItem, Abstractions.Model.Introduction introduction)
        {
            if (dbItem == null)
            {
                throw new InconsistencyException(Resources.TextMessages.IntroductionIsMissing);
            }


            if (dbItem.Version != introduction.Version)
            {
                throw new InconsistencyException
                (
                    string.Format(Resources.TextMessages.ItemWasAlreadyChanged, introduction.GetType().Name)
                );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = introduction.ExternalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                    continue;

                if (item.ExternalUrl.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, updated.GetType().Name)
                    );
                }
            }

        }
        //



        //Account
        public static void CheckBeforeDelete(DataModel.Account dbItem, Abstractions.Model.Account account)
        {
            if (account.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, account.GetType().Name)
                    );
            }

            if(dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, account.GetType().Name)
                    );
            }

            if(dbItem.Version != account.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, account.GetType().Name)
                    );
            }
        }
        
        public async static void CheckBeforeUpdate(DataModel.Account dbItem, Abstractions.Model.Account account, DataContext context)
        {
            if (account.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantUpdateNewItem, account.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, account.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Login")
                    );
            }

            if (string.IsNullOrEmpty(account.Role))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Role")
                    );
            }


            if (dbItem.Version != account.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, account.GetType().Name)
                    );
            }
        
            if(dbItem.Login != account.Login && await context.Accounts.AnyAsync(x => x.Login == account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Login")
                    );
            }

        }

        public static async void CheckBeforeCreate(Abstractions.Model.Account account, DataContext context)
        {
            if (account.Id != null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantCreateExistingItem, account.GetType().Name)
                    );
            }
           
            if (string.IsNullOrEmpty(account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Login")
                    );
            }

            if (string.IsNullOrEmpty(account.Password))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Password")
                    );
            }

            if (await context.Accounts.AnyAsync(x => x.Login == account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Login")
                    );
            }

        }
        //


        // Category
        public static void CheckBeforeDelete(DataModel.Category dbItem, Abstractions.Model.Category category)
        {
            if (category.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, category.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, category.GetType().Name)
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, category.GetType().Name)
                    );
            }

            if (dbItem.IsEverything)
            {
                throw new InconsistencyException(Resources.TextMessages.CantDeleteSystemCategory);
            }
        }

        public static async void CheckBeforeCreate(Abstractions.Model.Category category, DataContext context)
        {
            if (category.Id != null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantCreateExistingItem, category.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(category.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "DisplayName")
                    );
            }

            if (await context.Categories.AnyAsync(x => x.Code == category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

        }

        public static async void CheckBeforeUpdate(DataModel.Category dbItem, Abstractions.Model.Category category, DataContext context)
        {
            if (category.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantUpdateNewItem, category.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, category.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(category.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "DisplayName")
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, category.GetType().Name)
                    );
            }

            if (dbItem.Code != category.Code && await context.Categories.AnyAsync(x => x.Code == category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

        }
        //


        //Project
        public static void CheckBeforeDelete(DataModel.Project dbItem, Abstractions.Model.Project project)
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

        public static async void CheckBeforeCreate(Abstractions.Model.Project project, DataContext context)
        {
            if (project.Id != null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantCreateExistingItem, project.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(project.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "DisplayName")
                    );
            }

            if (project.Category == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Category")
                    );
            }

            if (!await context.Categories.AnyAsync(x=>x.Id == project.Category.Id))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.TheCategoryDoesNotExist, project.Category.Code)
                    );
            }


            if (await context.Projects.AnyAsync(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

            foreach (var item in project.ExternalUrls)
            {
                if(string.IsNullOrEmpty(item.DisplayName))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Display name of the External URL")
                    );

                if (string.IsNullOrEmpty(item.Url))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "URL of the External URL")
                    );
            }

            foreach (var item in project.GalleryImages)
            {
                if (string.IsNullOrEmpty(item.ImageUrl))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Image of the Gallery Image")
                    );
            }

        }

        public static async void CheckBeforeUpdate(DataModel.Project dbItem, Abstractions.Model.Project project, DataContext context)
        {
            if (project.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantUpdateNewItem, project.GetType().Name)
                    );
            }

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
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(project.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "DisplayName")
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

            if (dbItem.Code != project.Code && await context.Projects.AnyAsync(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = project.ExternalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                    continue;

                if (item.ExternalUrl.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, updated.GetType().Name)
                    );
                }
            }


            foreach (var item in dbItem.GalleryImages)
            {
                var newUrl = project.GalleryImages.FirstOrDefault(x => x.Id == item.Id);
                if (project == null)
                    continue;

                if (dbItem.Version != project.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, item.GetType().Name)
                    );
                }
            }

            foreach (var item in project.ExternalUrls)
            {
                if (string.IsNullOrEmpty(item.DisplayName))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Display name of the External URL")
                    );

                if (string.IsNullOrEmpty(item.Url))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "URL of the External URL")
                    );
            }

            foreach (var item in project.GalleryImages)
            {
                if (string.IsNullOrEmpty(item.ImageUrl))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantbeEmpty, "Image of the Gallery Image")
                    );
            }
        }

        //
    }

}
