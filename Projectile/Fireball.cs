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
    public class Proj_Fireball : Projectile
    {
        public override Rectangle hitbox => new Rectangle((int)position.X, (int)position.Y, width, height);
        public override void SetDefaults()
        {
            type = ProjectileID.Fireball;
            damage = 10;
            knockBack = 2f;
            friendly = true;
            width = 24;
            height = 8;
            timeLeft = 600;
            defaultColor = Color.Red;
            base.SetDefaults();
        }
        protected override void Init()
        { 
            texture = Main.texture;
            int i = Lamp.NewLamp(new Vector2(X, Y), 50f, defaultColor, this, false, this.whoAmI);
            light = Main.lamp[i];
            light.isProj = true;
            light.parent = this;
        }
        public override void AI()
        {
            position += velocity;
            if (timeLeft-- < 0)
                Dispose();
            base.AI();
        }                                     
        public override void HitNPC(Npc n)
        {
            if (n.NpcProjHit(this))
            {
                n.NpcHurt(damage, knockBack, Helper.AngleTo(n.Center, Center));
                switch (type)
                {
                    case ProjectileID.Fireball:
                    default:
                        Dispose();
                        break;
                }
            }
        }
        public override void Collide()
        {
            if (Tile.GetSafely((int)position.X / Tile.Size, (int)position.Y / Tile.Size).Active)
            {
                color = Color.Transparent;
                velocity = Vector2.Zero;
                light.range++;
            }
            if (light.range > Sight + 30)
                Dispose();
        }
        public override void Draw(Graphics graphics)
        {
            Drawing.DrawRotate(texture, hitbox, new Rectangle(0, 0, hitbox.Width, hitbox.Height), angle, new PointF(width / 2, height / 2), color, Color.Black, RotateType.MatrixTransform, graphics);
        }
    }
}
