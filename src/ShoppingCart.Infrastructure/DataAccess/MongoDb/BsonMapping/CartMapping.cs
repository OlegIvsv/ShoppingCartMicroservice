﻿using MongoDB.Bson.Serialization;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping
{
    internal class CartMapping
    {
        public void RegisterMap()
        {
            BsonClassMap.RegisterClassMap<Cart>(map =>
            {
                map.MapField("_items").SetElementName("items");
            });
        }
    }
}