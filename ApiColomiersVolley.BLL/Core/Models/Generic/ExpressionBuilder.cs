using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string), typeof(StringComparison) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string), typeof(StringComparison) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string), typeof(StringComparison) });

        public static Expression<Func<T, bool>> GetExpression<T>(IList<DynamicFilter> filters)
        {
            filters = filters.Any() ? filters.Where(f => !IsEmpty(f)).ToList() : new List<DynamicFilter>();

            if (!filters.Any())
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(param, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = GetExpression<T>(param, filters[0], filters[1]);
            }
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    exp = exp == null
                        ? GetExpression<T>(param, filters[0], filters[1])
                        : Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static bool IsEmpty(DynamicFilter filter)
        {
            return filter.Field == null || filter.Value == null || string.IsNullOrEmpty(filter.Value) || filter.Operator == null;
        }

        private static Expression GetExpression<T>(ParameterExpression param, DynamicFilter filter)
        {
            MemberExpression member = Expression.Property(param, filter.Field);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;

            var converter = TypeDescriptor.GetConverter(propertyType); // 1
            if (!converter.CanConvertFrom(typeof(string))) // 2
                throw new NotSupportedException();

            var propertyValue = converter.ConvertFromInvariantString(filter.Value); // 3
            var constant = Expression.Constant(propertyValue);
            var valueExpression = Expression.Convert(constant, propertyType); // 4

            switch (filter.Operator)
            {
                case "Equals":
                    return Expression.Equal(member, valueExpression);
                case "Contains":
                    return Expression.Call(member, containsMethod, constant, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                case "StartsWith":
                    return Expression.Call(member, startsWithMethod, constant, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                case "EndsWith":
                    return Expression.Call(member, endsWithMethod, constant, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                case "GreaterThan":
                    return Expression.GreaterThan(member, constant);
                case "GreaterThanOrEqual":
                    return Expression.GreaterThanOrEqual(member, constant);
                case "LessThan":
                    return Expression.LessThan(member, constant);
                case "LessThanOrEqual":
                    return Expression.LessThanOrEqual(member, constant);
                default:
                    return Expression.Equal(member, valueExpression);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, DynamicFilter filter1, DynamicFilter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }
    }
}
