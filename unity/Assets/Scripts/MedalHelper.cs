using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Model;

public class MedalHelper {
    public MedalRow.MedalType[] Results =
        new MedalRow.MedalType[(int)MedalRow.AchievementType.NumberOfAchievements];

    public MedalHelper(AchievementModel achievement)
    {
        DecideMedalType(MedalRow.AchievementType.Liberator, achievement.Liberator, 20, 40, 60, 80, 100);
        DecideMedalType(MedalRow.AchievementType.Connector, achievement.Connector, 20, 40, 60, 80, 100);
        DecideMedalType(MedalRow.AchievementType.MindController, achievement.MindController, 10, 15, 20, 30, 50);
        DecideMedalType(MedalRow.AchievementType.Illuminator, achievement.Illuminator, 100, 200, 400, 800, 1600);
        DecideMedalType(MedalRow.AchievementType.Explorer, achievement.Explorer, 30, 34, 38, 42, 43);
        DecideMedalType(MedalRow.AchievementType.Pioneer, achievement.Pioneer, 20, 25, 30, 40, 43);
        DecideMedalType(MedalRow.AchievementType.Hacker, achievement.Hacker, 100, 150, 300, 450, 800);
    }

    private void DecideMedalType(MedalRow.AchievementType achievementType, int value, params int[] thresholds)
    {
        var medalType = DecideMedalType(value, thresholds);
        Results[(int)achievementType] = medalType;

        Debug.Log(string.Format("Medal: {0} {1} {2}", achievementType, value, medalType));
    }

    private MedalRow.MedalType DecideMedalType(int value, params int[] thresholds)
    {
        for (int i = 0; i < (int)MedalRow.MedalType.NumberOfMedalTypes - 1; i++)
        {
            if (value < thresholds[i])
            {
                return (MedalRow.MedalType)i;
            }
        }

        return MedalRow.MedalType.Onyx;
    }
}
