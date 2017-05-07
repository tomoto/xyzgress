using System.Collections.Generic;

namespace Game.Model
{
    public class AchievementModel
    {
        private static readonly int[] RequiredAPForLevel = { 0, 2500, 20000, 70000, 150000, 300000, 600000, 1200000 };

        public static AchievementModel DummyAchievement
        {
            get { return new AchievementModel(); }
        }

        public int Liberator { get; private set; }
        public int Connector { get; private set; }
        public int MindController { get; private set; }
        public int Illuminator { get; private set; }
        public int Explorer { get; private set; }
        public int Pioneer { get; private set; }
        public int Hacker { get; private set; }
        public int AP { get; private set; }
        public int Level { get; private set; }
        public bool IsAPChanged { get; private set; }

        private ICollection<PortalModel> visitedPortals = new HashSet<PortalModel>();
        private ICollection<PortalModel> capturedPortals = new HashSet<PortalModel>();

        public AchievementModel()
        {
            Level = 1;
        }

        public void Hack(PortalModel portal, Faction faction = Faction.Player)
        {
            Hacker++;

            AP += (portal.Faction == Faction.None || portal.Faction == faction) ? 0 : 100;

            VisitPortal(portal);

            APChanged();
        }

        public void Capture(PortalModel portal)
        {
            VisitPortal(portal);

            Liberator++;

            if (!capturedPortals.Contains(portal))
            {
                Pioneer++;
                capturedPortals.Add(portal);
            }

            AP += 125 * 8 + 500 + 250;

            APChanged();
        }

        private void VisitPortal(PortalModel portal)
        {
            if (!visitedPortals.Contains(portal))
            {
                Explorer++;
                visitedPortals.Add(portal);
            }
        }

        public void CreateLink()
        {
            Connector++;

            AP += 313;

            APChanged();
        }

        public void DestroyPortal()
        {
            AP += 75 * 8;

            APChanged();
        }

        public void DestroyLink()
        {
            AP += 187;

            APChanged();
        }

        public void DestroyCF()
        {
            AP += 750;

            APChanged();
        }

        public void CreateCF(int mu)
        {
            MindController++;
            Illuminator += mu;

            AP += 1250;

            APChanged();
        }

        public void Recharge()
        {
            AP += 10;

            APChanged();
        }

        public void ResetAPChanged()
        {
            IsAPChanged = false;
        }


        private void APChanged()
        {
            IsAPChanged = true;
            if (Level < RequiredAPForLevel.Length && AP >= RequiredAPForLevel[Level])
            {
                Level++;
            }
        }
    }
}
