using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionSection
    {
        internal static DtoSection ToDtoSection(this Section section)
        {
            if (section == null)
            {
                return null;
            }

            return new DtoSection
            {
                IdSection = section.IdSection,
                Libelle = section.Libelle
            };
        }

        internal static IEnumerable<DtoSection> ToDtoSection(this IEnumerable<Section> sections)
        {
            return sections.Select(d => d.ToDtoSection()).ToList();
        }
    }
}
