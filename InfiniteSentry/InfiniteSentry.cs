using HarmonyLib;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System.Reflection;
using Rocket.API.Collections;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    public class InfiniteSentry : RocketPlugin<InfiniteSentryConfiguration>
    {
        public static InfiniteSentry Instance { get; private set; }
        public bool ShuttingDown { get; private set; }

        public static FieldInfo HasWeapon;

        protected override void Load()
        {
            Instance = this;
            ShuttingDown = false;
            Provider.onServerShutdown += OnShutdown;
            Level.onPostLevelLoaded += OnLevelLoaded;

            HasWeapon = typeof(InteractableSentry).GetField("hasWeapon",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Harmony harmony = new Harmony("pw.cirno.extraconcentratedjuice");

            var orig = typeof(InteractableSentry).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
            var pre = typeof(SentryUpdateOverride).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic);
            var post = typeof(SentryUpdateOverride).GetMethod("Postfix", BindingFlags.Static | BindingFlags.NonPublic);

            harmony.Patch(orig, new HarmonyMethod(pre), new HarmonyMethod(post));
            
            Logger.Log("Documentation available at: https://iceplugins.xyz/InfiniteSentry/");
        }

        protected override void Unload()
        {
            Instance.Configuration.Save();
            
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

                    if (s == null) 
                        continue;
                    
                    // Consider using reverse for loop here. SentryTrackerComponent can modify collection
                    // Using foreach will cause an exception to be thrown if/when this happens
                    foreach (SentryPosition pos in Configuration.Instance.sentries)
                        if (pos.CompareVector3(s.transform.position))
                            s.gameObject.AddComponent<SentryTrackerComponent>();
                }
        }

        public void TellPlayer(SteamPlayer recipient, Color color, string translationKey,
            params object[] translationParameters) =>
            ChatManager.serverSendMessage(Instance.Translate(translationKey, translationParameters), color,
                toPlayer: recipient);

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
