﻿using ApiColomiersVolley.BLL.Core.Models.Enums;
using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.DAL.Entities;
using ApiColomiersVolley.DAL.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Crypto;
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
            if (adherent.IdAdherent > 0)
            {
                Adherent adh = await GetAll().FirstOrDefaultAsync(a => a.IdAdherent == adherent.IdAdherent);
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

        public async Task<IEnumerable<DtoAdherent>> GetByIds(List<int> ids)
        {
            return (await GetAll().Where(a => ids.Contains(a.IdAdherent)).ToListAsync()).ToDtoAdherent();
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

        public async Task<DtoAdherent> Search(AdherentSearch search)
        {
            var adherents = GetAll();
            if (search != null)
            {
                var filters = new List<DynamicFilter>();
                if (!string.IsNullOrEmpty(search.nom))
                {
                    filters.Add(new DynamicFilter
                    {
                        Field = "LastName",
                        Operator = "Equals",
                        Value = search.nom
                    });
                }

                if (!string.IsNullOrEmpty(search.prenom))
                {
                    filters.Add(new DynamicFilter
                    {
                        Field = "FirstName",
                        Operator = "Equals",
                        Value = search.prenom
                    });
                }

                var predicate = ExpressionBuilder.GetExpression<Adherent>(filters);
                if (predicate != null)
                {
                    adherents = adherents.Where(predicate);
                }

                if (search.birthdayDate.HasValue)
                {
                    adherents = adherents.Where(a => a.BirthdayDate.HasValue 
                    && a.BirthdayDate.Value.Year == search.birthdayDate.Value.Year
                    && a.BirthdayDate.Value.Month == search.birthdayDate.Value.Month
                    && a.BirthdayDate.Value.Day == search.birthdayDate.Value.Day);
                }

                if (adherents.Any()) 
                {
                    var adherent = adherents.OrderByDescending(a => a.IdAdherent).First();
                    return adherent.ToDtoAdherent();
                }
            }

            return null;
        }

        private async Task<IQueryable<Adherent>> GetFilteredAdherents(AdherentFilter? filter)
        {
            var adherents = GetAll();
            if (filter != null)
            {
                var now = DateTime.Now;
                var y = now.Year;
                var m = now.Month;

                if (filter.Saison != null)
                {
                    adherents = adherents.Where(a => a.Saison == filter.Saison);
                }

                if (filter.Payment.HasValue)
                {
                    var query = from a in adherents
                                join o in _db.Orders on a.IdAdherent equals o.IdAdherent
                                where a.Saison == o.Saison
                                select a.IdAdherent;
                    var query2 = from a in adherents
                                 join o in _db.Orders on a.IdParent equals o.IdAdherent
                                 where a.Saison == o.Saison
                                 select a.IdParent;
                    switch (filter.Payment.Value)
                    {
                        case EnumPayment.Termine:
                            adherents = adherents.Where(a =>
                            (query.Contains(a.IdAdherent) || query2.Contains(a.IdParent))
                            && a.Payment == "Terminé");
                            break;
                        case EnumPayment.Attente:
                            adherents = adherents.Where(a => 
                            !(query.Contains(a.IdAdherent) || query2.Contains(a.IdParent))
                            && a.Payment == "En attente");
                            break;
                        case EnumPayment.Manuel:
                            adherents = adherents.Where(a => a.Payment == "Manuel");
                            break;
                        case EnumPayment.Autre:
                            adherents = adherents.Where(a => a.Payment != "Terminé" && a.Payment != "En attente" && a.Payment != "Manuel");
                            break;
                    }
                }

                if (filter.DateRange != null && (filter.DateRange.Start.HasValue || filter.DateRange.End.HasValue))
                {
                    adherents = FilterDate(adherents, filter.DateRange);
                }

                adherents = adherents.Where(ApplyFilters(filter));
                if (filter.Team != null)
                {
                    adherents = adherents.Where(a => a.IdCategory == 1 && (filter.Team == "sans" ? string.IsNullOrEmpty(a.Team1) && string.IsNullOrEmpty(a.Team2) : (a.Team1 == filter.Team || a.Team2 == filter.Team)));
                }

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
                    "Saison" => sorting.ApplyExpressions(list, x => x.Saison),
                    "BirthdayDate" => sorting.ApplyExpressions(list, x => x.BirthdayDate),
                    "InscriptionDate" => sorting.ApplyExpressions(list, x => x.InscriptionDate),
                    "CertificateDate" => sorting.ApplyExpressions(list, x => x.CertificateDate),
                    "HealthStatementDate" => sorting.ApplyExpressions(list, x => x.HealthStatementDate),
                    "FirstName" => sorting.ApplyExpressions(list, x => !string.IsNullOrEmpty(x.FirstName) ? x.FirstName.ToLower() : ""),
                    "LastName" => sorting.ApplyExpressions(list, x => !string.IsNullOrEmpty(x.LastName) ? x.LastName.ToLower() : ""),
                    "PostalCode" => sorting.ApplyExpressions(list, x => x.PostalCode),
                    "Payment" => sorting.ApplyExpressions(list, x => x.Payment),
                    "PaymentComment" => sorting.ApplyExpressions(list, x => x.PaymentComment),
                    "Category" => sorting.ApplyExpressions(list, x => x.Category.Code ?? ""),
                    "Section" => sorting.ApplyExpressions(list, x => x.Section.Libelle ?? ""),
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
                        // &&
                        //(filters.Team == null || adherent.Team1 == filters.Team || adherent.Team2 == filters.Team)
               ;

    }
}
