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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private AppDbContext _db;
        public CompanyRepository(AppDbContext dbContext):base(dbContext)
        {
            _db = dbContext;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Company company)
        {
            _db.Update(company);
        }
    }
}
