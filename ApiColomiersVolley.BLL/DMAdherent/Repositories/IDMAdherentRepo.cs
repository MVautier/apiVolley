﻿using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ApiColomiersVolley.BLL.DMAdherent.Repositories
{
    public interface IDMAdherentRepo
    {
        Task<IEnumerable<DtoAdherent>> GetAdherents();
        Task<IEnumerable<DtoAdherent>> SearchAdherents(string name, string cp);
    }
}