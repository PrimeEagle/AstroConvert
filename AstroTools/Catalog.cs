using System;
using System.Collections.Generic;
using System.Linq;

namespace AstroTools
{
    public static class Catalog
    {
        private static Dictionary<CatalogType, IEnumerable<string>> _catalog;

        public static void Init()
        {
            _catalog = new Dictionary<CatalogType, IEnumerable<string>>();
        }

        public static int Count(CatalogType cat)
        {
            return _catalog.ContainsKey(cat) ? _catalog[cat].Count() : 0;
        }

        public static void Add(CatalogType cat, string id)
        {
            if (!_catalog.ContainsKey(cat) && !string.IsNullOrEmpty(id))
            {
                _catalog.Add(cat, new HashSet<string>());
            }

            if (string.IsNullOrEmpty(id)) return;

            var tempList = _catalog[cat].ToHashSet();
            tempList.Add(id);
            _catalog[cat] = tempList;
        }

        public static HashSet<string> Get(CatalogType cat)
        {
            return !_catalog.ContainsKey(cat) ? new HashSet<string>() : _catalog[cat].ToHashSet<string>();
        }

        public static bool Contains(CatalogType cat, string id)
        {
            if (!_catalog.ContainsKey(cat)) return false;

            var result = _catalog[cat].Contains(id);
            return result;
        }
    }
}