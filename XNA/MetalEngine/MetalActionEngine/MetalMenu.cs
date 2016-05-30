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
using System.Windows;


namespace MetalActionEngine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MetalMenu : MetalDrawableGameComponent
    {
        const string DefaultTitleFontName = "Calibri_18";
        const string DefaultItemsFontName = "Calibri_14";


        MetalText title;

        SpriteFont itemsFont;
        Color itemsColor;

        MetalMenuItem selectedItem;


        Keys? pressedKey = null;
        TimeSpan lastStep;
        bool passedFirstStep;


        bool ignoreEnterPress;


        public int TitleAreaHeight { get; set; }

        public string TitleText { get; set; }

        public string TitleFontName { get; set; }
        public string ItemsFontName { get; set; }

        public string TitleColorExpression { get; set; }
        public string ItemsColorExpression { get; set; }
        public string SelectedItemColorExpression { get; set; }
        public string SelectedItemBackColorExpression { get; set; }


        private List<MetalMenuItem> _items;

        public List<MetalMenuItem> Items
        {
            get
            {
                if ( _items == null )
                    _items = new List<MetalMenuItem>();

                return _items;
            }
        }


        internal override void SetParent(MetalGame game)
        {
            base.SetParent(game);
        }
        internal void SetParent(MetalMenu menu)
        {
            base.SetParent(menu);
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();

            if ( !String.IsNullOrWhiteSpace(TitleText) )
            {
                title = new MetalText(TitleText);

                if ( String.IsNullOrWhiteSpace(TitleFontName) )
                    TitleFontName = (Parent is MetalMenu ? ((MetalMenu)Parent).TitleFontName : DefaultTitleFontName);

                title.Font = Game.Content.Load<SpriteFont>(TitleFontName);

                title.Color = Utilities.ColorFromString(TitleColorExpression, Color.Yellow);

                title.SetParent(this);
                title.DrawOrder = this.DrawOrder + 1;
                title.Visible = this.Visible;

                if ( TitleAreaHeight == 0 && Parent is MetalMenu )
                    TitleAreaHeight = ((MetalMenu)Parent).TitleAreaHeight;

                if ( String.IsNullOrWhiteSpace(ItemsFontName) )
                    ItemsFontName = (Parent is MetalMenu ? ((MetalMenu)Parent).ItemsFontName : DefaultItemsFontName);

                title.CenterHorizontally();
                title.CenterVertically(Y, Y + TitleAreaHeight);
            }

            itemsFont = Game.Content.Load<SpriteFont>(ItemsFontName);
            itemsColor = Utilities.ColorFromString(ItemsColorExpression, Color.White);

            foreach ( var item in Items )
            {
                item.SetParent(this, itemsFont, itemsColor);
                item.DrawOrder = this.DrawOrder + 1;
                item.Index = Items.IndexOf(item);

                item.SelectedItemColor = Utilities.ColorFromString(SelectedItemColorExpression, new Color(0, 50, 50));
                item.SelectedItemBackColor = Utilities.ColorFromString(SelectedItemColorExpression, Color.Cyan);

                item.Visible = this.Visible;

                item.CenterHorizontally();
                item.CenterVertically(TitleAreaHeight + (item.Index * item.Height * 1.5f), TitleAreaHeight + (item.Index * item.Height * 1.5f) + item.Height);

                //item.Initialize();

                // Sets the event handler that is executed when the selected item is changed;
                item.Selected += new MetalMenuItemSelectedHandler(item_Selected);
            }

            // If this group has items and no one is selected, set the first item as selected.
            if ( Items.Count > 0 && !Items.Any(item => item.IsSelected) )
                Items.First().IsSelected = true;

            // Attach the method to the event only when all items are loaded;
            VisibleChanged += new EventHandler<EventArgs>(MetalMenuGroup_VisibleChanged);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboardState = Keyboard.GetState();

            if ( keyboardState.IsKeyDown(Keys.Down) )
            {
                if ( !pressedKey.HasValue || pressedKey == Keys.Down )
                {
                    bool changeSelectedItem = false;

                    if ( !pressedKey.HasValue )
                        changeSelectedItem = true;
                    else if ( passedFirstStep && gameTime.TotalGameTime.Subtract(lastStep).Milliseconds >= 200 )
                        changeSelectedItem = true;
                    else if ( !passedFirstStep && gameTime.TotalGameTime.Subtract(lastStep).TotalMilliseconds >= 500 )
                        changeSelectedItem = true;

                    if ( changeSelectedItem )
                    {
                        if ( selectedItem.Index == Items.Count - 1 )
                            Items.First().IsSelected = true;
                        else
                            Items[selectedItem.Index + 1].IsSelected = true;

                        lastStep = gameTime.TotalGameTime;

                        if ( pressedKey.HasValue )
                            passedFirstStep = true;
                    }
                }

                pressedKey = Keys.Down;
            }
            else if ( keyboardState.IsKeyDown(Keys.Up) )
            {
                if ( !pressedKey.HasValue || pressedKey == Keys.Up )
                {
                    bool changeSelectedItem = false;

                    if ( !pressedKey.HasValue )
                        changeSelectedItem = true;
                    else if ( passedFirstStep && gameTime.TotalGameTime.Subtract(lastStep).Milliseconds >= 200 )
                        changeSelectedItem = true;
                    else if ( !passedFirstStep && gameTime.TotalGameTime.Subtract(lastStep).TotalMilliseconds >= 500 )
                        changeSelectedItem = true;

                    if ( changeSelectedItem )
                    {
                        if ( selectedItem.Index == 0 )
                            Items.Last().IsSelected = true;
                        else
                            Items[selectedItem.Index - 1].IsSelected = true;

                        lastStep = gameTime.TotalGameTime;

                        if ( pressedKey.HasValue )
                            passedFirstStep = true;
                    }
                }

                pressedKey = Keys.Up;
            }
            else if ( pressedKey.HasValue )
            {
                pressedKey = null;
                passedFirstStep = false;
            }

            if ( !ignoreEnterPress && keyboardState.IsKeyDown(Keys.Enter) )
            {
                if ( selectedItem.ChildMenu != null )
                {
                    Deactivate();

                    selectedItem.ChildMenu.Activate();
                }
                else
                {
                    switch ( selectedItem.Action )
                    {
                        // Execute the actions to start a new game.
                        case Enums.MenuItemAction.StartGame:
                            Game.StartGame();
                            break;

                        // Execute the actions to go back to the parent menu.
                        case Enums.MenuItemAction.BackToParentMenu:
                            if ( Parent is MetalMenu )
                            {
                                Deactivate();

                                ((MetalMenu)Parent).Activate();
                            }
                            break;

                        // Execute the actions to go exit the game (the application!).
                        case Enums.MenuItemAction.ExitGame:
                            Game.Exit();
                            break;
                    }
                }
            }
            else if ( ignoreEnterPress && keyboardState.IsKeyUp(Keys.Enter) )
            {
                ignoreEnterPress = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // TODO: Draw generic things specific to this menu.
        }


        /// <summary>
        /// Activates the menu.
        /// </summary>
        internal void Activate()
        {
            // Enables and shows the menu.
            Enabled = true; // Allow the execution of the Update method.
            Visible = true; // Allow the execution of the Visible method.

            // Defines a value that is checked when the Enter key is pressed.
            ignoreEnterPress = true;
        }

        /// <summary>
        /// Deactivates the menu.
        /// </summary>
        internal void Deactivate()
        {
            // Disable and hides the menu.
            Enabled = false; // Deny the execution of the Update method.
            Visible = false; // Deny the execution of the Visible method.
        }


        void item_Selected(MetalMenuItem item)
        {
            selectedItem = item;

            foreach ( var otherItem in Items.Where(i => i != item) )
                otherItem.IsSelected = false;
        }

        void MetalMenuGroup_VisibleChanged(object sender, EventArgs e)
        {
            title.Visible = this.Visible;
            Items.ForEach(item => item.Visible = this.Visible);
        }
    }
}
