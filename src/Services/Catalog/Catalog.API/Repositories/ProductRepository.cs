using Catalog.API.Data;
using Catalog.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataAppContext _context;

        public ProductRepository(DataAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                            .Products.ToListAsync();


        }
        public async Task<Product> GetProduct(string id)
        {
            return await _context
                           .Products

                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {


            return await _context
                            .Products
                            .Where(x => x.Name == name)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {


            return await _context
                            .Products
                            .Where(x => x.Category == categoryName)
                            .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                                        .Products.FindAsync(product.Id);
            updateResult.Name = product.Name;
            updateResult.Description = product.Description;
            updateResult.Category = product.Category;

            return true;

        }

        public async Task<bool> DeleteProduct(string id)
        {
            var updateResult = await _context
                                        .Products.FindAsync(id);

            _context.Products.Remove(updateResult);
            await _context.SaveChangesAsync();
            return true;




        }

    }
}
