using System;
using System.Collections.Generic;
using System.Reflection;

using Oxide.Core.Libraries.Covalence;

namespace Oxide.Game.Rust.Libraries.Covalence
{
    /// <summary>
    /// Represents a binding to a generic command system
    /// </summary>
    public class RustCommandSystem : ICommandSystem
    {
        // A reference to Rust's internal command dictionary
        private IDictionary<string, ConsoleSystem.Command> rustcommands;

        // Chat command handler
        private ChatCommandHandler chatCommandHandler;

        // All registered chat commands
        private IDictionary<string, CommandCallback> registeredChatCommands;

        /// <summary>
        /// Initialises the command system provider
        /// </summary>
        private void Initialise()
        {
            rustcommands = typeof(ConsoleSystem.Index).GetField("dictionary", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as IDictionary<string, ConsoleSystem.Command>;
            registeredChatCommands = new Dictionary<string, CommandCallback>();
            chatCommandHandler = new ChatCommandHandler(ChatCommandCallback, registeredChatCommands.ContainsKey);
        }

        private bool ChatCommandCallback(string cmd, CommandType type, IPlayer caller, string[] args)
        {
            CommandCallback callback;
            if (!registeredChatCommands.TryGetValue(cmd, out callback))
                return false;
            else
                return callback(cmd, type, caller, args);
        }

        /// <summary>
        /// Registers the specified command
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void RegisterCommand(string cmd, CommandType type, CommandCallback callback)
        {
            // Initialise if needed
            if (rustcommands == null) Initialise();

            // Is it a console command?
            if (type == CommandType.Console)
            {
                // TODO: Add console command to rustcommands
                /*rustcommands.Add(cmd, new ConsoleSystem.Command()
                {

                });*/
            }
            else if (type == CommandType.Chat)
            {
                registeredChatCommands.Add(cmd, callback);
            }
        }

        /// <summary>
        /// Unregisters the specified command
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        /// <param name="callback"></param>
        public void UnregisterCommand(string cmd, CommandType type)
        {
            // Initialise if needed
            if (rustcommands == null) Initialise();

            // Is it a console command?
            if (type == CommandType.Console)
            {
                // TODO: Remove console command from rustcommands (check if we "own" it!)
            }
            else if (type == CommandType.Chat)
            {
                registeredChatCommands.Remove(cmd);
            }
        }

        /// <summary>
        /// Handles a chat message
        /// </summary>
        /// <param name="player"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool HandleChatMessage(ILivePlayer player, string str)
        {
            return chatCommandHandler.HandleChatMessage(player, str);
        }
    }
}
