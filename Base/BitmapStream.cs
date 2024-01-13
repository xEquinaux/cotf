using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cotf.Base
{
    internal class BitmapStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => Buffer.Count;
        public override long Position { get; set; }

        public List<byte> Buffer = new List<byte>();

        private Dictionary<int, byte[]> lexicon = new Dictionary<int, byte[]>();
        private int index;
        public BitmapStream()
        {
        }
        public override void Flush()
        {
            throw new NotImplementedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return (buffer = Buffer.GetRange((int)Position, count).ToArray()).Length;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return Position = offset;
                case SeekOrigin.Current:
                    return Position += offset;
                case SeekOrigin.End:
                    return Position = Length - offset;
                default:
                    return Position;
            }
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] size = BitConverter.GetBytes(buffer.Length);
            Buffer.InsertRange((int)(Position += size.Length), size);
            Buffer.InsertRange((int)(Position += (buffer.Length + offset)), buffer);
            lexicon.Add(index++, buffer);
        }
        public Bitmap[] GetAllBitmaps()
        {
            Bitmap[] frames = new Bitmap[lexicon.Keys.Count];
            for (int i = 0; i < frames.Length; i++)
            {
                using (MemoryStream ms = new MemoryStream(lexicon[i]))
                {
                    frames[i] = new Bitmap(ms);
                }
            }
            return frames;
        }
        public void WriteBitmap(Bitmap bitmap, int index)
        {
            MemoryStream mem = new MemoryStream();
            bitmap.Save(mem, ImageFormat.Bmp);
            Write(mem.GetBuffer(), 0, 0);
            mem.Dispose();
        }              
        public Bitmap ReadFrame(int index)
        {
            MemoryStream mem = new MemoryStream(lexicon[index]);
            Bitmap frame = new Bitmap(mem);
            mem.Dispose();
            return frame;
        }
    }
}
