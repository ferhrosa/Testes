using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MetalActionEngine
{
    internal delegate void MetalMenuItemSelectedHandler(MetalMenuItem item);

    
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MetalMenuItem : MetalText
    {
        Color originalColor;


        internal int Index { get; set; }


        public Enums.MenuItemAction Action { get; set; }

        public MetalMenu ChildMenu { get; set; }


        bool _isSelected;
        internal bool IsSelected {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                if ( value )
                {
                    Selected(this);

                    originalColor = Color;
                    Color = SelectedItemColor;
                }
                else if ( originalColor != new Color() )
                {
                    Color = originalColor;
                }
            }
        }

        internal Color SelectedItemColor { get; set; }
        internal Color SelectedItemBackColor { get; set; }


        internal event MetalMenuItemSelectedHandler Selected;


        public override void Initialize()
        {
            base.Initialize();

            if ( ChildMenu != null )
            {
                ChildMenu.DrawOrder = Parent.DrawOrder;
                ChildMenu.Enabled = false;
                ChildMenu.Visible = false;
            }
        }

        internal override void SetParent(MetalDrawableGameComponent parent)
        {
            base.SetParent(parent);

            if ( ChildMenu != null )
            {
                ChildMenu.SetParent(Parent);
            }
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // TODO: Check if this is the current selected item.
        }

        public override void Draw(GameTime gameTime)
        {
            if ( IsSelected )
            {
                var backTextPosition = this.position;

                backTextPosition.X -= 1;
                backTextPosition.Y -= 1;
                SpriteBatch.DrawString(Font, Text, backTextPosition, SelectedItemBackColor);

                backTextPosition.X +=2;
                SpriteBatch.DrawString(Font, Text, backTextPosition, SelectedItemBackColor);

                backTextPosition.Y += 2;
                SpriteBatch.DrawString(Font, Text, backTextPosition, SelectedItemBackColor);

                backTextPosition.X -= 2;
                SpriteBatch.DrawString(Font, Text, backTextPosition, SelectedItemBackColor);
            }

            base.Draw(gameTime);
        }
    }
}
