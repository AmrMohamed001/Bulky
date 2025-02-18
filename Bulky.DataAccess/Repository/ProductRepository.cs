using Bulky.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private AppDbContext _db;
        public ProductRepository(AppDbContext Db): base(Db)
        {
            _db = Db;
        }

        public void Update(Product entity)
        {
            _db.Products.Update(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
