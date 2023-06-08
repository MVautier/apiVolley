using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMItem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Business
{
    public class BSAdherent : IBSAdherent
    {
        private readonly IDMAdherentRepo _adherentRepo;
        private readonly IDMOrderRepo _orderRepo;
        private readonly IFileManager _fileManager;

        public BSAdherent(IDMAdherentRepo adherentRepo, IFileManager fileManager, IDMOrderRepo orderRepo)
        {
            _adherentRepo = adherentRepo;
            _fileManager = fileManager;
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<DtoAdherent>> GetListe()
        {
            List<DtoAdherent> results = new List<DtoAdherent> ();
            var adherents =  await _adherentRepo.GetAdherents();
            foreach (var adherent in adherents)
            {
                adherent.Membres = adherents.Where(a => a.Address == adherent.Address && a.IdAdherent != adherent.IdAdherent).ToList();
                results.Add(adherent);
            }
            return results;
        }

        public async Task<PagedList<DtoAdherent>> GetPagedListe(AdherentFilter filter, Sorting sort, Pagination pager)
        {
            var result = await _adherentRepo.GetPagedAdherents(filter, sort, pager);
            if (result.Count > 0 && result.Datas.Any())
            {
                List<DtoAdherent> adherents = new List<DtoAdherent>();
                foreach(var adherent in result.Datas)
                {
                    List<DtoDocument> docs = new List<DtoDocument>();
                    if (adherent.Uid != null)
                    {
                        var paths = _fileManager.InitAdherentPaths(adherent.Uid, false);
                        var files = _fileManager.FindFiles(paths.BasePath);
                        if (files != null && files.Any())
                        {
                            foreach ( var file in files)
                            {
                                docs.Add(new DtoDocument
                                {
                                    filename = file.Name,
                                    type = _fileManager.GetTypeByName(file.Name),
                                    blob = null,
                                    sent = true
                                });
                            }
                        }
                    }

                    adherent.Documents = docs;
                    var orders = await _orderRepo.GetByAdherent(adherent.IdAdherent);
                    if (orders.Any())
                    {
                        adherent.Order = orders.OrderByDescending(o => o.Date).FirstOrDefault();
                    }
                    
                    adherents.Add(adherent);
                }

                result = new PagedList<DtoAdherent>(adherents, result.Count);
            }

            return result;
        }

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return await _adherentRepo.SearchAdherents(name, cp);
        }

        public async Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent)
        {
            DtoAdherent result = await _adherentRepo.AddOrUpdate(adherent);
            if (result != null) 
            {
                if (adherent.Membres.Any())
                {
                    foreach (DtoAdherent membre in adherent.Membres)
                    {
                        membre.IdParent = result.IdAdherent;
                        await _adherentRepo.AddOrUpdate(membre);
                    }
                }

                if (adherent.Order != null)
                {
                    if (adherent.Order.IdAdherent == 0)
                    {
                        adherent.Order.IdAdherent = result.IdAdherent;
                    }

                    await _orderRepo.AddOrUpdate(adherent.Order);
                }
            }

            return result;
        }

        public async Task<FileModel> GetExcelFile(AdherentFilter filter)
        {
            var adherents = await _adherentRepo.GetPagedAdherents(filter, null, null);
            var fileName = "Adhérents";

            if (adherents != null && adherents.Datas.Any())
            {
                var sheetName = filter.DateRange != null ? $"Adherents_{filter.DateRange.Start.GetValueOrDefault():dd'_'MM'_'yyyy}_{filter.DateRange.End.GetValueOrDefault():dd'_'MM'_'yyyy}" : "Adhérents";
                var fileBytes = await _fileManager.CreateExcelFile<DtoAdherent>(adherents.Datas, fileName, sheetName);
                return new FileModel(fileName + ".xlsx", fileBytes, "application/octet-stream");
            }

            throw new ArgumentNullException("Aucun compte n'a été trouvé");
        }
    }
}
