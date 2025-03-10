﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.Menus;

namespace Dem1se.CustomReminders.UI
{
    class CheckBox
    {
        public int PosX, PosY;
        public bool IsChecked = false;
        public float Scale = 3.5f;
        public ClickableTextureComponent CheckBox_Box, CheckBox_Check;

        public CheckBox(int x, int y)
        {
            PosX = x;
            PosY = y;
            SetupUI();
        }

        private void SetupUI()
        {
            CheckBox_Box = new ClickableTextureComponent(
                new Rectangle(PosX, PosY, (int)(18 * Scale), (int)(18 * Scale)),
                Utilities.Globals.Helper.Content.Load<Texture2D>("assets/CheckBox_Box.png", ContentSource.ModFolder),
                new Rectangle(),
                Scale
            );

            CheckBox_Check = new ClickableTextureComponent(
                new Rectangle(PosX + (int)(5 * Scale), PosY + (int)(6 * Scale), (int)(8 * Scale), (int)(7 * Scale)),
                Utilities.Globals.Helper.Content.Load<Texture2D>("assets/CheckBox_Check.png", ContentSource.ModFolder),
                new Rectangle(),
                Scale
            );

        }

        public void receiveLeftClick(int x, int y)
        {
            if (CheckBox_Box.containsPoint(x, y))
                IsChecked = !IsChecked;
        }

        public void draw(SpriteBatch b)
        {
            CheckBox_Box.draw(b);
            if (IsChecked)
                CheckBox_Check.draw(b);
        }
    }
}
