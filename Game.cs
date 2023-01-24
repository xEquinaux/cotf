using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace cotf
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphicsMngr;
        private SpriteBatch _spriteBatch;
        private Viewport _viewport
        {
            get { return GraphicsDevice.Viewport; }
            set { GraphicsDevice.Viewport = value; }
        }
        static BufferedGraphicsContext context = BufferedGraphicsManager.Current;

        private int _portX => _viewport.X;
        private int _portY => _viewport.Y;
        private Rectangle _size => new Rectangle(0, 0, _bounds.Width, _bounds.Height);
        private Size _oldBounds;
        private Size _bounds;

        private static Point _position;
        private static Point _oldPosition;
        public static Point Position => _position;
        public static Camera CAMERA = new Camera();

        public Game()
        {
            _graphicsMngr = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _Initialize();
            { 
                _bounds = new Size(800, 600);
                new Main();
                Settings();
            }
            base.Initialize();
        }

        protected void Resize(object sender, EventArgs e)
        {
            _graphicsMngr.PreferredBackBufferWidth = _bounds.Width;
            _graphicsMngr.PreferredBackBufferHeight = _bounds.Height;
            _graphicsMngr.ApplyChanges();
        }

        protected override bool BeginDraw()
        {
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied);
            { 
                _position = Window.Position;
                if (_oldBounds != _bounds || _oldPosition != _position)
                {
                    ResizeEvent.Invoke(this, new EventArgs());
                }
                _oldBounds = _bounds;
                _oldPosition = _position;
            }

            return base.BeginDraw();
        }

        protected override void EndDraw()
        {
            _spriteBatch.End();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void LoadContent()
        {
            LoadResources();
            {
                _viewport = new Viewport(_portX, _portY, 800, 600);
                ResizeEvent += Resize;
            }
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (Main.KeyPressTimer == 0)
                {
                    Main.KeyPressTimer++;
                    if (!Main.open)
                    {
                        Exit();
                    }
                }
            }
            else
            {
                Main.KeyPressTimer = 0;
            }
            Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            using (Bitmap bmp = new Bitmap(_bounds.Width, _bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bmp))
                using (BufferedGraphics buffered = context.Allocate(graphics, new System.Drawing.Rectangle(0, 0, _bounds.Width, _bounds.Height)))
                {
                    SetQuality(buffered.Graphics, new System.Drawing.Rectangle(0, 0, _bounds.Width, _bounds.Height));
                    buffered.Graphics.Clear(System.Drawing.Color.CornflowerBlue);
                    { 
                        this.Camera(buffered.Graphics, CAMERA);
                        this.PreDraw(buffered.Graphics);
                        this.Draw(buffered.Graphics);
                        this.TitleScreen(buffered.Graphics);
                    }
                    buffered.Render();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(stream, ImageFormat.Png);
                    using (Texture2D surface = Texture2D.FromStream(_graphicsMngr.GraphicsDevice, stream))
                    {
                        _spriteBatch.Draw(surface, _size, Color.White);
                    }
                }
            }

            base.Draw(gameTime);
        }

        protected void Settings()
        {
            this.Window.Title = "cotf";
            this.Window.IsBorderless = true;
            this.Window.AllowUserResizing = true;
            this.Window.AllowAltF4 = false;
        }
        
        #region events
        public static event EventHandler<EventArgs> ResizeEvent;
        public static event EventHandler<InitializeArgs> InitializeEvent;
        public static event EventHandler<EventArgs> LoadResourcesEvent;
        public static event EventHandler<DrawingArgs> MainMenuEvent;
        public static event EventHandler<PreDrawArgs> PreDrawEvent;
        public static event EventHandler<DrawingArgs> DrawEvent;
        public static event EventHandler<UpdateArgs> UpdateEvent;
        public static event EventHandler<CameraArgs> CameraEvent;
        public class DrawingArgs : EventArgs
        {
            public Graphics graphics;
        }
        public class PreDrawArgs : EventArgs
        {
            public Graphics graphics;
        }
        public class UpdateArgs : EventArgs
        {
        }
        public class CameraArgs : EventArgs
        {
            public Graphics graphics;
            public Camera CAMERA;
        }
        public class InitializeArgs : EventArgs
        {
        }
        #endregion
        #region methods
        private void LoadResources()
        {
            LoadResourcesEvent?.Invoke(this, new EventArgs());
        }
        private void _Initialize()
        {
            InitializeEvent?.Invoke(this, new InitializeArgs());
        }
        private void TitleScreen(Graphics graphics)
        {
            MainMenuEvent?.Invoke(this, new DrawingArgs() 
            { 
                graphics = graphics
            });
        }
        private void PreDraw(Graphics graphics)
        {
            PreDrawEvent?.Invoke(this, new PreDrawArgs()
            {
                graphics = graphics
            });
        }
        private void Draw(Graphics graphics)
        {
            DrawEvent?.Invoke(this, new DrawingArgs() 
            { 
                graphics = graphics
            });
        }
        private void Update()
        {
            UpdateEvent?.Invoke(this, new UpdateArgs());
        }
        private void Camera(Graphics graphics, Camera CAMERA)
        {
            CameraEvent?.Invoke(this, new CameraArgs() { graphics = graphics, CAMERA = CAMERA });
        }
        #endregion
        #region quality settings
        public CompositingQuality compositingQuality = CompositingQuality.AssumeLinear;
        public CompositingMode compositingMode = CompositingMode.SourceOver;
        public InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic;
        public TextRenderingHint textRenderHint = TextRenderingHint.ClearTypeGridFit;
        public GraphicsUnit graphicsUnit = GraphicsUnit.Pixel;
        public SmoothingMode smoothingMode = SmoothingMode.Default;
        private void SetQuality(Graphics graphics, System.Drawing.Rectangle bounds)
        {
            graphics.CompositingQuality = compositingQuality;
            graphics.CompositingMode = compositingMode;
            graphics.InterpolationMode = interpolationMode;
            graphics.TextRenderingHint = textRenderHint;
            //graphics.RenderingOrigin = new Point(bounds.X, bounds.Y);
            //graphics.Clip = new System.Drawing.Region(bounds);
            graphics.PageUnit = graphicsUnit;
            graphics.SmoothingMode = smoothingMode;
        }
        #endregion
    }
    public class Camera
    {
        public CirclePrefect.Vector2 oldPosition;
        public CirclePrefect.Vector2 position;
        public CirclePrefect.Vector2 velocity;
        public bool isMoving => velocity != CirclePrefect.Vector2.Zero || oldPosition != position;
        public bool follow = false;
        public bool active = false;
    }
}