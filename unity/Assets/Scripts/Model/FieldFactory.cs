using System.Linq;

namespace Game.Model
{
    public class FieldFactory
    {
        private const int _ = (int)FieldModel.FieldItem.None;
        private const int X = (int)FieldModel.FieldItem.Block;
        private const int O = (int)FieldModel.FieldItem.Portal;
        private const int P = (int)FieldModel.FieldItem.Player;
        private const int F = (int)FieldModel.FieldItem.Friendly;
        private const int A = (int)FieldModel.FieldItem.Enemy;
        private const int B = (int)FieldModel.FieldItem.Enemy + 1;
        private const int C = (int)FieldModel.FieldItem.Enemy + 2;

        public static FieldModel CreateDefaultField(int level)
        {
            var data = new int[]
            {
                X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X,
                X, O, _, _, _, _, _, _, _, X, X, _, _, _, _, _, X, X, X, _, _, _, _, _, _, _, X, X, X, _, _, _, _, _, _, _, _, X, X, _, _, _, _, _, _, X, X, X, _, X,
                X, _, X, X, _, _, X, X, _, X, X, _, X, X, _, _, X, X, _, F, _, X, X, X, X, O, X, X, X, _, _, X, _, _, _, _, _, X, X, _, O, _, _, _, _, X, X, X, _, X,
                X, _, X, X, _, O, X, X, _, _, _, O, _, _, O, _, X, X, _, _, X, X, X, X, X, _, X, X, X, _, _, X, X, X, X, X, F, X, X, _, _, _, X, O, _, X, X, X, _, X,
                X, _, X, X, _, _, X, X, X, X, _, _, _, O, O, _, _, _, _, _, O, _, _, _, _, _, _, _, _, O, _, X, X, X, X, X, _, _, _, _, X, X, X, _, _, _, _, _, _, X,
                X, _, X, X, _, O, X, X, X, X, _, _, X, X, X, _, X, X, _, X, X, _, _, _, _, _, _, _, _, _, _, A, _, O, _, _, _, O, _, _, X, X, X, _, X, X, X, X, _, X,
                X, _, X, X, _, _, _, _, _, _, O, O, X, X, X, _, X, X, _, X, X, _, O, _, _, X, X, X, _, _, _, X, X, _, _, _, X, X, _, _, _, _, _, _, X, X, O, _, _, X,
                X, _, _, _, _, _, O, O, _, X, _, _, X, X, X, _, X, X, _, X, X, _, _, _, _, X, X, X, _, _, _, X, X, _, _, _, X, X, _, _, _, O, _, _, _, _, _, _, _, X,
                X, _, _, _, _, _, _, _, P, X, _, O, _, _, O, _, X, X, O, _, _, _, _, _, _, _, _, _, _, X, X, _, _, O, _, _, X, X, _, _, _, _, X, _, _, _, _, _, _, X,
                X, _, X, X, _, X, _, X, _, _, A, _, _, _, _, _, _, _, _, X, X, _, X, X, X, X, X, O, _, X, X, _, O, O, _, _, _, _, _, _, X, X, X, _, X, X, X, X, _, X,
                X, _, X, X, _, X, X, X, _, X, X, X, _, X, X, _, X, O, _, X, X, _, O, X, _, X, X, X, _, X, X, O, O, _, _, _, _, O, _, _, X, X, X, _, X, X, X, X, _, X,
                X, _, X, X, _, _, _, O, _, X, X, X, _, X, X, _, X, X, _, X, X, _, _, X, _, X, X, X, _, X, X, _, _, _, _, _, X, X, _, _, _, _, _, O, _, _, _, _, _, X,
                X, _, X, _, _, _, _, _, _, X, X, X, _, X, X, O, _, _, _, _, _, _, _, _, _, _, _, O, _, F, _, _, _, _, _, _, X, X, _, _, _, C, _, _, X, X, _, _, _, X,
                X, _, _, O, _, X, X, X, X, X, X, X, _, X, X, _, _, _, _, B, _, O, _, X, X, X, X, X, X, X, _, _, _, X, X, X, X, X, _, O, _, X, X, X, X, X, _, O, _, X,
                X, _, _, _, _, _, _, _, _, _, O, _, _, X, X, _, _, _, _, _, _, _, _, X, X, X, X, X, X, X, _, _, _, _, O, _, _, _, _, _, _, X, X, X, X, X, _, _, _, X,
                X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X, X,
            };

            var field = new FieldModel(50, 16);
            field.SetItemsByIntArray(data.Select(i => CutOffEnemies(i, level)).ToArray());
            field.FlipVertically();

            return field;
        }

        private static int CutOffEnemies(int x, int level)
        {
            if (x < A)
            {
                return x == F ? _ : x; // disable friendlies
            }
            else
            {
                return (x > A + level) ? _ : A; // filter out some enemies as per the level
            }
        }
    }
}
