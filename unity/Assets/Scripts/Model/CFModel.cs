using System;
using System.Linq;
using UnityEngine;

namespace Game.Model
{
    public class CFModel
    {
        public LinkModel[] Links { get; private set; }
        public PortalModel[] Portals { get; private set; }
        public int MU { get; private set; }

        public Faction Faction { get { return Links[0].Faction; } }

        public CFModel(params LinkModel[] links)
        {
            if (links.Length != 3)
            {
                throw new ArgumentException("The number of links must be 3.");
            }

            Links = links;

            Portals = links.SelectMany(link => new[] { link.Source, link.Target }).Distinct().ToArray();

            MU = Mathf.FloorToInt(Mathf.Abs(Vector3.Cross(links[0].Vector, links[1].Vector).z));
        }

        public bool Contains(LinkModel link)
        {
            return Links.Contains(link);
        }

        public bool Overlaps(PortalModel portal)
        {
            return
                IsSameSide(Portals[0].Position, Portals[1].Position, Portals[2].Position, portal.Position) &&
                IsSameSide(Portals[1].Position, Portals[2].Position, Portals[0].Position, portal.Position) &&
                IsSameSide(Portals[2].Position, Portals[0].Position, Portals[1].Position, portal.Position);
        }

        private bool IsSameSide(Vector2 a, Vector2 b, Vector2 p1, Vector2 p2)
        {
            return IsSameSide(b - a, p1 - a, p2 - a);
        }

        private bool IsSameSide(Vector2 v, Vector2 p1, Vector2 p2)
        {
            return Vector3.Cross(v, p1).z * Vector3.Cross(v, p2).z > float.Epsilon;
        }

        public override string ToString()
        {
            return string.Format("CF({0},{1},{2},{3})", Links[0].ToString(), Links[1].ToString(), Links[2].ToString(), MU);
        }
    }
}
