﻿using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    public class CommandToggleInfinite : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "toggleinfinite";

        public string Help => "Toggles an infinite sentry on or off.";

        public string Syntax => "/toggleinfinite";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "infinitesentry.toggle" };
        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(ulong.Parse(caller.Id));
            if (InfiniteSentry.Instance.Configuration.Instance.allSentriesInfinite)
            {
                InfiniteSentry.Instance.TellPlayer(steamPlayer, Color.red, "disabled");
                return;
            }

            Player player = ((UnturnedPlayer)caller).Player;
            PlayerLook look = player.look;

            if (PhysicsUtility.raycast(new Ray(look.aim.position, look.aim.forward), out RaycastHit hit, Mathf.Infinity, RayMasks.BARRICADE | RayMasks.STRUCTURE))
            {
                // https://github.com/JetBrains/resharper-unity/wiki/Possible-unintended-bypass-of-lifetime-check-of-underlying-Unity-engine-object
                if (hit.transform == null)
                {
                    InfiniteSentry.Instance.TellPlayer(steamPlayer, Color.red, "no_sentry");
                    return;
                }
            
                var sentry = hit.transform.GetComponent<InteractableSentry>();
                if (sentry == null)
                {
                    InfiniteSentry.Instance.TellPlayer(steamPlayer, Color.red, "no_sentry");
                    return;
                }

                var component = sentry.gameObject.GetComponent<SentryTrackerComponent>();
                if (component == null)
                {
                    InfiniteSentry.Instance.TellPlayer(steamPlayer, Palette.SERVER, "added");
                    sentry.gameObject.AddComponent<SentryTrackerComponent>();
                }
                else
                {
                    UnityEngine.Object.Destroy(component);
                    InfiniteSentry.Instance.TellPlayer(steamPlayer, Palette.SERVER, "destroyed");
                }
            }
            else
                InfiniteSentry.Instance.TellPlayer(steamPlayer, Color.red, "no_sentry");
        }
    }
}