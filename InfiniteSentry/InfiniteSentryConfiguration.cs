using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    public class InfiniteSentryConfiguration : IRocketPluginConfiguration
    {
        public bool allSentriesInfinite;
        public List<SentryPosition> sentries;

        public void LoadDefaults()
        {
            allSentriesInfinite = false;
            sentries = new List<SentryPosition>();
        }
    }
}
