using System;
using UnityEngine;

namespace Game.Model
{
    public class LinkModel
    {
        public PortalModel Source { get; private set; }
        public PortalModel Target { get; private set; }
        public Vector2 Vector { get { return Target.Position - Source.Position; } }

        public Faction Faction { get { return Source.Faction; } }

        public LinkModel(PortalModel source, PortalModel target)
        {
            Source = source;
            Target = target;
        }

        public PortalModel OtherEnd(PortalModel portal)
        {
            if (!IsConnecting(portal))
            {
                throw new ArgumentException(string.Format("Portal {0} is not a part of link {1}", portal, this));
            }
            return Source == portal ? Target : Target == portal ? Source : null;
        }

        public bool IsConnecting(LinkModel other)
        {
            return IsConnecting(other.Source) || IsConnecting(other.Target);
        }

        public bool IsConnecting(PortalModel portal)
        {
            return Source == portal || Target == portal;
        }

        public bool EqualsIgnoreDirection(LinkModel other)
        {
            return (Source == other.Source && Target == other.Target) || (Source == other.Target && Target == other.Source);
        }

        public bool Intersects(LinkModel other)
        {
            if (IsConnecting(other))
            {
                // connection is ok
                return false;
            }

            var a1 = Source.Position;
            var a2 = Target.Position;
            var b1 = other.Source.Position;
            var b2 = other.Target.Position;
            var a12 = a2 - a1;
            var b12 = b2 - b1;

            var p = Vector3.Cross(a12, b1 - a1).z * Vector3.Cross(a12, b2 - a1).z;
            var q = Vector3.Cross(b12, a1 - b1).z * Vector3.Cross(b12, a2 - b1).z;

            if (p == 0 && q == 0)
            {
                // straight
                var b1r = Vector2.Dot(a12, b1 - a1) / a12.sqrMagnitude;
                var b2r = Vector2.Dot(a12, b2 - a1) / a12.sqrMagnitude;
                return (!Util.InRange(b1r, 0, 1) && Util.InRange(b2r, 0, 1)) || (!Util.InRange(b2r, 0, 1) && Util.InRange(b1r, 0, 1));
            }
            else if (p <= 0 && q <= 0)
            {
                // cross, inclusive
                return true;
            }
            else
            {
                // OK
                return false;
            }
        }

        public bool Overlaps(PortalModel portal)
        {
            if (IsConnecting(portal))
            {
                return false; // connecting is OK
            }

            var p = portal.Position - Source.Position;
            return Vector3.Cross(Vector, p).z == 0 && Util.InRange(Vector2.Dot(Vector, p) / Vector.sqrMagnitude, 0, 1);
        }

        public override string ToString()
        {
            return string.Format("L({0}->{1})", Source, Target);
        }
    }
}
