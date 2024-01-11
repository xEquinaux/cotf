using cotf.Base;
using cotf.Collections;
using cotf.World.Traps;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotf.World
{
    public struct Map : IDisposable
    {
        internal Tile[,] tile;
        internal Background[,] background;
        internal Dictionary<int, Room> room;
        internal Staircase[] staircase;
        internal Scenery[] scenery;
        internal Lamp[] lamp;
        internal Npc[] npc;
        internal Item[] item;
        internal Trap[] trap;
        internal Stash[] stash; 
        public static Map Create()
        {
            Map map = new Map();
            map.tile = Main.tile;
            map.background = Main.background;
            map.room = Main.room;
            map.staircase = Main.staircase;
            map.scenery = Main.scenery;
            map.lamp = Main.lamp;
            map.npc = Main.npc;
            map.item = Main.item;
            map.trap = Main.trap;
            map.stash = Main.stash;
            return map;
        }
        public static void Unload(Map map)
        {
            map.Dispose();
        }
        public void Dispose()
        {
            foreach (Tile item1 in tile)
            {
                item1?.Dispose();
            }
            Main.tile = null;
            foreach (Background item2 in background)
            {
                item2?.Dispose();
            }
            Main.background = null;
            Main.room.Clear();
            Array.ForEach(staircase, t => t?.Dispose());
            Array.ForEach(scenery, t => t?.Dispose());
            Array.ForEach(lamp, t => t?.Dispose());
            Array.ForEach(npc, t => t?.Dispose());
            Array.ForEach(item, t => t?.Dispose(true));
            Array.ForEach(trap, t => t?.Dispose());
        }
    }
}
