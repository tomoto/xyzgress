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
                return false;
            }

            var a1 = Source.Position;
            var a2 = Target.Position;
            var b1 = other.Source.Position;
            var b2 = other.Target.Position;

            return
                Vector3.Cross(a2 - a1, b1 - a1).z * Vector3.Cross(a2 - a1, b2 - a1).z < -float.Epsilon &&
                Vector3.Cross(b2 - b1, a1 - b1).z * Vector3.Cross(b2 - b1, a2 - b1).z < -float.Epsilon;
        }

        public override string ToString()
        {
            return string.Format("L({0}->{1})", Source, Target);
        }
    }
}
