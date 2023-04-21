using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMItem.Repositories;
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

        public BSAdherent(IDMAdherentRepo adherentRepo)
        {
            _adherentRepo = adherentRepo;
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

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return await _adherentRepo.SearchAdherents(name, cp);
        }

        public async Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent)
        {

            return adherent;
        }
    }
}
