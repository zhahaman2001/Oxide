using System;
using System.Collections.Generic;
using System.Linq;

using Oxide.Core;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Rust.Libraries.Covalence
{
    /// <summary>
    /// Represents a generic player manager
    /// </summary>
    public class RustPlayerManager : IPlayerManager
    {
        private struct PlayerRecord
        {
            public string Nickname;
            public ulong SteamID;
        }

        private IDictionary<string, PlayerRecord> playerData;

        private IDictionary<string, RustPlayer> players;
        private IDictionary<string, RustLivePlayer> livePlayers;

        internal void Initialise()
        {
            // Load player data
            playerData = Interface.GetMod().DataFileSystem.ReadObject<Dictionary<string, PlayerRecord>>("oxide.covalence.playerdata");
            players = new Dictionary<string, RustPlayer>();
            foreach (var pair in playerData)
            {
                players.Add(pair.Key, new RustPlayer(pair.Value.SteamID, pair.Value.Nickname));
            }
            livePlayers = new Dictionary<string, RustLivePlayer>();
        }

        private void NotifyPlayerJoin(ulong steamid, string nickname)
        {
            string uniqueID = steamid.ToString();

            // Do they exist?
            PlayerRecord record;
            if (playerData.TryGetValue(uniqueID, out record))
            {
                // Update
                record.Nickname = nickname;
                playerData[uniqueID] = record;

                // Swap out rust player
                players.Remove(uniqueID);
                players.Add(uniqueID, new RustPlayer(steamid, nickname));
            }
            else
            {
                // Insert
                record = new PlayerRecord();
                record.SteamID = steamid;
                record.Nickname = nickname;
                playerData.Add(uniqueID, record);

                // Create rust player
                players.Add(uniqueID, new RustPlayer(steamid, nickname));
            }

            // Save
            Interface.GetMod().DataFileSystem.WriteObject("oxide.covalence.playerdata", playerData);
        }

        internal void NotifyPlayerConnect(BasePlayer ply)
        {
            NotifyPlayerJoin(ply.userID, ply.net.connection.username);
            livePlayers[ply.userID.ToString()] = new RustLivePlayer(ply);
        }

        internal void NotifyPlayerDisconnect(BasePlayer ply)
        {
            livePlayers.Remove(ply.userID.ToString());
        }

        #region Offline Players

        /// <summary>
        /// Gets an offline player given their unique ID
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <returns></returns>
        public IPlayer GetOfflinePlayer(string uniqueID)
        {
            RustPlayer player;
            if (players.TryGetValue(uniqueID, out player))
                return player;
            else
                return null;
        }

        /// <summary>
        /// Gets all offline players
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPlayer> GetAllOfflinePlayers()
        {
            return players.Values.Cast<IPlayer>();
        }

        /// <summary>
        /// Finds a single offline player given a partial name (multiple matches returns null)
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        public IPlayer FindOfflinePlayer(string partialName)
        {
            // Pull 1 item and ONLY 1 item
            // TODO: If there's an exact match, just return that regardless?
            // That, or sort the sequence by how close the match is and return the best item
            try
            {
                return FindOfflinePlayers(partialName).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds any number of offline players given a partial name
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        public IEnumerable<IPlayer> FindOfflinePlayers(string partialName)
        {
            return players.Values
                .Where((p) => p.Nickname.Contains(partialName))
                .Cast<IPlayer>();
        }

        #endregion

        #region Online Players

        /// <summary>
        /// Gets an online player given their unique ID
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <returns></returns>
        public ILivePlayer GetPlayer(string uniqueID)
        {
            RustLivePlayer player;
            if (livePlayers.TryGetValue(uniqueID, out player))
                return player;
            else
                return null;
        }

        /// <summary>
        /// Gets all online players
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ILivePlayer> GetAllPlayers()
        {
            return livePlayers.Values.Cast<ILivePlayer>();
        }

        /// <summary>
        /// Finds a single online player given a partial name (wildcards accepted, multiple matches returns null)
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        public ILivePlayer FindPlayer(string partialName)
        {
            // Pull 1 item and ONLY 1 item
            // TODO: If there's an exact match, just return that regardless?
            // That, or sort the sequence by how close the match is and return the best item
            try
            {
                return FindPlayers(partialName).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds any number of online players given a partial name (wildcards accepted)
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        public IEnumerable<ILivePlayer> FindPlayers(string partialName)
        {
            return livePlayers.Values
                .Where((p) => p.BasePlayer.Nickname.Contains(partialName))
                .Cast<ILivePlayer>();
        }

        #endregion

    }
}
