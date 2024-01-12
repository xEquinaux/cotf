using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using cotf.Base;
using cotf.World;
using System.Threading;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotf
{
    public sealed class LightPass
    {
        public static List<Tile> NearbyTile(Lamp lamp)
        {
            List<Tile> brush = new List<Tile>();
            for (int i = 0; i < Main.tile.GetLength(0); i++)
            {
                for (int j = 0; j < Main.tile.GetLength(1); j++)
                {
                    if (Main.tile[i, j] != null && Main.tile[i, j].Active && Main.tile[i, j].solid)
                    {
                        if (Helper.Distance(Main.tile[i, j].Center, lamp.position) < lamp.range)
                        {
                            brush.Add(Main.tile[i, j]);
                        }
                    }
                }
            }
            return brush;
        }
        public static List<Background> NearbyFloor(Lamp lamp)
        {
            List<Background> brush = new List<Background>();
            for (int i = 0; i < Main.background.GetLength(0); i++)
            {
                for (int j = 0; j < Main.background.GetLength(1); j++)
                {
                    if (Main.background[i, j] != null && Main.background[i, j].active)
                    {
                        if (Helper.Distance(Main.background[i, j].Center, lamp.position) < lamp.range)
                        {
                            brush.Add(Main.background[i, j]);
                        }
                    }
                }
            }
            return brush;
        }
        public static void PreProcessing()
        {
            //  DEBUG: comment out for lighting
            //  return;
            for (int n = 0; n < Main.lamp.Length; n++)
            {
                n = Math.Min(9, n);
                Lamp lamp = Main.lamp[n];
                if (lamp == null || !lamp.active || lamp.owner != 255 || lamp.parent == null)
                    return;

                List<Background> bg = NearbyFloor(lamp);
                List<Tile> brush = NearbyTile(lamp);

                foreach (Background b in bg)
                {
                    if (b == null || !b.active)
                        continue;
                    b.preTexture = Drawing.Lightpass0(brush, b.preTexture, b.position, lamp, lamp.range);
                }
                #region one light bounce onto the tile objects
                //foreach (Tile t in brush)
                //{
                //    if (t == null || !t.Active)
                //        continue;
                //    t.preTexture = Drawing.Lightpass1(0f, new Surface(t.preTexture, t.position, t.width, t.height), Surface.GetSurface(t.hitbox, Main.background), t);
                //}
                #endregion
            }
        }
    }
    public class Surface
    {
        public Color value;
        public Bitmap bitmap;
        public Vector2 topLeft;
        public int 
            width, 
            height;
        public float range;
        public const int Range = 50;
        public Surface()
        {
        }
        public Surface(Bitmap bitmap, Vector2 topLeft, int width, int height)
        {
            this.bitmap = bitmap;
            this.topLeft = topLeft;
            this.width = width;
            this.height = height;
        }
        public static Surface[] GetSurface(Rectangle hitbox, Background[,] bg)
        {
            List<Surface> list = new List<Surface>();
            for (int i = 0; i < bg.GetLength(0); i++)
            {
                for (int j = 0; j < bg.GetLength(1); j++)
                {
                    if (bg[i, j] == null || !bg[i, j].active)
                        continue;
                    if (bg[i, j].hitbox.IntersectsWith(new Rectangle(hitbox.X - Range, hitbox.Y - Range, Range * 2, Range * 2)))
                    {
                        list.Add(new Surface() 
                        { 
                            width = bg[i, j].width,
                            height = bg[i, j].height,
                            bitmap = bg[i, j].preTexture,
                            topLeft = bg[i, j].position,
                            range = Range
                        });
                    }
                }
            }
            if (list.Count > 0)
                return list.ToArray();
            else return new Surface[] { };
        }
    }
}
