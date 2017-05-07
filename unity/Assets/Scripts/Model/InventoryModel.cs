using System.Collections.Generic;

namespace Game.Model
{
    public class KeyInventoryModel
    {
        private ICollection<PortalModel> portals = new HashSet<PortalModel>();

        public bool HasKey(PortalModel portal)
        {
            return portals.Contains(portal);
        }

        public void AddKey(PortalModel portal)
        {
            portals.Add(portal);
        }

        public void RemoveKey(PortalModel portal)
        {
            portals.Remove(portal);
        }

    }

    public class ItemInventoryModel : KeyInventoryModel
    {
        public const int MaxBusters = GameConstants.MaxBurstersInInventory;

        public int Bursters { get; set; }

        public void AddBursters(int value)
        {
            if (Bursters < MaxBusters)
            {
                Bursters += value;
            }
        }
    }
}
