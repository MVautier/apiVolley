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
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.DMAdherent.Business
{
    public class BSAdherent : IBSAdherent
    {
        private readonly IDMAdherentRepo _adherentRepo;
        private readonly IFileManager _fileManager;

        public BSAdherent(IDMAdherentRepo adherentRepo, IFileManager fileManager)
        {
            _adherentRepo = adherentRepo;
            _fileManager = fileManager;
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
            return await _adherentRepo.GetPagedAdherents(filter, sort, pager);
        }

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return await _adherentRepo.SearchAdherents(name, cp);
        }

        public async Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent)
        {

            return adherent;
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
