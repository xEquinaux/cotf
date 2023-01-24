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
    internal class Spikes : Trap
    {
        protected override void Init()
        {
            name = "Spikes";
            damage = 10;
        }
        public override void Update()
        {
            if (!base.PreUpdate(true))
                return;
            if (Contains(Main.myPlayer))
            {
                if (Main.myPlayer.IsMoving() && Main.myPlayer.iFrames == 0)
                {
                    Main.myPlayer.Hurt(damage, 5f, Helper.AngleTo(Center, Main.myPlayer.Center));
                    Main.myPlayer.iFrames = Main.myPlayer.iFramesMax;
                }
            }
        }
        public override void Draw(Graphics graphics)
        {
            if (!base.PreUpdate(true))
                return;
            base.Draw(graphics);
        }
    }
}
