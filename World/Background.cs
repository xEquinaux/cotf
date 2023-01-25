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


namespace cotf.World
{
    public class Background : Entity
    {
        public new Rectangle box => new Rectangle((int)position.X, (int)position.Y, width, height);
        public bool lit = false;
        public bool onScreen = false;
        public Color color0 = Color.Black;
        public DoorFacing doorFace = DoorFacing.None;
        public List<Entity> ent = new List<Entity>();

        public Background(int i, int j, int size)
        {
            name = "Background";
            TextureName = "background";
            active = true;
            width = size + 1;
            height = size + 1;
            color = color0;
            position = new Vector2(i * size, j * size);
            Main.background[i, j] = this;
            defaultColor = Color.Black;
            Init();
        }
        private void Init()
        {
            //  Brush style texture init
            Bitmap bmp = new Bitmap(50, 50);
            using (Graphics gfx = Graphics.FromImage(bmp))
                gfx.FillRectangle(Brushes.Gray, new Rectangle(0, 0, 50, 50));
            texture = preTexture = bmp;
        }
        private bool PreUpdate()
        {
            return onScreen =
                position.X >= Main.myPlayer.position.X - Main.ScreenWidth / 2 &&
                position.X <= Main.myPlayer.position.X + Main.ScreenWidth / 2 &&
                position.Y >= Main.myPlayer.position.Y - Main.ScreenHeight / 2 &&
                position.Y <= Main.myPlayer.position.Y + Main.ScreenHeight / 2;
        }
        public new void Update()
        {
            if (!PreUpdate())
                return;
            if (!discovered)
                discovered = Discovered(Main.myPlayer) || lit;
            inShadow = !(lit && discovered);
        }
        public void Draw(Graphics graphics)
        {
            if (!active || !onScreen || !discovered)
                return;
            //  Lightmap interaction
            Lightmap map;
            (map = Main.lightmap[box.X / Tile.Size, box.Y / Tile.Size]).Update(this);
            Drawing.TextureLighting(preTexture, hitbox, map, this, Main.Gamma, graphics);
            //  Experimental
            //graphics.DrawImage(preTexture, hitbox);
            //graphics.DrawImage(Drawing.Lightpass0(preTexture, new Lightmap(10, 10), position, Main.myPlayer.lamp), hitbox);
            //Drawing.BrushLighting(new Lightmap(10, 10), this, Main.myPlayer.lamp, graphics);
        }
        public static Background GetSafely(int i, int j)
        {
            return Main.background[Math.Max(Math.Min(i, Main.background.GetLength(0) - 1), 0), Math.Max(Math.Min(j, Main.background.GetLength(1) - 1), 0)];
        }
    }
    public enum DoorFacing
    {
        None = 0,
        FacingUp = 1,
        FacingRight = 2,
        FacingDown = 3,
        FacingLeft = 4
    }
}
