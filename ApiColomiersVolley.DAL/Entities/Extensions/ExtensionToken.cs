using ApiColomiersVolley.BLL.DMAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.DAL.Entities.Extensions
{
    internal static class ExtensionToken
    {
        internal static DtoToken ToDtoToken(this Token token)
        {
            if (token == null)
            {
                return null;
            }

            return new DtoToken
            {
                IdToken = token.IdToken,
                Key = token.Key,
                BeginDate = token.BeginDate,
                EndDate = token.EndDate,
                IdConnexion = token.IdToken
            };
        }

        internal static List<DtoToken> ToDtoToken(this List<Token> tokens)
        {
            return tokens.Select(d => d.ToDtoToken()).ToList();
        }

        internal static Token ToToken(this DtoToken token)
        {
            if (token == null)
            {
                return null;
            }

            return new Token
            {
                IdToken = token.IdToken,
                Key = token.Key,
                BeginDate = token.BeginDate,
                EndDate = token.EndDate,
                IdConnexion = token.IdToken
            };
        }

        internal static List<Token> ToToken(this List<DtoToken> tokens)
        {
            return tokens.Select(d => d.ToToken()).ToList();
        }
    }
}
