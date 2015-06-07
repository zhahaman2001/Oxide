using System;
using System.Collections.Generic;
using System.Reflection;

using Oxide.Core.Libraries.Covalence;

namespace Oxide.Game.Rust.Libraries.Covalence
{
    /// <summary>
    /// Represents a binding to a generic command system
    /// </summary>
    public class RustCommandSystemProvider : ICommandSystemProvider
    {
        // The Covalence command callback
        private CommandCallback callback;

        // A reference to Rust's internal command dictionary
        private IDictionary<string, ConsoleSystem.Command> rustcommands;

        // Chat command handler
        private ChatCommandHandler chatCommandHandler;

        // All registered chat commands
        private HashSet<string> registeredChatCommands;

        /// <summary>
        /// Initialises the command system provider
        /// </summary>
        private void Initialise()
        {
            rustcommands = typeof(ConsoleSystem.Index).GetField("dictionary", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as IDictionary<string, ConsoleSystem.Command>;
            registeredChatCommands = new HashSet<string>();
        }

        /// <summary>
        /// Sets the callback to use when a command is received
        /// </summary>
        /// <param name="callback"></param>
        public void SetCallback(CommandCallback callback)
        {
            this.callback = callback;
            chatCommandHandler = new ChatCommandHandler(callback, registeredChatCommands.Contains);
        }

        /// <summary>
        /// Registers that the specified command should be routed to the callback
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public void RegisterInterest(string cmd, CommandType type)
        {
            // Initialise if needed
            if (rustcommands == null) Initialise();

            // Is it a console command?
            if (type == CommandType.Console)
            {
                // TODO: Add console command to rustcommands
            }
            else if (type == CommandType.Chat)
            {
                registeredChatCommands.Add(cmd);
            }
        }

        /// <summary>
        /// Registers that the specified command should no longer be routed to the callback
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public void DisregardInterest(string cmd, CommandType type)
        {
            // Initialise if needed
            if (rustcommands == null) Initialise();

            // Is it a console command?
            if (type == CommandType.Console)
            {
                // TODO: Remove console command to rustcommands (check if we "own" it!)
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
