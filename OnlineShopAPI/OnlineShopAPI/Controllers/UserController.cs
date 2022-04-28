﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using OnlineShopAPI.Services.Interfaces;

namespace OnlineShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;

        public UserController(IUnitOfWork uow, IPhotoService photoService)
        {
            _uow = uow;
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> AddPhotoToUser(int id, IFormFile photo)
        {
            //HttpContext.Request.Headers.TryGetValue("Authorization", out var token);

            //token.ToString();
            return await _photoService.AddPhotoToUser(id, photo) ? Ok(new AppUser()) : BadRequest("Unable to upload the photo");
        }
    }
}
