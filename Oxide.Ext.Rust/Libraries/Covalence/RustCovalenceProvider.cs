using System;

using Oxide.Core.Libraries.Covalence;

namespace Oxide.Rust.Libraries.Covalence
{
    /// <summary>
    /// Provides Covalence functionality for the game "Rust"
    /// </summary>
    public class RustCovalenceProvider : ICovalenceProvider
    {
        /// <summary>
        /// Gets the name of the game for which this provider provides
        /// </summary>
        public string GameName { get { return "Rust"; } }
    }
}
