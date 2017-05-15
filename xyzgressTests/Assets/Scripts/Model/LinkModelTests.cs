using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Model.Tests
{
    [TestClass()]
    public class LinkModelTests
    {
        private LinkModel L(PortalModel p1, PortalModel p2)
        {
            return new LinkModel(p1, p2);
        }

        private PortalModel P(float x, float y)
        {
            return new PortalModel(new Vector2(x, y), Faction.Player);
        }

        [TestMethod()]
        public void IntersectsReturnsFalseForSeparateLinks()
        {
            var p = new[] { P(0, 0), P(1, 0), P(2, 0), P(3, 1) };
            var l = new[] { L(p[0], p[1]), L(p[2], p[3]) };
            Assert.IsFalse(l[0].Intersects(l[1]));
        }

        [TestMethod()]
        public void IntersectsReturnsFalseForConnectingLinks()
        {
            var p = new[] { P(0, 0), P(1, 0), P(2, 1) };
            var l = new[] { L(p[0], p[1]), L(p[1], p[2]) };
            Assert.IsFalse(l[0].Intersects(l[1]));
        }

        [TestMethod()]
        public void IntersectsReturnsTrueForTShapeLinks()
        {
            var p = new[] { P(0, 0), P(2, 0), P(1, 0), P(1, 2) };
            var l = new[] { L(p[0], p[1]), L(p[2], p[3]) };
            Assert.IsTrue(l[0].Intersects(l[1])); // Top first
            Assert.IsTrue(l[1].Intersects(l[0])); // Stem first
        }

        [TestMethod()]
        public void IntersectsReturnsFalseForStraightSeparateLinks()
        {
            var p = new[] { P(0, 0), P(2, 2), P(3, 3), P(4, 4) };
            var l = new[] { L(p[0], p[1]), L(p[2], p[3]) };
            Assert.IsFalse(l[0].Intersects(l[1])); // Forward
            Assert.IsFalse(l[1].Intersects(l[0])); // Backward
        }

        [TestMethod()]
        public void IntersectsReturnsFalseForStraightContainingLinks()
        {
            var p = new[] { P(0, 0), P(2, 2), P(3, 3), P(4, 4) };
            var l = new[] { L(p[0], p[3]), L(p[2], p[1]) };
            Assert.IsFalse(l[0].Intersects(l[1])); // lhs contains rhs
            Assert.IsFalse(l[1].Intersects(l[0])); // lhs is contained by rhs
        }

        [TestMethod()]
        public void IntersectsReturnsFalseForStraightConnectingLinks()
        {
            var p = new[] { P(0, 0), P(2, 2), P(3, 3) };
            var l = new[] { L(p[0], p[1]), L(p[1], p[2]) };
            Assert.IsFalse(l[0].Intersects(l[1]));
            Assert.IsFalse(l[1].Intersects(l[0]));
        }

        [TestMethod()]
        public void IntersectsReturnsFalseForStraightConnectingAndContainingLinks()
        {
            var p = new[] { P(0, 0), P(2, 2), P(3, 3) };
            var l = new[] { L(p[0], p[2]), L(p[1], p[2]) };
            Assert.IsFalse(l[0].Intersects(l[1])); // lhs contains rhs
            Assert.IsFalse(l[1].Intersects(l[0])); // lhs is contained by rhs
        }

        [TestMethod()]
        public void IntersectsReturnsTrueForStraightOverlappingLinks()
        {
            var p = new[] { P(0, 0), P(2, 2), P(3, 3), P(4, 4) };
            var l = new[] { L(p[0], p[2]), L(p[3], p[1]) };
            Assert.IsTrue(l[0].Intersects(l[1]));
        }
    }
}