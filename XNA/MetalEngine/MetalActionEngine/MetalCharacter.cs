using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using MetalActionEngine.Enums;

namespace MetalActionEngine
{
    public class MetalCharacter : MetalDrawableGameComponent
    {

        Texture2D sprite;
        SpriteEffects spriteEffects;
        byte frameNumber;
        Rectangle frame;
        TimeSpan lastFrameChange;


        internal Direction Direction { get; private set; }
        internal CharacterState State { get; private set; }


        internal string Name { get; set; }

        public override float Width
        {
            get
            {
                return frame.Width;
            }
            set
            {
                frame.Width = (int)value;
            }
        }

        public override float Height
        {
            get
            {
                return frame.Height;
            }
            set
            {
                frame.Height = (int)value;
            }
        }

        public byte StandingFrames { get; set; }
        public byte WalkingFrames { get; set; }

        public short StandingDelay { get; set; }
        public short WalkingDelay { get; set; }



        internal void SetParent(MetalGame game, string name)
        {
            this.Name = name;

            base.SetParent(game);
        }

        public override void Initialize()
        {
            base.Initialize();

            // Loads the character 
            sprite = Game.Content.Load<Texture2D>(String.Format("Characters/{0}", Name));

            // Sets the current frame of the animation to 1.
            frameNumber = 1;

            // Sets the default character direction to Right.
            Direction = Direction.None;

            CenterHorizontally();
            CenterVertically();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateState(gameTime);

            UpdateFrame(gameTime);
        }

        private void UpdateState(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if ( keyboardState.IsKeyDown(Keys.Left) )
                Direction = Direction.Left;
            else if ( keyboardState.IsKeyDown(Keys.Right) )
                Direction = Direction.Right;
            else
                Direction = Direction.None;

            if ( Direction == Direction.None )
            {
                State = CharacterState.Standing;
            }
            else
            {
                State = CharacterState.Walking;

                if ( Direction == Direction.Right )
                    spriteEffects = SpriteEffects.None;
                else
                    spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        private void UpdateFrame(GameTime gameTime)
        {
            switch ( State )
            {
                case CharacterState.Standing:
                    frame.Y = 0;

                    if ( StandingFrames > 1 )
                    {
                        if ( gameTime.TotalGameTime.Subtract(lastFrameChange).Milliseconds >= StandingDelay )
                            frameNumber++;

                        if ( frameNumber > StandingFrames )
                            frameNumber = 1;

                        frame.X = (frameNumber - 1) * (int)Width;
                    }
                    else
                    {
                        frame.X = 0;
                    }

                    break;

                case CharacterState.Walking:
                    frame.Y = (int)Height; // Would be "(int)Height * 1", but that's not necessary...

                    if ( WalkingFrames > 1 )
                    {
                        if ( gameTime.TotalGameTime.Subtract(lastFrameChange).Milliseconds >= WalkingDelay )
                        {
                            frameNumber++;
                            lastFrameChange = gameTime.TotalGameTime;
                        }

                        if ( frameNumber > WalkingFrames )
                            frameNumber = 1;

                        frame.X = (frameNumber - 1) * (int)Width;
                    }
                    else
                    {
                        frame.X = 0;
                    }

                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Draw(sprite, position, frame, Color.White, 0, Vector2.Zero, 1, spriteEffects, 0);
        }

    }
}
