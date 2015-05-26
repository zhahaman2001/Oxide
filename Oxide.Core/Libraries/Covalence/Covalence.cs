using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oxide.Core.Logging;

namespace Oxide.Core.Libraries.Covalence
{
    /// <summary>
    /// The Covalence library
    /// </summary>
    public class Covalence : Library
    {
        /// <summary>
        /// Returns if this library should be loaded into the global namespace
        /// </summary>
        public override bool IsGlobal { get { return false; } }

        // The provider
        private ICovalenceProvider provider;

        // The logger
        private Logger logger;

        /// <summary>
        /// Initialises a new instance of the Covalence class
        /// </summary>
        public Covalence()
        {
            // Get logger
            logger = Interface.GetMod().RootLogger;

            // Search for all provider types
            Type baseType = typeof(ICovalenceProvider);
            List<Type> candidates = new List<Type>(
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany((a) => a.GetTypes())
                    .Where((t) => t.IsClass && !t.IsAbstract && t.FindInterfaces((m, o) => m == baseType, null).Length == 1)
                );

            // Select a candidate
            Type selectedCandidate;
            if (candidates.Count == 0)
            {
                logger.Write(LogType.Warning, "No Covalence providers found, Covalence will not be functional for this session.");
                return;
            }
            else if (candidates.Count > 1)
            {
                selectedCandidate = candidates[0];
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < candidates.Count; i++)
                {
                    if (i > 1) sb.Append(',');
                    sb.Append(candidates[i].FullName);
                }
                logger.Write(LogType.Warning, "Multiple Covalence providers found! Using {0}. (Also found {1})");
            }
            else
                selectedCandidate = candidates[0];

            // Create it
            try
            {
                provider = Activator.CreateInstance(selectedCandidate) as ICovalenceProvider;
            }
            catch (Exception ex)
            {
                logger.Write(LogType.Warning, "Got exception when instantiating Covalence provider, Covalence will not be functional for this session.");
                logger.Write(LogType.Warning, "{0}", ex);
                return;
            }

            // Log
            logger.Write(LogType.Info, "Using Covalence provider for game '{0}'", provider.GameName);
        }

        /// <summary>
        /// Returns the name of the current game
        /// </summary>
        /// <returns></returns>
        [LibraryFunction("GetGame")]
        public string GetGame()
        {
            if (provider == null) return string.Empty;
            return provider.GameName;
        }

    }
}
