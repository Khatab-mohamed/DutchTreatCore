using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreatCore.Data;
using DutchTreatCore.Data.Entities;

namespace DutchTreatCore.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DutchContext _context;

        public ProductsRepository(DutchContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(p => p.Title).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            return _context.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public bool SaveAll()
        {
          return  _context.SaveChanges() > 0;
        }
    }
}