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

        public async Task<IEnumerable<DtoAdherent>> GetAdherentsByCategoryAndSeason(int idCategory, int year)
        {
            return (await GetAll().Where(a => a.IdCategory == idCategory && a.Saison == year).ToListAsync()).ToDtoAdherent();
        }

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return (await GetAll().Where(a => a.LastName.ToLower() == name.ToLower() && a.PostalCode == cp).ToListAsync()).ToDtoAdherent();
        }

        public async Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent)
        {
            Adherent adh = await GetAll().FirstOrDefaultAsync(a => a.Uid == adherent.Uid);
            if (adh != null)
            {
                adh = adherent.ToAdherent(adh);
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                return adherent;
            }
            else if (adherent.IdAdherent > 0)
            {
                adh = await GetAll().FirstOrDefaultAsync(a => a.IdAdherent == adherent.IdAdherent);
                if (adh != null)
                {
                    adh = adherent.ToAdherent(adh);
                    await _db.SaveChangesAsync();
                    return adherent;
                }

                return null;
            }
            else
            {
                var newAdh = await _db.AddAsync(adherent.ToAdherentAdd());
                await _db.SaveChangesAsync();
                if (newAdh != null)
                {
                    adherent.IdAdherent = newAdh.Entity.IdAdherent;
                    return adherent;
                }

                return null;
            }
        }

        public async Task<PagedList<DtoAdherent>> GetPagedAdherents(AdherentFilter? filter, Sorting? sorting, Pagination? pagination)
        {
            var adherents = await GetFilteredAdherents(filter);
            
            if (sorting != null)
            {
                adherents = SortData(adherents, sorting);
            }

            IQueryable<Adherent> paginated = pagination != null ? pagination.Paginate(adherents) : adherents;
            return new PagedList<DtoAdherent>(paginated.ToDtoAdherent().ToList(), adherents.Count());
        }

        private async Task<IQueryable<Adherent>> GetFilteredAdherents(AdherentFilter? filter)
        {

            var adherents = GetAll();

            if (filter != null)
            {
                if (filter.HasPaid != null && filter.HasPaid.HasValue)
                {
                    var now = DateTime.Now;
                    var y = now.Year;
                    var m = now.Month;
                    var season = now.Month >= 6 ? y : y - 1;

                    var ids = await _db.Orders.Where(o => o.Date != null && o.Date.Value.Year >= season).Select(o => o.IdAdherent).ToListAsync();
                    if (ids.Any())
                    {
                        if (filter.HasPaid.Value == true)
                        {
                            adherents = adherents.Where(a => a.Saison == season && (ids.Contains(a.IdAdherent) && a.Payment != null && a.Payment != "En attente"));
                        }
                        else
                        {
                            adherents = adherents.Where(a => a.Saison == season && (!ids.Contains(a.IdAdherent) || a.Payment == null || a.Payment == "En attente"));
                        }
                    }
                }

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
            //if (dateRange.Start.HasValue)
            //{
            //    adherents = adherents.Where(o => o.InscriptionDate.HasValue && String.Compare(Date2String(o.InscriptionDate.Value), Date2String(dateRange.Start.Value)) >= 0);
            //}
            //if (dateRange.End.HasValue)
            //{
            //    adherents = adherents.Where(o => o.InscriptionDate.HasValue && String.Compare(Date2String(o.InscriptionDate.Value), Date2String(dateRange.End.Value.AddDays(1))) <= 0);
            //}
            if (dateRange.Start.HasValue)
            {
                adherents = adherents.Where(o => o.InscriptionDate >= dateRange.Start);
            }
            if (dateRange.End.HasValue)
            {
                adherents = adherents.Where(o => o.InscriptionDate < dateRange.End.Value.AddDays(1));
            }

            return adherents;
        }

        private string Date2String(DateTime date)
        {
            int year = date.Year;
            int month = date.Month; 
            int day = date.Day;
            return year.ToString() + (month < 10 ? "0" + month.ToString() : month.ToString()) + (day < 10 ? "0" + day.ToString() : day.ToString());
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
