﻿using Bulky.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
