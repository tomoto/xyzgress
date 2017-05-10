using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Model
{
    public class LinkManager
    {
        private List<PortalModel> portals = new List<PortalModel>();
        private List<LinkModel> links = new List<LinkModel>();
        private List<CFModel> cfs = new List<CFModel>();

        public float GetMU(Faction faction)
        {
            return cfs.Where(cf => cf.Faction == faction).Sum(cf => cf.MU);
        }

        public PortalModel AddPortal(Vector2 position, Faction faction)
        {
            var portal = new PortalModel(position, faction);
            portals.Add(portal);
            return portal;
        }

        public IEnumerable<PortalModel> FindLinkablePortals(Faction faction, KeyInventoryModel inventory, PortalModel source)
        {
            string message;
            if (!IsLinkableSource(faction, source, out message))
            {
                Debug.Log(message);
                return Enumerable.Empty<PortalModel>();
            }

            var candidatePortals = portals.Where(p => IsLinkableTarget(faction, inventory, source, p, out message)).ToList();
            candidatePortals.Sort((a, b) => a.DistanceFrom(source.Position).CompareTo(b.DistanceFrom(source.Position)));
            return candidatePortals;
        }

        public bool IsLinkableSource(Faction faction, PortalModel source, out string message)
        {
            if (source.Faction != faction)
            {
                message = string.Format("Source portal {0} is an enemy portal.", source);
                return false;
            }

            if (cfs.Any(cf => cf.Overlaps(source)))
            {
                message = string.Format("Source portal {0} is overlapped by a CF.", source);
                return false;
            }

            message = null;
            return true;
        }

        public bool IsLinkableTarget(Faction faction, KeyInventoryModel inventory, PortalModel source, PortalModel target, out string message)
        {
            if (source == target)
            {
                message = string.Format("Source portal {0} is also the target portal.", source);
                return false;
            }

            if (target.Faction != faction)
            {
                message = string.Format("Target portal {0} is an enemy portal.", target);
                return false;
            }

            if (!inventory.HasKey(target))
            {
                message = string.Format("No key for target portal {0}.", target);
                return false;
            }

            LinkModel newLink = new LinkModel(source, target);

            if (links.Any(link => link.EqualsIgnoreDirection(newLink)))
            {
                message = string.Format("Link already exists between portal {0} and {1}.", source, target);
                return false;
            }

            if (links.Any(link => link.Intersects(newLink)))
            {
                message = "Link crosses an existing link.";
                return false;
            }

            message = "";
            return true;
        }

        public LinkModel AddLink(Faction faction, KeyInventoryModel inventory, PortalModel source, PortalModel target)
        {
            string message;

            if (IsLinkableSource(faction, source, out message) && IsLinkableTarget(faction, inventory, source, target, out message))
            {
                LinkModel newLink = new LinkModel(source, target);
                links.Add(newLink);
                source.NumberOfLinks++;
                target.NumberOfLinks++;
                return newLink;
            }
            else
            {
                throw new Exception(message);
            }
        }

        public IEnumerable<CFModel> AddCFs(LinkModel newLink)
        {
            var connectingLinks = links.Where(link => link != newLink && link.IsConnecting(newLink));

            var connectingLinkCombinations = connectingLinks.SelectMany(link1 => 
                connectingLinks.Where(l => l != link1).Select(link2 => new LinkModel[] { link1, link2 })
                ).ToList();

            var triangleLinkCombinations = connectingLinkCombinations.Where(lc =>
                lc[0].IsConnecting(newLink.Source) &&
                lc[1].IsConnecting(newLink.Target) &&
                lc[0].OtherEnd(newLink.Source) == lc[1].OtherEnd(newLink.Target));

            var candidateCFs = triangleLinkCombinations.Select(lc => new CFModel(newLink, lc[0], lc[1])).ToList();

            // TODO: Should exclude smaller overlapping CFs?

            cfs.AddRange(candidateCFs);

            return candidateCFs;
        }

        public void DestroyPortal(PortalModel portal, out IList<LinkModel> destroyedLinksResult, out IList<CFModel> destroyedCFsResult)
        {
            var destroyedLinks = links.Where(link => link.IsConnecting(portal)).ToList();
            var destroyedCFs = cfs.Where(cf => destroyedLinks.Any(cf.Contains)).ToList();

            foreach (var link in destroyedLinks)
            {
                link.Source.NumberOfLinks--;
                link.Target.NumberOfLinks--;
                links.Remove(link);
            }

            foreach (var cf in destroyedCFs)
            {
                cfs.Remove(cf);
            }

            destroyedLinksResult = destroyedLinks;
            destroyedCFsResult = destroyedCFs;
        }
    }
}
