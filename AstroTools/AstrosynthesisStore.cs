using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstroTools.Formats;
using ConcurrentCollections;

namespace AstroTools
{
    public class AstrosynthesisStore : IEnumerable<AstrosynthesisCsv>
    {
        //private readonly HashSet<AstrosynthesisCsv> _store = new HashSet<AstrosynthesisCsv>();
        private readonly ConcurrentHashSet<AstrosynthesisCsv> _store = new ConcurrentHashSet<AstrosynthesisCsv>();
        public int Count => _store.Count();

        private static bool Exists(AstrosynthesisCsv item)
        {
            bool exists;
            var existsList = new List<bool>();

            if (!string.IsNullOrEmpty(item.BdId)) existsList.Add(Catalog.Contains(CatalogType.BD, item.BdId));
            if (!string.IsNullOrEmpty(item.CodId)) existsList.Add(Catalog.Contains(CatalogType.COD, item.CodId));
            if (!string.IsNullOrEmpty(item.GlieseId)) existsList.Add(Catalog.Contains(CatalogType.CPD, item.GlieseId));
            if (!string.IsNullOrEmpty(item.CpdId)) existsList.Add(Catalog.Contains(CatalogType.Gliese, item.CpdId));
            if (!string.IsNullOrEmpty(item.HenryDraperId)) existsList.Add(Catalog.Contains(CatalogType.HD, item.HenryDraperId));
            if (!string.IsNullOrEmpty(item.HipparcosId)) existsList.Add(Catalog.Contains(CatalogType.HIP, item.HipparcosId));
            if (!string.IsNullOrEmpty(item.HarvardRevisedId)) existsList.Add(Catalog.Contains(CatalogType.HR, item.HarvardRevisedId));
            if (!string.IsNullOrEmpty(item.Name)) existsList.Add(Catalog.Contains(CatalogType.Name, item.Name));

            if (existsList.Count == 0)
            {
                exists = false;
            }
            else
            {
                exists = existsList.Count(e => e) == existsList.Count();
            }

            return exists;
        }

        public bool Add(AstrosynthesisCsv item, bool? alternateConditionToAddItem = null)
        {
            bool add;

            if (alternateConditionToAddItem.HasValue) add = !Exists(item) || alternateConditionToAddItem.Value;
            else add = !Exists(item);

            if (!add) return false;
            _store.Add(item);

            Catalog.Add(CatalogType.HD, item.HenryDraperId);
            Catalog.Add(CatalogType.HR, item.HarvardRevisedId);
            Catalog.Add(CatalogType.HIP, item.HipparcosId);
            Catalog.Add(CatalogType.COD, item.CodId);
            Catalog.Add(CatalogType.BD, item.BdId);
            Catalog.Add(CatalogType.CPD, item.CpdId);
            Catalog.Add(CatalogType.Gliese, item.GlieseId);
            Catalog.Add(CatalogType.Name, item.Name);

            return add;
        }

        public void RemoveAll(Predicate<AstrosynthesisCsv> match)
        {
            var toRemove = _store.Where(i => match(i));
            
            Parallel.ForEach(toRemove, item => 
            {
                _store.TryRemove(item);
            });
        }

        public IEnumerable<AstrosynthesisCsv> ToList()
        {
            return _store;
        }

        public IEnumerator<AstrosynthesisCsv> GetEnumerator()
        {
            return ((IEnumerable<AstrosynthesisCsv>)_store).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_store).GetEnumerator();
        }
    }
}
