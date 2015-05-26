using System;
using System.Net;

using Oxide.Core.Libraries.Covalence;

namespace Oxide.Rust.Libraries.Covalence
{
    /// <summary>
    /// Represents the Rust server hosting the game instance
    /// </summary>
    public class RustServer : IServer
    {
        /// <summary>
        /// Gets the public-facing name of the server
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the public-facing IP address of the server, if known
        /// </summary>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// Gets the public-facing network port of the server, if known
        /// </summary>
        public ushort Port { get; private set; }

        private ServerMgr mgr;

        /// <summary>
        /// Initialises a new instance of the RustServer class
        /// </summary>
        public RustServer()
        {
            mgr = ServerMgr.Instance;
            
        }

        #region Console

        /// <summary>
        /// Prints the specified message to the server console
        /// </summary>
        /// <param name="message"></param>
        public void PrintToConsole(string message)
        {

        }

        /// <summary>
        /// Runs the specified server command
        /// </summary>
        /// <param name="cmd"></param>
        public void RunServerCommand(string cmd)
        {
            
        }

        #endregion
    }
}
