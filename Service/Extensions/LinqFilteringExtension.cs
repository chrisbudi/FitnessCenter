using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Services.Class;
using Services.Helpers;

namespace Services.Extensions
{

    public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod =
        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod =
        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type resultType) where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");
            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }


        public static Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

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

        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = null;
            var filterNested = filter.PropertyName.Split('.');
            if (filterNested.Count() == 1)
            {
                member = Expression.Property(param, filter.PropertyName);
            }
            else
            {
                MemberExpression exp = null;
                for (var i = 0; i < filterNested.Count() - 1; i++)
                {
                    if (exp == null)
                        exp = Expression.Property(param, filterNested[i]);
                    else
                        exp = Expression.Property(exp, filterNested[i]);
                }
                if (exp != null) member = Expression.Property(exp, filterNested[filterNested.Count() - 1]);
                //MemberExpression exp =
                //    Expression.Property(
                //        Expression.Property(Expression.Property(param, filterNested[0]), filterNested[1]),
                //        filterNested[2]);
            }

            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case EnumFilterOp.Equals:
                    return Expression.Equal(member, constant);

                case EnumFilterOp.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case EnumFilterOp.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case EnumFilterOp.LessThan:
                    return Expression.LessThan(member, constant);

                case EnumFilterOp.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case EnumFilterOp.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case EnumFilterOp.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case EnumFilterOp.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }
    }
}