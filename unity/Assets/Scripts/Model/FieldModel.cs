using System;
using System.Linq;

namespace Game.Model
{
    public class FieldModel
    {
        public enum FieldItem
        {
            None, Block, Portal, Player, Friendly, Enemy
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private FieldItem[] fieldItems;

        public FieldModel(int width, int height)
        {
            Width = width;
            Height = height;
            fieldItems = new FieldItem[width * height];
        }

        public FieldItem GetItemAt(int x, int y)
        {
            return fieldItems[GetIndex(x, y)];
        }

        public void SetItemAt(int x, int y, FieldItem item)
        {
            fieldItems[GetIndex(x, y)] = item;
        }

        public void SetItemsByIntArray(int[] items)
        {
            if (items.Length != Width * Height)
            {
                throw new ArgumentException("The array size " + items.Length + " does not match to the field size " + Width + " x " + Height);
            }

            fieldItems = items.Select(i => (FieldItem)i).ToArray();
        }

        public void FlipVertically()
        {
            for (int y1 = 0; y1 < Height / 2; y1++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var y2 = Height - 1 - y1;
                    var item1 = GetItemAt(x, y1);
                    var item2 = GetItemAt(x, y2);
                    SetItemAt(x, y1, item2);
                    SetItemAt(x, y2, item1);
                }
            }
        }

        private int GetIndex(int x, int y)
        {
            return x + y * Width;
        }
    }
}
