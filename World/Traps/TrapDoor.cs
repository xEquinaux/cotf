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
    internal class TrapDoor : Trap
    {
        protected override void Init()
        {
            name = "Trap Door";
        }
        public override void Update()
        {
            if (!base.PreUpdate())
                return;
            if (Contains(Main.myPlayer))
            {
                //Main.GenerateFloor(player: Main.myPlayer);
            }
        }
        public override void Draw(Graphics graphics)
        {
            if (!base.PreUpdate())
                return;
            base.Draw(graphics);
        }
    }
}
