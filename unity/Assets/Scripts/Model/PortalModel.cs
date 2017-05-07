using UnityEngine;

namespace Game.Model
{
    public class PortalModel
    {
        public Vector2 Position { get; private set; }
        public Faction Faction { get; set; }
        public int NumberOfLinks { get;  set; }
        public float Mitigation { get { return 400 / 9 * Mathf.Atan(NumberOfLinks / Mathf.Exp(1)); } }

        public PortalModel(Vector2 position, Faction faction)
        {
            Position = position;
            Faction = faction;
        }

        public float DistanceFrom(Vector2 p)
        {
            return (p - Position).magnitude;
        }

        public override string ToString()
        {
            return string.Format("P({0},{1})", Position.x, Position.y);
        }
    }
}
