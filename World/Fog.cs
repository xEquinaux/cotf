using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CirclePrefect;
using cotf.Base;
using cotf.World;
using cotf.Collections;
using cotf.Assets;
using Microsoft.Xna.Framework.Graphics;

namespace cotf.World
{
    public class Fog
    {
        public bool lit = false;
        public int size;
        public float alpha; 
        public Vector2 Center;
        public Vector2 position;
    }
    public class FogMethods
    {
        public Texture2D texture => Game.fog;
        public static void DrawEffect(SpriteBatch sb)
        {
            for (int i = 0; i < Main.background.GetLength(0); i++)
            {
                for (int j = 0; j < Main.background.GetLength(1); j++)
                {
                    var _t = Main.background[i, j];
                    if (_t != null && _t.active)
                    {
                        EffectFog(_t.onScreen, _t.position, _t.Center, sb, size: 25, range: 150f);
                    }
                }
            }
            for (int i = 0; i < Main.tile.GetLength(0); i++)
            {
                for (int j = 0; j < Main.tile.GetLength(1); j++)
                {
                    var _t = Main.tile[i, j];
                    if (_t != null && _t.Active)
                    {
                        EffectFog(_t.onScreen, _t.position, _t.Center, sb, size: 25, range: 150f);
                    }
                }
            }
        }

        private static void EffectFog(bool onScreen, CirclePrefect.Vector2 ent_Position, CirclePrefect.Vector2 ent_Center, SpriteBatch sb, float range = 100f, int size = 10, int scale = 3)
        {
            int x = (int)ent_Position.X - size + Main.ScreenX;
            int y = (int)ent_Position.Y - size + Main.ScreenY;
            float distance = (float)Main.myPlayer.Distance(ent_Center);
            if (onScreen)
            {
                for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    //float alpha = GetAlphaDynamic(ent_Center, Main.lamp.Concat(new Entity[] { Main.myPlayer }).ToArray(), range);
                    //sb.Draw(texture, new Rectangle(x + i * size, y + j * size, size * scale, size * scale), Color.Black * alpha); //Math.Min(distance / (range * scale), 1f)
                }
            }
        }

        private float Range(CirclePrefect.Vector2 to, CirclePrefect.Vector2 from, float range = 100f)
        {
            return (float)Math.Min(Helper.Distance(from, to) / (range * 3f), 1f);
        }

        public float GetAlphaDynamic(CirclePrefect.Vector2 Center, Entity[] e, float range = 100f)
        {
            //if (t != null && t.active && t.onScreen)
            //{
            //    return Range(Center, e.OrderBy(t => t.Distance(Center)).First(t => t != null && t.active).Center, range);
            //}
            return 0f;
        }
    }
}
