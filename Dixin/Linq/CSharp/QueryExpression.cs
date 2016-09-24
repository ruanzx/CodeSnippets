﻿namespace Dixin.Linq.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dixin.Linq.EntityFramework;

#if DEMO
    internal static partial class Int32Extensions
    {
        internal static TResult Select<TResult>(this int int32, Func<int, TResult> selector) => selector(int32);
    }

    internal static partial class QueryExpression
    {
        internal static void SelectInt32()
        {
            int mapped1 = from zero in default(int) // 0
                          select zero; // 0

            double mapped2 = from three in 1 + 2 // 3
                             select Math.Sqrt(three + 1); // 2
        }
    }

    internal static partial class QueryExpression
    {
        internal static void CompiledSelectInt32()
        {
            int mapped1 = default(int).Select(zero => zero); // 0
            double mapped2 = (1 + 2).Select(three => Math.Sqrt(three + 1)); // 2
        }
    }

    internal static partial class ObjectExtensions
    {
        internal static TResult Select<TSource, TResult>(this TSource value, Func<TSource, TResult> selector) => selector(value);
    }

    internal static partial class QueryExpression
    {
        internal static void SelectGuid()
        {
            string mapped = from newGuid in Guid.NewGuid()
                            select newGuid.ToString();
        }

        internal static void CompiledSelectGuid()
        {
            string mapped = Guid.NewGuid().Select(newGuid => newGuid.ToString());
        }
    }
#endif

    internal static partial class QueryExpression
    {
        internal static void LocalQuery(ILocal<Uri> uris)
        {
            ILocal<ILocalGroup<string, Uri>> query =
                from uri in uris
                where uri.IsAbsoluteUri // ILocal.Where and anonymous method.
                group uri by uri.Host into hostUris // ILocal.GroupBy and anonymous method.
                let host = hostUris.Key // ILocal.Select and anonymous method.
                orderby host // ILocal.OrderBy and anonymous method.
                select hostUris; // ILocal.Select and anonymous method.
        }

        internal static void RemoteQuery(IRemote<Uri> uris)
        {
            IRemote<IRemoteGroup<string, Uri>> query =
                from uri in uris
                where uri.IsAbsoluteUri // IRemote.Where and expression tree.
                group uri by uri.Host into hostUris // IRemote.GroupBy and expression tree.
                let host = hostUris.Key // IRemote.Select and expression tree.
                orderby host // IRemote.OrderBy and expression tree.
                select hostUris; // IRemote.Select and expression tree.
        }
    }

    internal static partial class QueryExpression
    {
        internal static void CompiledLocalQuery(ILocal<Uri> uris)
        {
            ILocal<ILocalGroup<string, Uri>> query = uris
                .Where(uri => uri.IsAbsoluteUri) // ILocal.Where and anonymous method.
                .GroupBy(uri => uri.Host) // ILocal.GroupBy and anonymous method.
                .Select(hostUris => new { hostUris, host = hostUris.Key }) // ILocal.Select and anonymous method.
                .OrderBy(hostUris => hostUris.host) // ILocal.OrderBy and anonymous method.
                .Select(hostUris => hostUris.hostUris); // ILocal.Select and anonymous method.
        }

        internal static void CompiledRemoteQuery(IRemote<Uri> uris)
        {
            IRemote<IRemoteGroup<string, Uri>> query = uris
                .Where(uri => uri.IsAbsoluteUri) // IRemote.Where and expression tree.
                .GroupBy(uri => uri.Host) // IRemote.GroupBy and expression tree.
                .Select(hostUris => new { hostUris, host = hostUris.Key }) // IRemote.Select and expression tree.
                .OrderBy(hostUris => hostUris.host) // IRemote.OrderBy and expression tree.
                .Select(hostUris => hostUris.hostUris); // IRemote.Select and expression tree.
        }
    }

    internal static partial class QueryExpression
    {
        internal static void LinqToObjects(IEnumerable<Type> types)
        {
            IEnumerable<IGrouping<string, Type>> query =
                from type in types
                where type.BaseType == typeof(MulticastDelegate) // Enumerbale.Where and anonymous method.
                group type by type.Namespace into namespaceTypes // Enumerbale.GroupBy and anonymous method.
                let @namespace = namespaceTypes.Key // Enumerbale.Select and anonymous method.
                orderby namespaceTypes.Count() descending, @namespace // Enumerbale.OrderByDescending, Enumerbale.ThenBy and anonymous method.
                select namespaceTypes; // Enumerbale.Select and anonymous method.
        }

        internal static void ParallelLinq(ParallelQuery<Type> types)
        {
            ParallelQuery<IGrouping<string, Type>> query =
                from type in types
                where type.BaseType == typeof(MulticastDelegate) // ParallelEnumerbale.Where and anonymous method.
                group type by type.Namespace into namespaceTypes // ParallelEnumerbale.GroupBy and anonymous method.
                let @namespace = namespaceTypes.Key // ParallelEnumerbale.Select and anonymous method.
                orderby namespaceTypes.Count() descending, @namespace // ParallelEnumerbale.OrderByDescending, ParallelEnumerbale.ThenBy and anonymous method.
                select namespaceTypes; // ParallelEnumerbale.Select and anonymous method.
        }

        internal static void LinqToEntities(IQueryable<Product> products)
        {
            IQueryable<IGrouping<int?, Product>> query =
                from product in products
                where product.ListPrice > 0 // Queryable.Where and expression tree.
                group product by product.ProductSubcategoryID into subcategoryProducts // Queryable.GroupBy and expression tree.
                let subcategory = subcategoryProducts.Key // Queryable.Select and expression tree.
                orderby subcategoryProducts.Count() descending, subcategory // Queryable.OrderByDescending, Queryable.ThenBy and expression tree.
                select subcategoryProducts; // Queryable.Select and expression tree.
        }
    }

    internal static partial class QueryExpression
    {
        internal static void CompiledLinqToObjects(IEnumerable<Type> types)
        {
            IEnumerable<IGrouping<string, Type>> query = types
                .Where(type => type.BaseType == typeof(MulticastDelegate)) // Enumerbale.Where and anonymous method.
                .GroupBy(type => type.Namespace) // Enumerbale.GroupBy and anonymous method.
                .Select(namespaceTypes => new { namespaceTypes, @namespace = namespaceTypes.Key }) // Enumerbale.Select and anonymous method.
                .OrderByDescending(namespaceTypes => namespaceTypes.namespaceTypes.Count()) // Enumerbale.OrderByDescending and anonymous method.
                .ThenBy(namespaceTypes => namespaceTypes.@namespace) // Enumerbale.ThenBy and anonymous method.
                .Select(namespaceTypes => namespaceTypes.namespaceTypes); // Enumerbale.Select and anonymous method.
        }

        internal static void CompiledParallelLinq(ParallelQuery<Type> types)
        {
            ParallelQuery<IGrouping<string, Type>> query = types
                .Where(type => type.BaseType == typeof(MulticastDelegate)) // ParallelEnumerbale.Where and anonymous method.
                .GroupBy(type => type.Namespace) // ParallelEnumerbale.GroupBy and anonymous method.
                .Select(namespaceTypes => new { namespaceTypes, @namespace = namespaceTypes.Key }) // ParallelEnumerbale.Select and anonymous method.
                .OrderByDescending(@t => @t.namespaceTypes.Count()) // ParallelEnumerbale.OrderByDescending and anonymous method.
                .ThenBy(namespaceTypes => namespaceTypes.@namespace) // ParallelEnumerbale.ThenBy and anonymous method.
                .Select(namespaceTypes => namespaceTypes.namespaceTypes); // ParallelEnumerbale.Select and anonymous method.
        }

        internal static void CompiledLinqToEntities(IQueryable<Product> products)
        {
            IQueryable<IGrouping<int?, Product>> query = products
                .Where(product => product.ListPrice > 0) // Queryable.Where and expression tree.
                .GroupBy(product => product.ProductSubcategoryID) // Queryable.GroupBy and expression tree.
                .Select(subcategoryProducts => new { subcategoryProducts, subcategory = subcategoryProducts.Key }) // Queryable.Select and expression tree.
                .OrderByDescending(subcategoryProducts => subcategoryProducts.subcategoryProducts.Count()) // Queryable.OrderByDescending and expression tree.
                .ThenBy(subcategoryProducts => subcategoryProducts.subcategory) // Queryable.ThenBy and expression tree.
                .Select(subcategoryProducts => subcategoryProducts.subcategoryProducts); // Queryable.Select and expression tree.
        }

        internal static void QueryExpressionAndMethod(IEnumerable<Product> products)
        {
            IEnumerable<Product> query =
                (from product in products
                 where product.ListPrice > 0
                 select product)
                .Skip(20)
                .Take(10);
        }

        internal static void QueryMethod(IEnumerable<Product> products)
        {
            IEnumerable<Product> query = products
                .Where(product => product.ListPrice > 0)
                .Skip(20)
                .Take(10);
        }
    }
}

#if DEMO
namespace System.Linq
{
    using System.Collections.Generic;

    public static class Enumerable
	{
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate);
	}
}

namespace System.Linq
{
    using System.Collections.Generic;

    public static class Enumerable
    {
        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count);

        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count);
    }
}
#endif