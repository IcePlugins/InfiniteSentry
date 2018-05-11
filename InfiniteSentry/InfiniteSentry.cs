using Harmony;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Rocket.API.Collections;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    public class InfiniteSentry : RocketPlugin<InfiniteSentryConfiguration>
    {
        public static InfiniteSentry Instance { get; private set; }
        public bool ShuttingDown { get; private set; }

        protected override void Load()
        {
            Instance = this;
            ShuttingDown = false;
            Provider.onServerShutdown += OnShutdown;
            Level.onPostLevelLoaded += OnLevelLoaded;

            HarmonyInstance harmony = HarmonyInstance.Create("pw.cirno.extraconcentratedjuice");

            var orig = typeof(InteractableSentry).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
            var pre = typeof(SentryUpdateOverride).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic);
            var post = typeof(SentryUpdateOverride).GetMethod("Postfix", BindingFlags.Static | BindingFlags.NonPublic);

            harmony.Patch(orig, new HarmonyMethod(pre), new HarmonyMethod(post));
        }

        protected override void Unload()
        {
            Provider.onServerShutdown -= OnShutdown;
            Level.onPostLevelLoaded -= OnLevelLoaded;
        }

        private void OnShutdown() => ShuttingDown = true;
        private void OnLevelLoaded(int l) => ApplyComponents();

        private void ApplyComponents()
        {
            if (Configuration.Instance.allSentriesInfinite)
                return;

            foreach (BarricadeRegion r in BarricadeManager.regions)
                for (int i = r.drops.Count - 1; i >= 0; i--)
                {
                    BarricadeDrop drop = r.drops[i];

                    InteractableSentry s = drop.interactable as InteractableSentry;

                    if (s != null)
                    {
                        foreach (SentryPosition pos in Configuration.Instance.sentries)
                            if (pos.Equals(s.transform.position))
                                s.gameObject.AddComponent<SentryTrackerComponent>();
                    }
                }
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "no_sentry", "No sentry was found at that location." },
                { "added", "You've made this sentry into an infinite sentry." },
                { "destroyed", "You've revoked infinite status from this sentry." },
                { "disabled", "This command has been disabled; all sentries are infinite." }
            };
    }
}
