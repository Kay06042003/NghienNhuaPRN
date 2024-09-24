using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nghien_Nhua.Models
{
    public class VisitorsDataAccess
    {
        private readonly db_Nghien_NhuaContext _db;

        public VisitorsDataAccess()
        {
            _db = new db_Nghien_NhuaContext();
        }
        public VisitorsDataAccess(db_Nghien_NhuaContext db)
        {
            _db = db;
        }

        public async Task UpdateVisitorsCountInView(string date, int count)
        {
            var visitor = _db.Views.FirstOrDefault(v => v.Date == date);
            if (visitor == null)
            {
                visitor = new View
                {
                    Date = date,
                    Count = count
                };
                _db.Views.Add(visitor);
            }
            else
            {
                visitor.Count += count;
            }
            await _db.SaveChangesAsync();
        }

    }
}