using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oxide.Core.Libraries.Covalence
{
    /// <summary>
    /// Specifies a provider for core game-specific covalence functionality
    /// </summary>
    public interface ICovalenceProvider
    {
        /// <summary>
        /// Gets the name of the game for which this provider provides
        /// </summary>
        string GameName { get; }

    }
}
