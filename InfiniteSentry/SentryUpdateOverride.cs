using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    internal static class SentryUpdateOverride
    {
        public const int AMMO_INDEX = 10;

        internal static void Prefix(ref byte __state, InteractableSentry __instance)
        {
            SentryTrackerComponent s = __instance.gameObject.GetComponent<SentryTrackerComponent>();

            if (s == null && !InfiniteSentry.Instance.Configuration.Instance.allSentriesInfinite)
                return;

            Item disp = __instance.displayItem;
            bool wep = (bool)typeof(InteractableSentry).GetField("hasWeapon", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

            if (disp != null && wep)
                __state = disp.state[AMMO_INDEX];
        }

        internal static void Postfix(byte __state, InteractableSentry __instance)
        {
            if (__state != default(byte))
                __instance.displayItem.state[AMMO_INDEX] = __state;
        }
    }
}
