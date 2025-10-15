using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;

namespace Services.Specifications
{
    abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        #region Include
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new List<Expression<Func<TEntity, object>>>();
        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExpression)
        {
            IncludeExpressions.Add(IncludeExpression);
        }
        #endregion

        #region Sorinng
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity , object>> orderByExp)
        {
            OrderBy = orderByExp;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExp)
        {
            OrderByDescending = orderByDescExp;
        }
        #endregion

        #region Pagination
        public int Take { get; private set; }

        public int Skip {  get; private set; }

        public bool IsPaginated { get; set; }

        protected void ApplyPagination(int PageSize, int PageIndex)
        {
            Take = PageSize;
            Skip = PageSize * (PageIndex - 1);
            IsPaginated = true;
        }
        #endregion

    }
}
