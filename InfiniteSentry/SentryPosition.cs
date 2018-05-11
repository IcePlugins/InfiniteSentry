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

        public override bool Equals(object obj)
        {
            if (obj is Vector3 vec)
                return X == vec.x && Y == vec.y && Z == vec.z;

            if (obj is SentryPosition pos)
                return X == pos.X && Y == pos.Y && Z == pos.Z;

            return false;
        }
            

        public SentryPosition() { }
    }
}
