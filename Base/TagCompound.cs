using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using RUDD;
using Color = System.Drawing.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace cotf.Base
{
    public enum SaveType : byte
    {
        None = 0,
        Player = 1,
        Map = 2,
        World = 3
    }
    public sealed class TagCompound : IDisposable
    {
        public TagCompound(Entity subject)
        {
            this.subject = subject;
        }
        public TagCompound(Entity subject, SaveType type)
        {
            this.subject = subject;
            this.type = type;
        }
        private static string 
            psPath, 
            msPath;
        private Entity subject;
        private SaveType type;
        private FileStream file;
        private BinaryReader br;
        private BinaryWriter bw;
        internal static void SetPaths(string playerSavePath, string mapSavePath)   //  Called in Game.Initialize
        {
            psPath = playerSavePath;
            msPath = mapSavePath;
            if (!Directory.Exists(psPath))
            {
                Directory.CreateDirectory(psPath);
            }
            if (!Directory.Exists(msPath))
            {
                Directory.CreateDirectory(msPath);
            }
        }
        private void Init(string name)
        {
            switch (type)
            {
                default:
                case SaveType.None:
                    break;
                case SaveType.Player:
                    name = Path.Combine(psPath, name); //  Has path separator at end
                    break;
                case SaveType.Map:
                    name = Path.Combine(msPath, name);
                    //  Something similar to SaveType.Player, perhaps putting data in OS
                    //  %userprofile%\\Documents\\"My Games"
                    break;
                case SaveType.World:
                    break;
            }
            file = new FileStream(name, FileMode.OpenOrCreate);
            br = new BinaryReader(file);
            bw = new BinaryWriter(file);
        }
        public void WorldInit(string name)
        {
            name = Path.Combine(msPath, name);
            file = new FileStream(name, FileMode.OpenOrCreate);
            br = new BinaryReader(file);
            bw = new BinaryWriter(file);
        }
        private object GetValue(string tag, Type type)
        {
            object value = -1f;
            while (file.Position < file.Length)
            {
                int read = 0;
                if ((read = file.ReadByte()) != -1)
                {
                    if (Encoding.ASCII.GetString(new[] { (byte)read }).StartsWith(tag[0].ToString()))
                    {
                        byte[] buf = Encoding.ASCII.GetBytes(tag);
                        byte[] compare = new byte[buf.Length - 1];
                        string output = tag;
                        if (tag.Length > 1)
                        {
                            file.Read(compare, 0, compare.Length);
                            output = Encoding.ASCII.GetString(compare);
                            output = output.Insert(0, tag[0].ToString());
                        }
                        if (tag == output)
                        {
                            if (type == typeof(bool))
                            {
                                return value = br.ReadBoolean();
                            }
                            else if (type == typeof(byte))
                            {
                                return value = br.ReadByte();
                            }
                            else if (type == typeof(Int16))
                            {
                                return value = br.ReadInt16();
                            }
                            else if (type == typeof(Int32))
                            { 
                                return value = br.ReadInt32();
                            }
                            else if (type == typeof(UInt16))
                            {
                                return value = br.ReadUInt16();
                            }
                            else if (type == typeof(float))
                            {
                                return value = br.ReadSingle();
                            }
                            else if (type == typeof(string))
                            {
                                return value = br.ReadString();
                            }
                            else if (type == typeof(Int64))
                            { 
                                return value = br.ReadInt64();
                            }
                            else if (type == typeof(double))
                            {
                                return value = br.ReadDouble();
                            }
                            else if (type == typeof(Vector2))
                            {
                                float x = br.ReadSingle();
                                float y = br.ReadSingle();
                                return value = new Vector2(x, y);
                            }
                            else if (type == typeof(Color))
                            {
                                byte a = br.ReadByte();
                                byte r = br.ReadByte();
                                byte g = br.ReadByte();
                                byte b = br.ReadByte();
                                return value = Color.FromArgb(a, r, g, b);
                            }
                            else if (type == typeof(Purse))
                            {
                                Purse purse = new Purse(0);
                                purse.Content = new Stash();
                                purse.Content.copper = br.ReadUInt32();
                                purse.Content.silver = br.ReadInt32();
                                purse.Content.gold = br.ReadInt32();
                                purse.Content.platinum = br.ReadInt32();
                                return value = purse;
                            }
                        }
                    }
                }
            }
            return value;
        }
        private bool TagExists(string tag)
        {
            while (file.Position < file.Length)
            {
                int read = 0;
                if ((read = file.ReadByte()) != -1)
                {
                    if (Encoding.ASCII.GetString(new []{ (byte)read }).StartsWith(tag[0].ToString()))
                    {
                        byte[] buf = Encoding.ASCII.GetBytes(tag);
                        byte[] compare = new byte[buf.Length - 1];
                        string output = tag;
                        if (tag.Length > 1)
                        {
                            file.Read(compare, 0, compare.Length);
                            output = Encoding.ASCII.GetString(compare);
                            output = output.Insert(0, tag[0].ToString());
                        }
                        if (tag == output)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #region save value
        public void SaveValue(string tag, bool value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, byte value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, Int16 value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, Int32 value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, Int64 value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, UInt16 value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, Single value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, Double value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, string value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value);
            Dispose();
        }
        public void SaveValue(string tag, Vector2 value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value.X);
            bw.Write(value.Y);
            Dispose();
        }
        public void SaveValue(string tag, Color value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value.A);
            bw.Write(value.R);
            bw.Write(value.G);
            bw.Write(value.B);
            Dispose();
        }
        public void SaveValue(string tag, Purse value)
        {
            Init(subject.Name);
            if (!TagExists(tag))
            {
                //throw new Exception($"Tag, {tag}, already exists");
                bw.Write(tag);
            }
            bw.Write(value.Content.copper);
            bw.Write(value.Content.silver);
            bw.Write(value.Content.gold);
            bw.Write(value.Content.platinum);
            Dispose();
        }
        #endregion
        #region variable retrieve
        public bool GetBool(string name)
        {
            Init(subject.Name);
            bool value = false;
            try
            {
                value = (bool)GetValue(name, typeof(bool));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public byte GetByte(string name)
        {
            Init(subject.Name);
            byte value = 0;
            try
            {
                value = (byte)GetValue(name, typeof(byte));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public short GetInt16(string name)
        {
            Init(subject.Name);
            Int16 value = 0;
            try
            {
                value = (Int16)GetValue(name, typeof(Int16));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public int GetInt32(string name)
        {
            Init(subject.Name);
            Int32 value = 0;
            try
            {
                value = (Int32)GetValue(name, typeof(Int32));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public long GetInt64(string name)
        {
            Init(subject.Name);
            Int64 value = 0;
            try
            {
                value = (Int64)GetValue(name, typeof(Int64));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public float GetSingle(string name)
        {
            Init(subject.Name);
            float value = -1f;
            try
            { 
                value = (float)GetValue(name, typeof(float));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public double GetDouble(string name)
        {
            Init(subject.Name);
            double value = 0;
            try
            {
                value = (double)GetValue(name, typeof(double));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public string GetString(string name)
        {
            Init(subject.Name);
            string value = string.Empty;
            try
            {
                value = (string)GetValue(name, typeof(string));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public Vector2 GetVector2(string name)
        {
            Init(subject.Name);
            Vector2 value = Vector2.Zero;
            try
            {
                value = (Vector2)GetValue(name, typeof(Vector2));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public Color GetColor(string name)
        {
            Init(subject.Name);
            Color value = Color.Gray;
            try
            {
                value = (Color)GetValue(name, typeof(Color));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        public Stash GetStash(string name)
        {
            Init(subject.Name);
            Stash value = new Stash();
            try
            {
                value = (Stash)GetValue(name, typeof(Stash));
            }
            catch { }
            finally
            {
                Dispose();
            }
            return value;
        }
        #endregion
        public void Dispose()
        {
            br.Dispose();
            bw.Dispose();
            file.Dispose();
        }
    }
}
