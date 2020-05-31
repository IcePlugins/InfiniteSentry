using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    public class SentryTrackerComponent : MonoBehaviour
    {
        public SentryTrackerComponent()
        {
            Vector3 pos = transform.position;
            SentryPosition newPos = new SentryPosition(pos.x, pos.y, pos.z);

            if (InfiniteSentry.Instance.Configuration.Instance.sentries.Any(x => x.CompareVector3(pos)))
                return;
            
            InfiniteSentry.Instance.Configuration.Instance.sentries.Add(newPos);
            InfiniteSentry.Instance.ChangedConfig = true;
        }

        public void OnDisable()
        {
            if (InfiniteSentry.Instance.ShuttingDown)
                return;

            SentryPosition pos = InfiniteSentry.Instance.Configuration.Instance.sentries.FirstOrDefault(x =>
                x.CompareVector3(transform.position));

            if (pos == null)
                return;
            
            InfiniteSentry.Instance.Configuration.Instance.sentries.Remove(pos);
            InfiniteSentry.Instance.ChangedConfig = true;
        }
    }
}
