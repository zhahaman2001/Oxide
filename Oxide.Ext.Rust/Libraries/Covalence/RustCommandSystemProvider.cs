using System;

using Oxide.Core.Libraries.Covalence;

namespace Oxide.Rust.Libraries.Covalence
{
    /// <summary>
    /// Represents a binding to a generic command system
    /// </summary>
    public class RustCommandSystemProvider : ICommandSystemProvider
    {
        /// <summary>
        /// Sets the callback to use when a command is received
        /// </summary>
        /// <param name="callback"></param>
        public void SetCallback(CommandCallback callback)
        {

        }

        /// <summary>
        /// Registers that the specified command should be routed to the callback
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public void RegisterInterest(string cmd, CommandType type)
        {

        }

        /// <summary>
        /// Registers that the specified command should no longer be routed to the callback
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public void DisregardInterest(string cmd, CommandType type)
        {

        }
    }
}
