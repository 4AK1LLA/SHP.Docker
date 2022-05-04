﻿using AutoMapper;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Constants;
using OnlineShopAPI.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopAPI.Controllers
{
    [Authorize] //(Roles = Roles.Buyer)
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LikeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetLikedProductsByUser()
        {
            var products = await _uow?.UserRepository.GetProductsLikedByUser(this.GetUserId());

            if (products is null || !products.Any())
            {
                return BadRequest("There are not liked products");
            }

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult<ProductDto>> LikeProduct(int productId)
        {
            var product = await _uow?.ProductRepository.LikeProductByUser(this.GetUserId(), productId);

            return await _uow?.ConfirmAsync() 
                ? Ok(_mapper.Map<ProductDto>(product))
                : BadRequest("Unable to like the product");
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<ProductDto>> UnlikeProduct(int productId)
        {
            var product = await _uow?.ProductRepository.UnlikeProductByUser(this.GetUserId(), productId);

            return await _uow?.ConfirmAsync()
                ? Ok(_mapper.Map<ProductDto>(product))
                : BadRequest("Unable to unlike the product");
        }
    }
}
