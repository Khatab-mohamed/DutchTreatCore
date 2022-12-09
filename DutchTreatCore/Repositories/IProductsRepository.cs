using System.Collections.Generic;
using DutchTreatCore.Data.Entities;

namespace DutchTreatCore.Repositories
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
    }
}