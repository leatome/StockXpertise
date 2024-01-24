﻿using StockXpertise.Models;

namespace StockXpertise.Helpers
{
    public class ForeignKey<T> where T : BaseModel<T>, new()
    {
        public int Id { get; set; }

        public static implicit operator int(ForeignKey<T> foreignKey)
        {
            return foreignKey.Id;
        }

        public static explicit operator ForeignKey<T>(int id)
        {
            return new ForeignKey<T> { Id = id };
        }

        public T ResolveEntity()
        {
            var instance = new T();
            return BaseModel<T>.GetBy("id_" + instance.TableName, Id);
        }

    }
}
