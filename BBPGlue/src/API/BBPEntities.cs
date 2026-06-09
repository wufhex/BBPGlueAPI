using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Utilities for enumerating and querying entities (NPCs and players) in the world.
    /// </summary>
    public sealed class BBPEntities
    {
        /// <summary>
        /// Read-only list of NPC wrappers present in the environment.
        /// </summary>
        public IReadOnlyList<BBPNpc> Npcs => GetNpcs();
        /// <summary>
        /// Read-only list of player references present in the environment.
        /// </summary>
        public IReadOnlyList<BBPPlayerRef> Players => GetPlayers();

        /// <summary>
        /// The NPC closest to the player position.
        /// </summary>
        public BBPNpc? ClosestNpc =>
            GetClosestNpc(BBP.Player.Position);

        /// <summary>
        /// The player reference closest to the player position (useful in multiplayer scenarios).
        /// </summary>
        public BBPPlayerRef? ClosestPlayer =>
            GetClosestPlayer(BBP.Player.Position);

        /// <summary>
        /// Enumerates NPCs present in the environment and returns wrappers for them.
        /// </summary>
        /// <returns>List of BBPNpc wrappers.</returns>
        public List<BBPNpc> GetNpcs()
        {
            List<BBPNpc> result = new List<BBPNpc>();

            IList? rawNpcs = BBP.Environment.Npcs;
            if (rawNpcs == null)
                return result;

            foreach (object? npc in rawNpcs)
            {
                if (npc != null)
                    result.Add(new BBPNpc(npc));
            }

            return result;
        }

        /// <summary>
        /// Enumerates player references in the environment.
        /// </summary>
        /// <returns>List of BBPPlayerRef wrappers.</returns>
        public List<BBPPlayerRef> GetPlayers()
        {
            List<BBPPlayerRef> result = new List<BBPPlayerRef>();

            IList? rawPlayers = BBP.Environment.Players;
            if (rawPlayers == null)
                return result;

            foreach (object? player in rawPlayers)
            {
                if (player != null)
                    result.Add(new BBPPlayerRef(player));
            }

            return result;
        }

        /// <summary>
        /// Finds the NPC closest to the specified position.
        /// </summary>
        /// <param name="position">Position to measure distance from.</param>
        /// <returns>The closest BBPNpc or null if none found.</returns>
        public BBPNpc? GetClosestNpc(Vector3 position)
        {
            BBPNpc? closest = null;
            float bestDistance = float.MaxValue;

            foreach (BBPNpc npc in GetNpcs())
            {
                float distance = Vector3.Distance(position, npc.Position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = npc;
                }
            }

            return closest;
        }

        /// <summary>
        /// Finds the player reference closest to the specified position.
        /// </summary>
        /// <param name="position">Position to measure distance from.</param>
        /// <returns>The closest BBPPlayerRef or null if none found.</returns>
        public BBPPlayerRef? GetClosestPlayer(Vector3 position)
        {
            BBPPlayerRef? closest = null;
            float bestDistance = float.MaxValue;

            foreach (BBPPlayerRef player in GetPlayers())
            {
                float distance = Vector3.Distance(position, player.Position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = player;
                }
            }

            return closest;
        }
    }
}