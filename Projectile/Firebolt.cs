using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Foundation;
using CirclePrefect;
using cotf.Base;
using cotf.World;

namespace cotf
{
    public class Proj_Firebolt : Projectile
    {
        public override Rectangle hitbox => new Rectangle((int)position.X, (int)position.Y, width, height);
        public override void SetDefaults()
        {
            type = ProjectileID.FireBolt;
            damage = 10;
            knockBack = 1f;
            friendly = true;
            hostile = true;
            width = 16;
            height = 8;
            timeLeft = 600;
            defaultColor = Color.Red;
            base.SetDefaults();
        }
        public override void Collide()
        {
            if (Collision(this, 4))
            { 
                Dispose();
            }
        }
        public override void HitNPC(Npc npc)
        {
        }
        public override void HitPlayer(Player player)
        {
        }
        public override void AI()
        {
            if (lamp == null)
            {
                int index = Lamp.NewLamp(X, Y, 100f);
                light = Main.lamp[index];
                light.parent = this;
                light.color = Color.Red;
            }
            light.position = position;
            position += velocity;
            if (timeLeft-- < 0)
                Dispose();
            base.AI();
        }
        public override void Draw(Graphics graphics)
        {
            Drawing.DrawRotate(texture, hitbox, 0f, new PointF(width / 2, height / 2), Color.Black, RotateType.MatrixTransform, graphics);
        }
    }
}
