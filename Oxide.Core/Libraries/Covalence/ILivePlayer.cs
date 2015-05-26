using System;

namespace Oxide.Core.Libraries.Covalence
{
    /// <summary>
    /// Represents a generic connected player within a game
    /// </summary>
    public interface ILivePlayer
    {
        /// <summary>
        /// Gets the base player of this player
        /// </summary>
        IPlayer BasePlayer { get; }

        #region Administration

        /// <summary>
        /// Kicks this player from the game
        /// </summary>
        /// <param name="reason"></param>
        void Kick(string reason);

        #endregion
    }
}
