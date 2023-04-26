using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.DAL.Entities;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.DataProviders
{
    public class DPAdherent : IDMAdherentRepo
    {
        private ColomiersVolleyContext _db { get; set; }

        public DPAdherent(ColomiersVolleyContext db)
        {
            _db = db;
        }

        private IQueryable<Adherent> GetAll()
        {
            return _db.Adherents
                .Include(a => a.Section)
                .Include(a => a.Category);
        }

        public async Task<IEnumerable<DtoAdherent>> GetAdherents()
        {
            return (await GetAll().ToListAsync()).ToDtoAdherent();
        }

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return (await GetAll().Where(a => a.LastName.ToLower() == name.ToLower() && a.PostalCode == cp).ToListAsync()).ToDtoAdherent();
        }

        public async Task<PagedList<DtoAdherent>> GetPagedAdherents(AdherentFilter? filter, Sorting? sorting, Pagination? pagination)
        {
            var adherents = GetFilteredAdherents(filter);
            
            if (sorting != null)
            {
                adherents = SortData(adherents, sorting);
            }

            IQueryable<Adherent> paginated = pagination != null ? pagination.Paginate(adherents) : adherents;
            return new PagedList<DtoAdherent>(paginated.ToDtoAdherent().ToList(), adherents.Count());
        }

        private IQueryable<Adherent> GetFilteredAdherents(AdherentFilter? filter)
        {

            var adherents = GetAll();

            if (filter != null)
            {
                if (filter.DateRange != null && (filter.DateRange.Start.HasValue || filter.DateRange.End.HasValue))
                {
                    adherents = FilterDate(adherents, filter.DateRange);
                }

                adherents = adherents.Where(ApplyFilters(filter));
                if (filter.DynamicFilter != null)
                {
                    var predicate = ExpressionBuilder.GetExpression<Adherent>(new List<DynamicFilter> { filter.DynamicFilter });
                    if (predicate != null)
                    {
                        adherents = adherents.Where(predicate);
                    }
                }
            }

            return adherents;
        }

        private IQueryable<Adherent> SortData(IQueryable<Adherent> list, Sorting sorting)
        {
            if (!String.IsNullOrWhiteSpace(sorting.Field))
            {
                list = sorting.Field switch
                {
                    "IdAdherent" => sorting.ApplyExpressions(list, x => x.IdAdherent),
                    "BirthdayDate" => sorting.ApplyExpressions(list, x => x.BirthdayDate),
                    "InscriptionDate" => sorting.ApplyExpressions(list, x => x.InscriptionDate),
                    "CertificateDate" => sorting.ApplyExpressions(list, x => x.CertificateDate),
                    "HealthStatementDate" => sorting.ApplyExpressions(list, x => x.HealthStatementDate),
                    "FirstName" => sorting.ApplyExpressions(list, x => !string.IsNullOrEmpty(x.FirstName) ? x.FirstName.ToLower() : ""),
                    "LastName" => sorting.ApplyExpressions(list, x => !string.IsNullOrEmpty(x.LastName) ? x.LastName.ToLower() : ""),
                    "PostalCode" => sorting.ApplyExpressions(list, x => x.PostalCode),
                    "City" => sorting.ApplyExpressions(list, x => x.City),
                    _ => sorting.ApplyExpressions(list, x => x.IdAdherent)
                };
            }
            else
            {
                list = sorting.ApplyExpressions(list, x => x.IdAdherent);
            }
            return list;
        }

        private IQueryable<Adherent> FilterDate(IQueryable<Adherent> adherents, DateRange dateRange)
        {
            if (dateRange.Start.HasValue)
            {
                adherents = adherents.Where(o => o.InscriptionDate >= dateRange.Start);
            }
            if (dateRange.End.HasValue)
            {
                adherents = adherents.Where(o => o.InscriptionDate <= dateRange.End.Value.AddDays(1));
            }

            return adherents;
        }

        private Func<AdherentFilter, Expression<Func<Adherent, bool>>> ApplyFilters
           => (filters)
               => (adherent)
                    => (!filters.HasPhoto.HasValue || (filters.HasPhoto.HasValue && filters.HasPhoto.Value ? !string.IsNullOrEmpty(adherent.Photo) : string.IsNullOrEmpty(adherent.Photo)))
                        &&
                        (!filters.HasLicence.HasValue || (filters.HasLicence.HasValue && filters.HasLicence.Value ? !string.IsNullOrEmpty(adherent.Licence) : string.IsNullOrEmpty(adherent.Licence)))
                        &&
                        (!filters.IdSection.HasValue || adherent.IdSection == filters.IdSection)
                        &&
                        (!filters.IdCategory.HasValue || adherent.IdCategory == filters.IdCategory)
               ;

    }
}
