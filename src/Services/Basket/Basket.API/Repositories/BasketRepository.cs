﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Repositories {
  internal class BasketRepository : IBasketRepository {
    private readonly IDistributedCache redisCache;

    public BasketRepository(IDistributedCache redisCache) {
      this.redisCache = redisCache;
    }

    public async Task<ShoppingCart> GetBasket(string userName) {
      var basket = await redisCache.GetStringAsync(userName);

      return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket) {
      await redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

      return await GetBasket(basket.UserName);
    }

    public async Task DeleteBasket(string userName) {
      await redisCache.RemoveAsync(userName);
    }
  }
}
