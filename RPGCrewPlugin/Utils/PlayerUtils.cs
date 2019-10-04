using Sandbox.Game.World;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Torch.Managers;

namespace RPGCrewPlugin.Utils
{
    class PlayerUtils
    {   private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly List<IMyPlayer> _playerCache = new List<IMyPlayer>();
        public static MyIdentity GetIdentityByNameOrId(string playerNameOrSteamId)
        {

            foreach (var identity in MySession.Static.Players.GetAllIdentities())
            {

                if (identity.DisplayName == playerNameOrSteamId)
                    return identity;

                if (ulong.TryParse(playerNameOrSteamId, out ulong steamId))
                {

                    ulong id = MySession.Static.Players.TryGetSteamId(identity.IdentityId);
                    if (id == steamId)
                        return identity;
                }
            }

            return null;
        }

        public static MyIdentity GetIdentityByName(string playerName)
        {

            foreach (var identity in MySession.Static.Players.GetAllIdentities())
                if (identity.DisplayName == playerName)
                    return identity;

            return null;
        }

        public static MyIdentity GetIdentityById(long playerId)
        {

            foreach (var identity in MySession.Static.Players.GetAllIdentities())
                if (identity.IdentityId == playerId)
                    return identity;

            return null;
        }
        public static MyIdentity GetMyPlayerBySteamId(ulong playerId)
        {
            long id = 0L;
            var lll = MySession.Static.Players;
            if (lll != null)
            {
                Log.Warn($"Найден id = {lll.TryGetIdentityId(playerId).ToString()}");
                id = lll.TryGetIdentityId(playerId);
            }
            
            if (id!=0L)
            {
                var Iden = lll.TryGetIdentity(id);

                if (Iden != null)
                {
                    Log.Warn($"Найден Identity  '{Iden.DisplayName}'");
                    return Iden;
                }
                else Log.Warn($"Identity не найден");
            }
            
        
          
            return null;
        }

        

        public static string GetPlayerNameById(long playerId)
        {

            MyIdentity identity = GetIdentityById(playerId);

            if (identity != null)
                return identity.DisplayName;

            return "Nobody";
        }
        public static bool TryGetEntityByNameOrId(string nameOrId, out IMyEntity entity)
        {
            if (long.TryParse(nameOrId, out long id))
                return MyAPIGateway.Entities.TryGetEntityById(id, out entity);

            foreach (var ent in MyEntities.GetEntities())
            {
                if (ent.DisplayName == nameOrId)
                {
                    entity = ent;
                    return true;
                }
            }

            entity = null;
            return false;
        }

        public static IMyPlayer GetPlayerByNameOrId(string nameOrPlayerId)
        {
            if (!long.TryParse(nameOrPlayerId, out long id))
            {
                foreach (var identity in MySession.Static.Players.GetAllIdentities())
                {
                    if (identity.DisplayName == nameOrPlayerId)
                    {
                        id = identity.IdentityId;
                    }
                }
            }

            MyPlayer.PlayerId playerId;
            if (MySession.Static.Players.TryGetPlayerId(id, out playerId))
            {
                if (MySession.Static.Players.TryGetPlayerById(playerId, out MyPlayer player))
                {
                    return player;
                }
            }

            return null;
        }
        public static IMyPlayer GetPlayerBySteamId(ulong steamId)
        {
            _playerCache.Clear();
            MyAPIGateway.Players.GetPlayers(_playerCache);
            return _playerCache.FirstOrDefault(p => p.SteamUserId == steamId);
        }

        public static IMyPlayer GetPlayerById(long identityId)
        {
            _playerCache.Clear();
            MyAPIGateway.Players.GetPlayers(_playerCache);
            return _playerCache.FirstOrDefault(p => p.IdentityId == identityId);
        }
    }
}
