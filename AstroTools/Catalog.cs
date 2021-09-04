using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroTools
{
    public static class Catalog
    {
        private static Dictionary<CatalogType, IEnumerable<string>> catalog;

        public static void Init()
        {
            catalog = new Dictionary<CatalogType, IEnumerable<string>>();
        }

        public static int Count(CatalogType cat)
        {
            if (catalog.ContainsKey(cat))
                return catalog[cat].Count();
            else
                return 0;
        }

        public static void Add(CatalogType cat, string id)
        {
            if (!catalog.ContainsKey(cat) && !string.IsNullOrEmpty(id))
            {
                catalog.Add(cat, new HashSet<string>());
            }

            if (!string.IsNullOrEmpty(id))
            {
                var tempList = catalog[cat].ToHashSet();
                tempList.Add(id);
                catalog[cat] = tempList;
            }
        }

        public static HashSet<string> Get(CatalogType cat)
        {
            if (!catalog.ContainsKey(cat)) return new HashSet<string>();

            return catalog[cat].ToHashSet<string>();
        }

        public static bool Contains(CatalogType cat, string id)
        {
            bool result = false;

            if (!catalog.ContainsKey(cat)) return false;

            result = catalog[cat].Contains(id);

            return result;
        }
    }
}
