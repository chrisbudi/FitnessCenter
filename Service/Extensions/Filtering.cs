using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Services.Extensions
{
    public class Filtering
    {
        private static readonly MethodInfo StringContainsMethod =
            typeof (string).GetMethod(@"Contains", BindingFlags.Instance |
                                                   BindingFlags.Public, null, new[] {typeof (string)}, null);

        private static readonly MethodInfo BooleanEqualsMethod =
            typeof (bool).GetMethod(@"Equals", BindingFlags.Instance |
                                               BindingFlags.Public, null, new[] {typeof (bool)}, null);

        public static Expression<Func<TDbType, bool>>
            BuildPredicate<TDbType, TSearchCriteria>(TSearchCriteria searchCriteria)
        {
            var predicate = PredicateBuilder.True<TDbType>();

            // Iterate the search criteria properties
            var searchCriteriaPropertyInfos = searchCriteria.GetType().GetProperties();
            foreach (var searchCriteriaPropertyInfo in searchCriteriaPropertyInfos)
            {
                // Get the name of the DB field, which may not be the same as the property name.
                //var dbFieldName = GetDbFieldName(searchCriteriaPropertyInfo);
                // Get the target DB type (table)
                var dbType = typeof (TDbType);
                // Get a MemberInfo for the type's field (ignoring case
                // so "FirstName" works as well as "firstName")
                //var dbFieldMemberInfo = dbType.GetMember(dbFieldName,
                    //BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Single();
                // STRINGS
                //if (searchCriteriaPropertyInfo.PropertyType == typeof (string))
                //{
                //    predicate = ApplyStringCriterion(searchCriteria,
                //        searchCriteriaPropertyInfo, dbType, dbFieldMemberInfo, predicate);
                //}
                //// BOOLEANS
                //else if (searchCriteriaPropertyInfo.PropertyType == typeof (bool?))
                //{
                //    predicate = ApplyBoolCriterion(searchCriteria,
                //        searchCriteriaPropertyInfo, dbType, dbFieldMemberInfo, predicate);
                //}
                // ADD MORE TYPES...
            }

            return predicate;
        }

        private static Expression<Func<TDbType, bool>> ApplyStringCriterion<TDbType,
            TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo,
                Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate)
        {
            // Check if a search criterion was provided
            var searchString = searchCriterionPropertyInfo.GetValue(searchCriteria) as string;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return predicate;
            }
            // Then "and" it to the predicate.
            // e.g. predicate = predicate.And(x => x.firstName.Contains(searchCriterion.FirstName)); ...
            // Create an "x" as TDbType
            var dbTypeParameter = Expression.Parameter(dbType, @"x");
            // Get at x.firstName
            var dbFieldMember = Expression.MakeMemberAccess(dbTypeParameter, dbFieldMemberInfo);
            // Create the criterion as a constant
            var criterionConstant = new Expression[] {Expression.Constant(searchString)};
            // Create the MethodCallExpression like x.firstName.Contains(criterion)
            var containsCall = Expression.Call(dbFieldMember, StringContainsMethod, criterionConstant);
            // Create a lambda like x => x.firstName.Contains(criterion)
            var lambda = Expression.Lambda(containsCall, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            // Apply!
            return predicate.And(lambda);
        }

        private static Expression<Func<TDbType, bool>> ApplyBoolCriterion<TDbType,
            TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo,
                Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate)
        {
            // Check if a search criterion was provided
            var searchBool = searchCriterionPropertyInfo.GetValue(searchCriteria) as bool?;
            if (searchBool == null)
            {
                return predicate;
            }
            // Then "and" it to the predicate.
            // e.g. predicate = predicate.And(x => x.isActive.Contains(searchCriterion.IsActive)); ...
            // Create an "x" as TDbType
            var dbTypeParameter = Expression.Parameter(dbType, @"x");
            // Get at x.isActive
            var dbFieldMember = Expression.MakeMemberAccess(dbTypeParameter, dbFieldMemberInfo);
            // Create the criterion as a constant
            var criterionConstant = new Expression[] {Expression.Constant(searchBool)};
            // Create the MethodCallExpression like x.isActive.Equals(criterion)
            var equalsCall = Expression.Call(dbFieldMember, BooleanEqualsMethod, criterionConstant);
            // Create a lambda like x => x.isActive.Equals(criterion)
            var lambda = Expression.Lambda(equalsCall, dbTypeParameter) as Expression<Func<TDbType, bool>>;
            // Apply!
            return predicate.And(lambda);
        }

        //private static string GetDbFieldName(PropertyInfo propertyInfo)
        //{
        //    var fieldMapAttribute =
        //        propertyInfo.GetCustomAttributes(typeof (FieldMapAttribute), false).FirstOrDefault();
        //    var dbFieldName = fieldMapAttribute != null
        //        ? ((FieldMapAttribute) fieldMapAttribute).Field
        //        : propertyInfo.Name;
        //    return dbFieldName;
        //}
    }
}