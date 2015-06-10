using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oxide.Core.Logging;
using Oxide.Core.Plugins;

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

        /// <summary>
        /// Gets the server mediator
        /// </summary>
        [LibraryProperty("Server")]
        public IServer Server { get; private set; }

        /// <summary>
        /// Gets the player manager mediator
        /// </summary>
        [LibraryProperty("Players")]
        public IPlayerManager Players { get; private set; }

        // The provider
        private ICovalenceProvider provider;

        // The command system provider
        private ICommandSystemProvider cmdSystem;

        // Registered commands
        private class RegisteredCommand
        {
            /// <summary>
            /// The plugin that handles the command
            /// </summary>
            public readonly Plugin Source;

            /// <summary>
            /// The name of the command
            /// </summary>
            public readonly string Command;

            /// <summary>
            /// The name of the callback
            /// </summary>
            public readonly string Callback;

            /// <summary>
            /// Initialises a new instance of the RegisteredCommand class
            /// </summary>
            /// <param name="source"></param>
            /// <param name="command"></param>
            /// <param name="callback"></param>
            public RegisteredCommand(Plugin source, string command, string callback)
            {
                // Store fields
                Source = source;
                Command = command;
                Callback = callback;
            }
        }
        private IDictionary<string, RegisteredCommand> commands;

        // The logger
        private Logger logger;

        /// <summary>
        /// Initialises a new instance of the Covalence class
        /// </summary>
        public Covalence()
        {
            // Get logger
            logger = Interface.GetMod().RootLogger;
        }

        /// <summary>
        /// Initialises the Covalence library
        /// </summary>
        internal void Initialise()
        {
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

            // Create mediators
            Server = provider.CreateServer();
            Players = provider.CreatePlayerManager();
            //cmdSystem = provider.CreateCommandSystemProvider();
            //cmdSystem.SetCallback(CommandCallback);

            // Initialise other things
            commands = new Dictionary<string, RegisteredCommand>();

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

        /// <summary>
        /// Processes an incoming command
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        /// <param name="caller"></param>
        /// <param name="args"></param>
        private bool CommandCallback(string cmd, CommandType type, IPlayer caller, string[] args)
        {
            // Lookup the command
            RegisteredCommand regCmd;
            if (!commands.TryGetValue(cmd, out regCmd))
            {
                // Don't route this here anymore
                cmdSystem.DisregardInterest(cmd, type);
                return false;
            }

            // Handle it
            // TODO: Handle errors?
            regCmd.Source.Call(regCmd.Callback, caller, args);

            // Success
            return true;
        }

        /// <summary>
        /// Registers a command (chat + console)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="plugin"></param>
        /// <param name="callback"></param>
        [LibraryFunction("RegisterCommand")]
        public void RegisterCommand(string cmd, Plugin plugin, string callback)
        {
            
        }

    }
}
