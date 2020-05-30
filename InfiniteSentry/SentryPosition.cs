using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtraConcentratedJuice.InfiniteSentry
{
    public class SentryPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public SentryPosition(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Overriding Equals and not GetHashCode will cause inconsistencies
        public bool CompareVector3(Vector3 position) => X == position.x && Y == position.y && Z == position.z;

        public SentryPosition() { }
    }
}
