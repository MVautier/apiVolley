using ApiColomiersVolley.BLL.DMAdherent.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = ApiColomiersVolley.BLL.DMParametres.Models;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionParametres
    {
        internal static Models.DtoParametres ToDtoParametres(this Parametres param)
        {
            if (param == null)
            {
                return null;
            }

            return new Models.DtoParametres
            {
                IdParametre = param.IdParametre,
                InscriptionOpened = param.InscriptionOpened,
                Reinscription = param.Reinscription,
                InscriptionFilter = param.InscriptionFilter,
                AdoOpened = param.AdoOpened,
                LoisirOpened = param.LoisirOpened,
                CompetOpened = param.CompetOpened,
                NbAdoMax = param.NbAdoMax,
                TarifLocal = param.TarifLocal,
                TarifExterior = param.TarifExterior,
                TarifMember = param.TarifMember,
                TarifLoisir = param.TarifLoisir,
                TarifLicense = param.TarifLicense,
                TarifAdo = param.TarifAdo,
                SubHeader = param.SubHeader,
                Text1 = param.Text1,
                Text2 = param.Text2,
                Text3 = param.Text3
            };
        }

        internal static Parametres ToParametres(this Models.DtoParametres param, Parametres source)
        {
            if (param == null)
            {
                return null;
            }

            source.IdParametre = param.IdParametre;
            source.InscriptionOpened = param.InscriptionOpened;
            source.Reinscription = param.Reinscription;
            source.InscriptionFilter = param.InscriptionFilter;
            source.AdoOpened = param.AdoOpened;
            source.LoisirOpened = param.LoisirOpened;
            source.CompetOpened = param.CompetOpened;
            source.NbAdoMax = param.NbAdoMax;
            source.TarifLocal = param.TarifLocal;
            source.TarifExterior = param.TarifExterior;
            source.TarifMember = param.TarifMember;
            source.TarifLoisir = param.TarifLoisir;
            source.TarifLicense = param.TarifLicense;
            source.TarifAdo = param.TarifAdo;
            source.SubHeader = param.SubHeader;
            source.Text1 = param.Text1;
            source.Text2 = param.Text2;
            source.Text3 = param.Text3;

            return source;
        }

        internal static Parametres ToParametresAdd(this Models.DtoParametres param)
        {
            if (param == null)
            {
                return null;
            }

            return new Parametres
            {
                IdParametre = param.IdParametre,
                InscriptionOpened = param.InscriptionOpened,
                Reinscription = param.Reinscription,
                InscriptionFilter = param.InscriptionFilter,
                AdoOpened = param.AdoOpened,
                LoisirOpened = param.LoisirOpened,
                CompetOpened = param.CompetOpened,
                NbAdoMax = param.NbAdoMax,
                TarifLocal = param.TarifLocal,
                TarifExterior = param.TarifExterior,
                TarifMember = param.TarifMember,
                TarifLoisir = param.TarifLoisir,
                TarifLicense = param.TarifLicense,
                TarifAdo = param.TarifAdo,
                SubHeader = param.SubHeader,
                Text1 = param.Text1,
                Text2 = param.Text2,
                Text3 = param.Text3
            };
        }
    }
}
