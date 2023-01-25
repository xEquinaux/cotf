using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CirclePrefect;
using cotf;
using cotf.Base;
using cotf.World;

namespace cotf
{
    public class SilverCoin : Coin
    {
        protected override Color rarity => base.rarity;
        public override ToolTip ToolTip => toolTip = new ToolTip(name, text, rarity);
        public override void SetDefaults()
        {
            base.SetDefaults();
            name = "Silver Coin";
            value = 100;
            isCoin = true;
            friendly = true;
        }
        protected override void Init()
        {
            defaultColor = CoinType.cSilver;
            base.Init();
        }
    }
}
