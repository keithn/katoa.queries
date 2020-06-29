using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Katoa.Queries
{
    public class Query
    {
        public static string QueryFolder = "Queries";
        public static HashSet<string> AssembliesLoaded = new HashSet<string>();
        private static Dictionary<string, string> Queries { get; set; } = new Dictionary<string, string>();

        public static bool IsLoaded(Assembly assembly) => AssembliesLoaded.Contains(assembly.GetName().Name);

        private static void LoadEmbeddedQueries(Assembly assembly)
        {
            if (IsLoaded(assembly)) return;
            lock (Queries)
            {
                // recheck inside the lock, this way, normally we avoid the lock, but in the initial case we will avoid race conditions
                if (IsLoaded(assembly)) return;
                var resources = assembly.GetManifestResourceNames();
                var queryNamespacePrefix = $"{assembly.GetName().Name}.{QueryFolder}.";
                var queries = resources.Where(q => q.StartsWith(queryNamespacePrefix)).ToList();
                queries.ForEach(q =>
                {
                    using (var stream = assembly.GetManifestResourceStream(q))
                    {
                        if (stream == null) return;
                        using (var reader = new StreamReader(stream))
                        {
                            var name = q.Replace(queryNamespacePrefix, "").Replace(".sql", "");
                            if (!Queries.ContainsKey(name))
                            {
                                Queries.Add(name, FilterForQuery(reader.ReadToEnd()));
                            }
                        }
                    }
                });
                AssembliesLoaded.Add(assembly.GetName().Name);
            }
        }

        // Allow SQL files to have a option header section where test parameters can be declared for the query parameters
        // The Real Query must start after the SQL comment  -- QUERY
        private static string FilterForQuery(string sql)
        {
            var result = "";
            var lines = sql.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var l in lines)
            {
                result += l + "\r\n";
                if (l.Replace(" ", "").Contains("--QUERY"))
                {
                    result = "";
                }
            }

            return result;
        }

        public static string For(string query)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            LoadEmbeddedQueries(callingAssembly);
            if (!Queries.ContainsKey(query))
            {
                throw new Exception(
                    $"Query named {query} not found in assembly {callingAssembly.GetName().Name} in Folder {QueryFolder}.  Most likely causes are the name has a typo or the query has not been marked as an embedded resource");
            }

            return Queries[query];
        }
    }
}