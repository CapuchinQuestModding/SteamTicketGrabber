using System;
using System.IO;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MelonLoader;
using SteamTicketGrabber;
using UnityEngine;

[assembly: MelonInfo(typeof(MelonLoaderInit), ModInfo.Name, ModInfo.Version, ModInfo.Author)]
[assembly: MelonGame("Duttbust", "Capuchin")]

namespace SteamTicketGrabber
{
    public class ModInfo
    {
        public const string UUID = "monky.steamticketgrabber";
        public const string Name = "SteamTicketGrabber";
        public const string Author = "Monky";
        public const string Version = "1.0.0";
    }

    public enum ModLoader { BepInEx, MelonLoader }

    [BepInPlugin(ModInfo.UUID, ModInfo.Name, ModInfo.Version)]
    public class BepInExInit : BasePlugin
    {
        public override void Load()
        {
            AddComponent<Plugin>();
        }

        public override bool Unload()
        {
            return true;
        }
    }

    public class MelonLoaderInit : MelonMod
    {
        bool done;

        Type Player = Type.GetType("Il2CppLocomotion.Player"), LoginManager = Type.GetType("Il2CppCapuchinPlayfab.LoginManager");
        public override void OnUpdate()
        {
            if (!done && Traverse.Create(Player).Field("Instance")?.GetValue() != null)
            {
                string ticket = Traverse.Create(LoginManager).Method("GetSteamAuthTicket")?.GetValue<string>();
                
                var path = Path.Combine(Traverse.Create(Type.GetType("MelonLoader.Utils.MelonEnvironment")).Field("UserDataDirectory").GetValue<string>(), "steam_ticket.txt");
                File.WriteAllText(path, ticket);
                
                done = true;
            }
        }
    }
}
