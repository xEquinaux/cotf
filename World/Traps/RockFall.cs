using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CirclePrefect;
using Foundation;
using cotf;
using cotf.Base;
using cotf.ID;

namespace cotf.World.Traps
{
    internal class RockFall : Trap
    {
        bool trip;
        protected override void Init()
        {
            name = "Rock Fall";
            damage = 20;
        }
        public override void Update()
        {
            if (trip || !base.PreUpdate(true))
                return;
            if (Contains(Main.myPlayer))
            {
                if (Main.myPlayer.IsControlMove())
                {
                    trip = true;
                    Main.myPlayer.Hurt(damage, 5f, 0f);
                }
            }
        }
        public override void Draw(Graphics graphics)
        {
            if (!trip || !base.PreUpdate(true))
                return;
            base.Draw(graphics);
        }
    }
}
