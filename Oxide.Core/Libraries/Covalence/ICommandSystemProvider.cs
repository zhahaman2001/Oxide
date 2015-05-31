using System;

namespace Oxide.Core.Libraries.Covalence
{
    /// <summary>
    /// Represents a source of commands
    /// </summary>
    public enum CommandType { Chat, Console }


    public delegate bool CommandCallback(string cmd, CommandType type, IPlayer caller, string[] args);

    /// <summary>
    /// Represents a binding to a generic command system
    /// </summary>
    public interface ICommandSystemProvider
    {
        /// <summary>
        /// Sets the callback to use when a command is received
        /// </summary>
        /// <param name="callback"></param>
        void SetCallback(CommandCallback callback);

        /// <summary>
        /// Registers that the specified command should be routed to the callback
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        void RegisterInterest(string cmd, CommandType type);

        /// <summary>
        /// Registers that the specified command should no longer be routed to the callback
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        void DisregardInterest(string cmd, CommandType type);
    }
}
