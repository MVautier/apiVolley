using ApiColomiersVolley.BLL.Core.Models.Enums;
using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.Core.Tools;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMAdherent.Repositories;
using ApiColomiersVolley.BLL.DMItem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
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
        private readonly IServiceSendMail _mailManager;

        public BSAdherent(IDMAdherentRepo adherentRepo, IFileManager fileManager, IDMOrderRepo orderRepo, IServiceSendMail mailManager)
        {
            _adherentRepo = adherentRepo;
            _fileManager = fileManager;
            _orderRepo = orderRepo;
            _mailManager = mailManager;
        }

        public async Task<IEnumerable<DtoAdherent>> GetListe()
        {
            List<DtoAdherent> results = new List<DtoAdherent>();
            var adherents =  await _adherentRepo.GetAdherents();
            var orders = await _orderRepo.Get();
            foreach (var adherent in adherents)
            {
                adherent.Orders = orders.Where(o => o.IdAdherent == adherent.IdAdherent).OrderByDescending(o => o.Date).ToList();
                adherent.Membres = !string.IsNullOrEmpty(adherent.Address) ? 
                    adherents.Where(a => a.Address == adherent.Address && a.IdAdherent != adherent.IdAdherent && a.IdParent == adherent.IdAdherent && a.Saison == adherent.Saison).ToList()
                    : new List<DtoAdherent>();
                results.Add(adherent);
            }
            return results;
        }

        public async Task<DtoAdherent> Search(AdherentSearch search)
        {
            return await _adherentRepo.Search(search);
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
                        adherent.Orders = orders.OrderByDescending(o => o.Date).ToList();
                    }
                    
                    adherents.Add(adherent);
                }

                result = new PagedList<DtoAdherent>(adherents, result.Count);
            }

            return result;
        }

        public async Task<List<DtoStat>> GetStats()
        {
            List<DtoStat> stats = new List<DtoStat>();
            IEnumerable<DtoAdherent> adherents = await _adherentRepo.GetAdherents();
            stats.Add(new DtoStat { type = "geo", label = "Colomiers", value = adherents.Where(a => a.PostalCode == "31770").Count() });
            stats.Add(new DtoStat { type = "geo", label = "Externe", value = adherents.Where(a => a.PostalCode != "31770").Count() });

            stats.Add(new DtoStat { type = "category", label = "Compétition", value = adherents.Where(a => a.Category == "C").Count() });
            stats.Add(new DtoStat { type = "category", label = "Loisir", value = adherents.Where(a => a.Category == "L").Count() });
            stats.Add(new DtoStat { type = "category", label = "Enfants", value = adherents.Where(a => a.Category == "E").Count() });
            return stats;
        }

        public async Task<List<DtoOrderFull>> GetOrders(OrderFilter search)
        { 
            List<DtoOrderFull> orders = new List<DtoOrderFull>();
            List<DtoOrderFull> ordersHello = new List<DtoOrderFull>();
            List<DtoOrderFull> ordersManual = new List<DtoOrderFull>();
            IEnumerable<DtoAdherent> adherents = await _adherentRepo.GetAdherents();
            
            var commandes = await _orderRepo.Get();
            foreach (var adherent in adherents)
            {
                if (adherent.Payment == "Terminé" || adherent.Payment == "En attente" || adherent.Payment == "Manuel")
                {
                    var membres = adherents.Where(a => a.IdParent == adherent.IdAdherent && a.Saison == adherent.Saison);
                    if (adherent.Payment == "Terminé" || adherent.Payment == "En attente")
                    {
                        // Paiements Helloasso
                        var cmds = commandes.Where(c => c.IdAdherent == adherent.IdAdherent);
                        if (cmds.Any())
                        {
                            foreach (var cmd in cmds)
                            {
                                if (search.start.HasValue && search.end.HasValue && cmd.Date >= search.start.Value && cmd.Date <= search.end.Value)
                                {
                                    ordersHello.Add(new DtoOrderFull(adherent, cmd, membres));
                                }
                                else if (search.season.HasValue && (search.season.Value == 0 || cmd.Saison == search.season.Value))
                                {
                                    ordersHello.Add(new DtoOrderFull(adherent, cmd, membres));
                                }
                            }
                        }
                    }
                    else if (adherent.Payment == "Manuel")
                    {
                        // Paiements manuels
                        if (search.start.HasValue && search.end.HasValue && adherent.InscriptionDate.HasValue && adherent.InscriptionDate.Value >= search.start.Value && adherent.InscriptionDate.Value <= search.end.Value)
                        {
                            ordersManual.Add(new DtoOrderFull(adherent, null, membres));
                        }
                        else if (search.season.HasValue && (search.season.Value == 0 || adherent.Saison == search.season.Value))
                        {
                            ordersManual.Add(new DtoOrderFull(adherent, null, membres));
                        }
                    }
                } 
            }

            if (ordersHello.Any())
            {
                orders.AddRange(ordersHello.OrderBy(o => o.Date?.ToString("yyyy-MM-dd")).ThenBy(o => o.LastName.ToLower()));
            }

            if (ordersManual.Any())
            {
                orders.AddRange(ordersManual.OrderBy(o => o.InscriptionDate?.ToString("yyyy-MM-dd")).ThenBy(o => o.LastName.ToLower()));
            }

            return orders;
        }

        public async Task<FileModel> GetDocuments(AdherentFilter filter, string type)
        {
            var result = await _adherentRepo.GetPagedAdherents(filter, null, null);
            var docs = GetDocuments(result, type);
            if (docs.Any())
            {
                string zipfile = _fileManager.CreateZipFile(type + "s", docs);
                var file = await _fileManager.GetFile(zipfile);
                return file;
            }
            return null;
        }

        private List<DtoDocument> GetDocuments(PagedList<DtoAdherent> list, string type)
        {
            List<DtoDocument> docs = new List<DtoDocument>();
            if (list.Count > 0 && list.Datas.Any())
            {
                foreach (var adherent in list.Datas)
                {
                    if (adherent.Uid != null && adherent.FirstName != null && adherent.LastName != null && adherent.BirthdayDate != null)
                    {
                        var paths = _fileManager.InitAdherentPaths(adherent.Uid, false);
                        var files = _fileManager.FindFiles(paths.BasePath);
                        if (files != null && files.Any())
                        {
                            foreach (var file in files)
                            {
                                string _type = _fileManager.GetTypeByName(file.Name);
                                if (_type == type)
                                {
                                    docs.Add(new DtoDocument
                                    {
                                        filename = Path.Combine(adherent.Uid, file.Name),
                                        customName = GetDocumentName(adherent, file.Name),
                                        type = type,
                                        blob = null,
                                        sent = true
                                    });
                                }
                                
                            }
                        }
                    }
                }
            }

            return docs;
        }

        private string GetDocumentName(DtoAdherent adherent, string filename)
        {
            string name = adherent.FirstName.ToLower().Replace(" ", "-") + "-" + 
                adherent.LastName.ToLower().Replace(" ", "-") + "-" +
                LeadZero(adherent.BirthdayDate.Value.Day) + "-" + LeadZero(adherent.BirthdayDate.Value.Month) + "-" + adherent.BirthdayDate.Value.Year.ToString() + "-" +  
                filename;
            return name;
        }

        private string LeadZero(int value)
        {
            if (value < 10)
            {
                return "0" + value.ToString();
            }
            return value.ToString();
        }

        public async Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp)
        {
            return await _adherentRepo.SearchAdherents(name, cp);
        }

        public async Task<DtoAdherent> AddOrUpdate(DtoAdherent adherent)
        {
            try
            {
                if (string.IsNullOrEmpty(adherent.Phone))
                {
                    adherent.Phone = "";
                }

                if (string.IsNullOrEmpty(adherent.Email))
                {
                    adherent.Email = "";
                }

                if (adherent.InscriptionDate.HasValue)
                {
                    if (adherent.Histo != null && adherent.Histo.Any())
                    {
                        var histo = adherent.Histo.FirstOrDefault(h => h.saison == adherent.Saison);
                        if (histo == null)
                        {
                            adherent.Histo.Insert(0, new DtoHisto { saison = adherent.Saison, date = adherent.InscriptionDate.Value.ToString("yyyy-MM-dd"), Category = IntCategory(adherent.Category) });
                        }
                    }
                    else
                    {
                        adherent.Histo = new List<DtoHisto> { new DtoHisto { saison = adherent.Saison, date = adherent.InscriptionDate.Value.ToString("yyyy-MM-dd"), Category = IntCategory(adherent.Category) } };
                    }
                }
                

                DtoAdherent result = await _adherentRepo.AddOrUpdate(adherent);
                if (result != null)
                {
                    if (adherent.Membres.Any())
                    {
                        foreach (DtoAdherent membre in adherent.Membres)
                        {
                            membre.IdParent = result.IdAdherent;
                            membre.Payment = result.Payment;
                            await _adherentRepo.AddOrUpdate(membre);
                        }
                    }

                    if (adherent.Orders.Any(o => o != null))
                    {
                        adherent.Orders = adherent.Orders.Where(o => o != null).ToList();
                        foreach (var order in adherent.Orders)
                        {
                            if (order.IdAdherent == 0)
                            {
                                order.IdAdherent = result.IdAdherent;
                            }

                            if (order.Saison == 0 && result.Saison.HasValue)
                            {
                                order.Saison = result.Saison.Value;
                            }

                            await _orderRepo.AddOrUpdate(order);
                        }
                        
                    }
                }

#if (!DEBUG)
            await SendMailInfo(result);
#endif

                return result;
            }
            catch (Exception ex)
            {
                string content = JsonConvert.SerializeObject(adherent);
                await _mailManager.SendMailErreur(ex, "Erreur saving adherent: " + Environment.NewLine + content);
            }

            return null;

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

        public async Task<FileModel> GetEmailFile(AdherentFilter filter)
        {
            var adherents = await _adherentRepo.GetPagedAdherents(filter, null, null);
            var fileName = "Emails";

            if (adherents != null && adherents.Datas.Any())
            {
                List<string> emails = adherents.Datas.Select(a => a.Email + ";").ToList();
                var fileBytes = await _fileManager.CreateEmailFile(emails, fileName, "txt");
                return new FileModel(fileName + ".txt", fileBytes, "application/octet-stream");
            }

            throw new ArgumentNullException("Aucun compte n'a été trouvé");
        }

        public async Task<FileModel> GetOrderFile(AdherentFilter filter)
        {
            var adherents = await GetPagedListe(filter, null, null);
            var fileName = "Paiements";
            var orders_hello = new List<DtoOrderExport>();
            var orders_manuals = new List<DtoOrderExport>();
            var end = filter.DateRange.End.HasValue ? filter.DateRange.End.Value : DateTime.Now.AddDays(1);

            if (adherents != null && adherents.Datas.Any())
            {

                foreach (var a in adherents.Datas)
                {
                    a.Membres = !string.IsNullOrEmpty(a.Address) ?
                            adherents.Datas.Where(_a => _a.Address == a.Address && _a.IdAdherent != a.IdAdherent && _a.IdParent == a.IdAdherent && _a.Saison == a.Saison).ToList()
                            : new List<DtoAdherent>();
                    if (a.Saison == filter.Saison && a.Orders != null && a.Orders.Any())
                    {
                        foreach(var o in a.Orders)
                        {
                            if (o.Date >= filter.DateRange.Start && o.Date <= end)
                            {
                                orders_hello.Add(new DtoOrderExport
                                {
                                    Id = o.Id,
                                    IdPaiement = o.IdPaiement,
                                    IdAdherent = a.IdAdherent,
                                    Date = a.InscriptionDate,
                                    Nom = a.LastName,
                                    CotisationC3L = o.CotisationC3L,
                                    Total = o.Total,
                                    Prenom = a.FirstName,
                                    Email = a.Email,
                                    DateNaissance = a.BirthdayDate,
                                    PaymentLink = o.PaymentLink,
                                    Payment = a.Payment,
                                    PaymentComment = a.PaymentComment,
                                    Members = GetMembersInfo(a)
                                });
                            }
                        }
                        
                    } 
                    else if (!string.IsNullOrEmpty(a.Payment) && a.Payment == "Manuel" && a.InscriptionDate >= filter.DateRange.Start && a.InscriptionDate <= end)
                    {
                        orders_manuals.Add(new DtoOrderExport
                        {
                            Id = 0,
                            IdPaiement = 0,
                            IdAdherent = a.IdAdherent,
                            Date = a.InscriptionDate,
                            Nom = a.LastName,
                            Prenom = a.FirstName,
                            Email = a.Email,
                            DateNaissance = a.BirthdayDate,
                            Payment = a.Payment,
                            PaymentComment = a.PaymentComment,
                            Members = GetMembersInfo(a)
                        });
                    }
                }
            }

            var orders = orders_hello.Concat(orders_manuals).ToList();
            if (orders.Any())
            {
                var sheetName = filter.DateRange != null ? $"Paiements_{filter.DateRange.Start.GetValueOrDefault():dd'_'MM'_'yyyy}_{filter.DateRange.End.GetValueOrDefault():dd'_'MM'_'yyyy}" : "Adhérents";
                var fileBytes = await _fileManager.CreateExcelFile<DtoOrderExport>(orders, fileName, sheetName);
                return new FileModel(fileName + ".xlsx", fileBytes, "application/octet-stream");
            }

            throw new ArgumentNullException("Aucun paiement n'a été trouvé");
        }

        private string GetMembersInfo(DtoAdherent adherent)
        {
            string info = "";
            int i = 0;
            if (adherent.Membres != null && adherent.Membres.Any())
            {
                foreach(var m in adherent.Membres)
                {
                    info += i > 0 ? ", " : "";
                    info  += m.FirstName + " " + m.LastName;
                    if (m.BirthdayDate.HasValue)
                    {
                        info += " (" + m.BirthdayDate.Value.ToString("dd/MM/yyyy") + ")";
                    }
                    i++;
                }
            }
            return info;
        }

        private async Task SendMailInfo(DtoAdherent adherent)
        {
            try
            {
                string title = "Adherent was saved";
                string content = JsonConvert.SerializeObject(adherent);
                await _mailManager.SendMailSimple(title, content);
            }
            catch (Exception)
            {

            }
        }

        public int IntCategory(string code)
        {
            switch (code)
            {
                case "C": return 1;
                case "L": return 2;
                case "E": return 3;
                default: return 2;
            }
        }
    }
}
