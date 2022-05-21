﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OnlineShopAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICloudinaryService _cloudinaryService;

        public PhotoService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
        }

        public async Task<bool> AddPhotoToProduct(AppUser user, int productId, IFormFile photo)
        {
            if (photo == null)
            {
                return false;
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), photo.OpenReadStream()),
            };

            var uploadResult = await _cloudinaryService.UploadAsync(uploadParams);

            var linkToPhoto = uploadResult.Url;

            var product = user.Products.Where(p => p.Id == productId).FirstOrDefault();

            if (product == null)
            {
                return false;
            }

            var photoEntity = new Photo
            {
                URL = linkToPhoto.AbsoluteUri
            };

            if (!product.Photos.Any())
            {
                photoEntity.IsMain = true;
            }

            product.Photos.Add(photoEntity);

            await _uow.UserRepository.UpdateAsync(user);
            await _uow.ConfirmAsync();

            return await Task.FromResult(true);
        }

        public async Task<bool> AddPhotoToUser(int userId, IFormFile photo)
        {
            if (photo == null)
            {
                return false;
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), photo.OpenReadStream()),
            };

            var uploadResult = await _cloudinaryService.UploadAsync(uploadParams);

            var linkToPhoto = uploadResult.Url;

            var user = await _uow.UserRepository.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.PhotoUrl = linkToPhoto.AbsoluteUri;

            await _uow.UserRepository.UpdateAsync(user);

            await _uow.ConfirmAsync();

            return await Task.FromResult(true);
        }
    }
}
