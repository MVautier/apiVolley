﻿using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces
{
    public interface IBSAdherent
    {
        Task<IEnumerable<DtoAdherent>> GetListe();
        Task<PagedList<DtoAdherent>> GetPagedListe(AdherentFilter filter, Sorting sort, Pagination pager);
        Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp);
        Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent);
        Task<FileModel> GetExcelFile(AdherentFilter filter);
        Task<FileModel> GetDocuments(AdherentFilter filter, string type);
        Task<FileModel> GetEmailFile(AdherentFilter filter);
        Task<FileModel> GetOrderFile(AdherentFilter filter);
        Task<DtoAdherent> Search(AdherentSearch search);
        Task<List<DtoOrderFull>> GetOrders(OrderFilter search);
        Task<List<DtoStat>> GetStats();
        Task<bool> RepairDocs();
    }
}
