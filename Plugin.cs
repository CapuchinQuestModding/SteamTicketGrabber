using System;
using System.IO;
using HarmonyLib;
using UnityEngine;

namespace SteamTicketGrabber
{
    public class Plugin : MonoBehaviour
    {
        bool playerSpawned, done;

        Type Player = Type.GetType("Locomotion.Player"), LoginManager = Type.GetType("CapuchinPlayfab.LoginManager");

        private void Update()
        {
            if (!playerSpawned)
            {
                if (Traverse.Create(Player).Field("Instance")?.GetValue() != null)
                {
                    playerSpawned = true;
                }
            }

            if (playerSpawned && !done)
            {
                string ticket = Traverse.Create(LoginManager).Method("GetSteamAuthTicket")?.GetValue<string>();
                
                var path = Path.Combine(Traverse.Create(Type.GetType("BepInEx.Paths")).Field("BepInExRootPath").GetValue<string>(), "steam_ticket.txt");
                File.WriteAllText(path, ticket);
                
                done = true;
            }
        }
    }
}