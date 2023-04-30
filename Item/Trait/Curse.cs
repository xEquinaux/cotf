using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cotf.Base;
using cotf.Buff;
using cotf.ID;

namespace cotf
{
    internal class Curse : Trait
    {
        public override void Cursed(Item item, bool cursed)
        {
            switch (item.type)
            {
                case ItemID.Broadsword:
                    item.damage = (int)(item.damage * 1.2f);
                    item.useSpeed /= 2;
                    break;
                case ItemID.Boots:
                    Main.myPlayer.moveSpeed /= 1.5f;
                    break;
            }
        }
        public override void RemoveEffect(Player player)
        {
            
        }
    }
}
