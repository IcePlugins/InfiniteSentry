using Rocket.API;
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
            if (InfiniteSentry.Instance.Configuration.Instance.allSentriesInfinite)
            {
                UnturnedChat.Say(caller, InfiniteSentry.Instance.Translate("disabled"), Color.red);
                return;
            }

            Player player = ((UnturnedPlayer)caller).Player;
            PlayerLook look = player.look;

            if (PhysicsUtility.raycast(new Ray(look.aim.position, look.aim.forward), out RaycastHit hit, Mathf.Infinity, RayMasks.BARRICADE | RayMasks.STRUCTURE))
            {
                var sentry = hit.transform?.GetComponent<InteractableSentry>();

                if (sentry == null)
                {
                    UnturnedChat.Say(caller, InfiniteSentry.Instance.Translate("no_sentry"), Color.red);
                    return;
                }

                var component = sentry.gameObject.GetComponent<SentryTrackerComponent>();

                if (component == null)
                {
                    UnturnedChat.Say(caller, InfiniteSentry.Instance.Translate("added"));
                    sentry.gameObject.AddComponent<SentryTrackerComponent>();
                }
                else
                {
                    UnityEngine.Object.Destroy(component);
                    UnturnedChat.Say(caller, InfiniteSentry.Instance.Translate("destroyed"));
                }
            }
            else
                UnturnedChat.Say(caller, InfiniteSentry.Instance.Translate("no_sentry"), Color.red);
        }
    }
}