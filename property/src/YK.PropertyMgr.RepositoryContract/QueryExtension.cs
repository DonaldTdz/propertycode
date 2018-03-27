// ================================================================================
// Create by  : Donald Tang
// Create date: 2011-09-15
// Description: Some extension class and method for EF IQueryable.
// --------------------------------------------------------------------------------
// Modification History:
// 
// ================================================================================

namespace YK.PropertyMgr.RepositoryContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Reflection;

    /// <summary>
    /// Query extension
    /// </summary>
    public static class QueryExtension
    {
        /// <summary>
        /// Sorting and Pagging, sotring expression is mandatory.
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="sortExpression">Sorting expression</param>
        /// <param name="startRowIndex">Start wow index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Out int Total Count</param>
        /// <returns>Entity List</returns>
        public static IQueryable<T> SortingAndPaging<T>(this IQueryable<T> source, string sortExpression, int startRowIndex, int pageSize, out int totalCount)
            where T : class
        {
            return SortingAndPaging(source, null, sortExpression, startRowIndex, pageSize, out totalCount);
        }

        /// <summary>
        /// Sorting and Pagging, sotring expression is mandatory.
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="sortExpression">Sorting expression</param>
        /// <param name="startRowIndex">Start wow index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Out int Total Count</param>
        /// <returns>Entity List</returns>
        public static IQueryable<T> SortingAndPaging<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, string sortExpression, int startRowIndex, int pageSize, out int totalCount)
            where T : class
        {
            if (predicate != null)
                source = source.Where(predicate);

            totalCount = source.Count<T>();
            if (pageSize <= 0) pageSize = int.MaxValue;

            return source.Sorting(sortExpression).Skip(startRowIndex).Take(pageSize).AsQueryable();
        }


        /// <summary>
        /// Sorting by sorting expression, sotring expression is mandatory.
        /// </summary>
        /// <typeparam name="T">Base Entity Object</typeparam>
        /// <param name="source">IQueryable</param>
        /// <param name="sortExpression">sorting expression e.g.: userid,userName desc, mandatory</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> Sorting<T>(this IQueryable<T> source, string sortExpression) where T : class
        {
            if (string.IsNullOrEmpty(sortExpression))
            {
                sortExpression = typeof(T).GetProperties()[0].Name;
            }

            String[] orderFields = sortExpression.Split(',');
            IOrderedQueryable<T> result = null;
            for (int currentFieldIndex = 0; currentFieldIndex < orderFields.Length; currentFieldIndex++)
            {
                String[] expressionPart = orderFields[currentFieldIndex].Trim().Split(' ');
                String sortField = expressionPart[0];
                Boolean sortDescending = (expressionPart.Length == 2) && (expressionPart[1].Equals("DESC", StringComparison.OrdinalIgnoreCase));
                if (sortDescending)
                {
                    result = currentFieldIndex == 0 ? source.OrderByDescending(sortField) : result.ThenByDescending(sortField);
                }
                else
                {
                    result = currentFieldIndex == 0 ? source.OrderBy(sortField) : result.ThenBy(sortField);
                }
            }
            return result;
        }

        /// <summary>
        /// Filters an IQueryable(Of T) according to the specified condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, Condition<T> condition)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (condition == null) throw new ArgumentNullException("condition");

            var callExpr = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType },
                source.Expression, Expression.Quote(condition.ExpressionBody));

            return (IQueryable<T>)(source.Provider.CreateQuery(callExpr));
        }

        #region private helper method for sorting

        /// <summary>
        /// Generate Selector Lambda Expression
        /// </summary>
        /// <typeparam name="T">Entity Object</typeparam>
        /// <param name="propertyName">property Name</param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        private static LambdaExpression GenerateSelector<T>(String propertyName, out Type resultType) where T : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(T), "Entity");
            //  create the selector part, but support child properties
            PropertyInfo property = null;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.       
                String[] childProperties = propertyName.Split('.');
                Type t;
                propertyAccess = parameter;

                for (int i = 0; i < childProperties.Length; i++)
                {
                    if (property == null)
                        t = typeof(T);
                    else
                        t = property.PropertyType;

                    property = t.GetProperty(childProperties[i]);//t.GetProperty(propertyName);
                    if (property == null)
                        property = t.GetProperties().Where(t1 => t1.Name.ToUpperInvariant() == propertyName.ToUpperInvariant()).FirstOrDefault();

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                Type t = typeof(T);
                property = t.GetProperty(propertyName);
                if (property == null)
                    property = t.GetProperties().Where(t1 => t1.Name.ToUpperInvariant() == propertyName.ToUpperInvariant()).FirstOrDefault();
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.         
            return Expression.Lambda(propertyAccess, parameter);
        }

        /// <summary>
        /// Generates the method call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static MethodCallExpression GenerateMethodCall<T>(IQueryable<T> source, string methodName, String fieldName) where T : class
        {
            Type type = typeof(T);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<T>(fieldName, out selectorResultType);
            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { type, selectorResultType }, source.Expression, Expression.Quote(selector));
            return resultExp;
        }

        /// <summary>
        /// Orders T by field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string fieldName) where T : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<T>(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
        }

        /// <summary>
        /// Orders the T by descending.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string fieldName) where T : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<T>(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
        }

        /// <summary>
        /// Then order by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string fieldName) where T : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<T>(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
        }

        /// <summary>
        /// ThenByDescending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string fieldName) where T : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<T>(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
        }

        #endregion


    }


    /// <summary>
    /// Expresssion condtion support re-visit.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Condition<T>
    {
        /// <summary>
        ///  this ExpressionBody
        /// </summary>
        public Expression<Func<T, bool>> ExpressionBody { get; set; }

        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="expression">this expression</param>
        public Condition(Expression<Func<T, bool>> expression) { ExpressionBody = expression; }

        /// <summary>
        ///  Generate a Expression tree condition
        /// </summary>
        /// <param name="left">this left </param>
        /// <param name="right">this right </param>
        /// <returns></returns>
        public static Condition<T> operator &(Condition<T> left, Condition<T> right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");

            DynamicExpression<T> builder = new DynamicExpression<T>();
            return new Condition<T>(builder.BuildAndQuery(left.ExpressionBody, right.ExpressionBody));
        }

        /// <summary>
        /// Bit wise And
        /// </summary>
        /// <param name="right">this right</param>
        /// <returns></returns>
        public Condition<T> BitwiseAnd(Condition<T> right)
        {
            return this & right;
        }

        /// <summary>
        ///  Generate a Expression tree condition
        /// </summary>
        /// <param name="left">this left</param>
        /// <param name="right">this right</param>
        /// <returns></returns>
        public static Condition<T> operator |(Condition<T> left, Condition<T> right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");

            DynamicExpression<T> builder = new DynamicExpression<T>();
            return new Condition<T>(builder.BuildOrQuery(left.ExpressionBody, right.ExpressionBody));
        }

        /// <summary>
        ///  Bit wise Or
        /// </summary>
        /// <param name="right">this right</param>
        /// <returns></returns>
        public Condition<T> BitwiseOr(Condition<T> right)
        {
            return this | right;
        }
    }

    /// <summary>
    /// Dynamic Expresssion
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DynamicExpression<T>
    {
        ParameterExpression parameter;

        public DynamicExpression()
        {
            parameter = Expression.Parameter(typeof(T), "parameter");
        }

        public Expression<Func<T, bool>> BuildAndQuery(Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            ParameterExpressionRewriter param = new ParameterExpressionRewriter(parameter);
            leftExpression = param.VisitAndConvert(leftExpression, "BuildAndQuery");
            rightExpression = param.VisitAndConvert(rightExpression, "BuildAndQuery");
            var result = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            return Expression.Lambda<Func<T, bool>>(result, parameter);
        }

        public Expression<Func<T, bool>> BuildOrQuery(Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            ParameterExpressionRewriter param = new ParameterExpressionRewriter(parameter);
            leftExpression = param.VisitAndConvert(leftExpression, "BuildOrQuery");
            rightExpression = param.VisitAndConvert(rightExpression, "BuildOrQuery");
            var result = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            return Expression.Lambda<Func<T, bool>>(result, parameter);
        }
    }

    /// <summary>
    /// Parameter Expression Rewriter
    /// </summary>
    class ParameterExpressionRewriter : ExpressionVisitor
    {
        ParameterExpression _parameter;

        public ParameterExpressionRewriter(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node != null && node.GetType() != _parameter.GetType())
                return node;
            else
                return _parameter;
        }
    }

}
