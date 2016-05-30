using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MetalActionEngine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MetalText : MetalDrawableGameComponent
    {

        string _text;
        public string Text {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        internal SpriteFont Font { get; set; }

        internal Color Color { get; set; }


        public MetalText(string text):base()
        {
            Text = text;
        }
        public MetalText()
        {
        }


        internal override void SetParent(MetalDrawableGameComponent parent)
        {
            base.SetParent(parent);
         
            var size = Font.MeasureString(Text);
            Width = size.X;
            Height = size.Y;
        }
        internal void SetParent(MetalDrawableGameComponent parent, SpriteFont font, Color color)
        {
            Font = font;
            Color = color;

            SetParent(parent);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.DrawString(Font, Text, position, Color);
        }
    }
}
