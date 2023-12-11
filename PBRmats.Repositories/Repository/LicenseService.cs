using PBRmats.Core.Entities;
using PBRmats.Persistence.Data.Context;
using PBRmats.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBRmats.Repositories.Repos
{
    public class LicenseService : IListService<License>
    {
        private readonly PBRmatsContext _context;

        public LicenseService(PBRmatsContext context) => 
            _context = context;

        public ICollection<License> GetList() =>
            _context.Licenses.ToList();
    }
}
