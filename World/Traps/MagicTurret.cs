using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CirclePrefect;
using cotf.Base;
using cotf.ID;

namespace cotf.World.Traps
{
    public class MagicTurret : Trap
    {
        protected override void Init()
        {
            name = "Magic Turret";
            damage = 10;
            elapse = 180;
        }
        public override void Update()
        {
            if (!base.PreUpdate(true))
                return;
            if (Main.myPlayer.IsMoving())
            {
                if (SightLine(Main.myPlayer))
                {
                    ticks++;
                    if (ticks % elapse == 0)
                    {
                        float angle = AngleTo(Main.myPlayer.Center);
                        Projectile.NewProjectile(Center, Helper.AngleToSpeed(angle, 4f), angle, ProjectileID.FireBolt, this); 
                    }
                }
            }
        }
        public override void Draw(Graphics graphics)
        {
            if (!base.PreUpdate(true))
                return;
            Drawing.TextureLighting(texture, hitbox, this, graphics);
        }
    }
}
