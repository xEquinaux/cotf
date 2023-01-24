using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CirclePrefect;
using cotf.Base;
using cotf.ID;

namespace cotf.World.Traps
{
    public class FogMachine : Trap
    {
        protected override void Init()
        {
            name = "Fog Machine";
            width = 50;
            height = 50;
        }
        public override void Update()
        {
            if (!base.PreUpdate(true))
                return;
            Room room = EntityHelper.GetRoom(t => t != null && t.bounds.Contains(Center));
            if (room != default)
            {
                room.fog = true;
            }
        }
        public override void Draw(Graphics graphics)
        {
            if (!base.PreUpdate(true))
                return;
            Bitmap tex = Drawing.Mask_Circle(100, Main.Mask);
            Bitmap result = (Bitmap)Drawing.TextureMask((Bitmap)texture, tex, Main.Mask);
            Drawing.TextureLighting(result, hitbox, this, graphics);
        }
    }
}
