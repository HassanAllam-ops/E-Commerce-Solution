using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        // Create Query
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> InputQuery,ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var query = InputQuery;

            if(specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            if(specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if(specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if(specification.IsPaginated)
            {
                query = query.Skip(specification.Skip);
                query = query.Take(specification.Take);
            }

            if(specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
            {
                //foreach(var expression in specification.IncludeExpressions)
                //{
                //    query = query.Include(expression);
                //}

                query = specification.IncludeExpressions.Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
            }

            return query;
        }
    }
}
