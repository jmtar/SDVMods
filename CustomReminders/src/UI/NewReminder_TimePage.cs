﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace Dem1se.CustomReminders.UI
{

    /// <summary>
    /// Second page of the menu that sets the reminder's time
    /// </summary>
    public class NewReminder_TimePage : IClickableMenu
    {
        // title and the error message
        private readonly List<ClickableComponent> Labels = new();

        private readonly int XPos = (int)(Game1.viewport.Width * Game1.options.zoomLevel * (1 / Game1.options.uiScale)) / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
        private readonly int YPos = (int)(Game1.viewport.Height * Game1.options.zoomLevel * (1 / Game1.options.uiScale)) / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
        private readonly int UIWidth = 632 + IClickableMenu.borderWidth * 2;
        private readonly int UIHeight = 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize;

        private readonly List<ClickableTextureComponent> MinutesAndMeridiemList = new();
        private readonly List<ClickableTextureComponent> HoursButtons = new();
        private ClickableComponent CurrentChoiceDisplay;
        private ClickableTextureComponent OkButton;

        private int Hours = 0;
        private string Minutes;
        private string Meridiem;

        /// <summary>The callback to invoke when the ok button is pressed.</summary>
        private readonly Action<int> OnChanged;

        /// <summary>The callback function that gets called when ok button is pressed</summary>
        public NewReminder_TimePage(Action<int> onChange)
        {
            base.initialize(XPos, YPos, UIWidth, UIHeight);
            OnChanged = onChange;
            SetupPositions();
        }

        private void SetupPositions()
        {
            // Ok button
            OkButton = new ClickableTextureComponent("OK", new Rectangle(xPositionOnScreen + width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, yPositionOnScreen + height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), "", null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f);

            // Titles
            Labels.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 1 + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + 8, 1, 1), Utilities.Globals.Helper.Translation.Get("new-reminder.reminder-time")));
            Labels.Add(new ClickableComponent(new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 1 + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 2 - Game1.tileSize / 8 + 8, 1, 1), "", "error"));

            // Current Choice
            CurrentChoiceDisplay = new ClickableComponent(new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 1 + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize - Game1.tileSize / 8 + 8, 1, 1), "CurrentTimeDisplay");

            // AM or PM
            MinutesAndMeridiemList.Add(new ClickableTextureComponent("00", new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 6, Game1.tileSize * 2 + 16, Game1.tileSize + 8), "", "", Utilities.Globals.Helper.Content.Load<Texture2D>("assets/00.png", ContentSource.ModFolder), new Rectangle(), (int)(Game1.pixelZoom * 0.75f)));
            MinutesAndMeridiemList.Add(new ClickableTextureComponent("30", new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 7 + 8, Game1.tileSize * 2 + 16, Game1.tileSize + 8), "", "", Utilities.Globals.Helper.Content.Load<Texture2D>("assets/30.png", ContentSource.ModFolder), new Rectangle(), (int)(Game1.pixelZoom * 0.75f)));

            // Minutes
            MinutesAndMeridiemList.Add(new ClickableTextureComponent("AM", new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4 + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 6, Game1.tileSize * 2 + 16, Game1.tileSize + 8), "", "", Utilities.Globals.Helper.Content.Load<Texture2D>("assets/AM.png", ContentSource.ModFolder), new Rectangle(), (int)(Game1.pixelZoom * 0.75f)));
            MinutesAndMeridiemList.Add(new ClickableTextureComponent("PM", new Rectangle(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4 + 4, yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 7 + 8, Game1.tileSize * 2 + 16, Game1.tileSize + 8), "", "", Utilities.Globals.Helper.Content.Load<Texture2D>("assets/PM.png", ContentSource.ModFolder), new Rectangle(), (int)(Game1.pixelZoom * 0.75f)));

            // Hour
            // first row: 1-6
            for (int i = 1; i <= 6; i++)
            {
                HoursButtons.Add(new ClickableTextureComponent($"{i}", new Rectangle(xPositionOnScreen + width / 2 - (int)(Game1.tileSize * 5.0f) + (int)(Game1.tileSize * (i * 1.25f)), yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 3, Game1.tileSize + 16, Game1.tileSize + 16), "", "", Utilities.Globals.Helper.Content.Load<Texture2D>($"assets/hourButtons/{i}HourButton.png", ContentSource.ModFolder), new Rectangle(), Game1.pixelZoom));
            }
            // second row: 7-12
            for (int i = 7; i <= 12; i++)
            {
                HoursButtons.Add(new ClickableTextureComponent($"{i}", new Rectangle(xPositionOnScreen + width / 2 - (int)(Game1.tileSize * 5.0f) + (int)(Game1.tileSize * ((i - 6) * 1.25f)), yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 4 + Game1.tileSize / 4, Game1.tileSize + 16, Game1.tileSize + 16), "", "", Utilities.Globals.Helper.Content.Load<Texture2D>($"assets/hourButtons/{i}HourButton.png", ContentSource.ModFolder), new Rectangle(), Game1.pixelZoom));
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            // The 12, hour buttons
            foreach (ClickableTextureComponent hourButton in HoursButtons)
            {
                if (hourButton.containsPoint(x, y))
                {
                    Game1.playSound("coin");
                    hourButton.scale -= 0.25f;
                    Hours = Convert.ToInt32(hourButton.name);
                    hourButton.scale = Math.Max(0.75f, Game1.pixelZoom);
                }
            }

            // AM/PM and 00/30 buttons
            foreach (ClickableTextureComponent button in MinutesAndMeridiemList)
            {
                if (button.containsPoint(x, y))
                {
                    Game1.playSound("coin");
                    button.scale -= 0.25f;
                    if (button.name.EndsWith("M"))
                    {
                        Meridiem = button.name;
                    }
                    else if (button.name.EndsWith("0"))
                    {
                        Minutes = button.name;
                    }
                    button.scale = Math.Max(0.75f, Game1.pixelZoom * 0.75f);
                }
            }

            // ok button
            if (OkButton.containsPoint(x, y))
            {
                if (IsOkButtonReady())
                {
                    Game1.playSound("coin");
                    OkButton.scale -= 0.25f;
                    int reminderTime = Convert.ToInt32($"{Hours}{Minutes}");
                    if (Meridiem == "PM")
                        reminderTime += 1200;
                    OnChanged(reminderTime);
                    OkButton.scale = Math.Max(0.75f, OkButton.scale);
                    Game1.exitActiveMenu();
                }
            }

            // The error message label to say invalid time is chosen
            if (Hours != 0 && !string.IsNullOrEmpty(Minutes) && !string.IsNullOrEmpty(Meridiem))
            {
                if (!IsValidTime())
                {
                    Labels[1].name = Utilities.Globals.Helper.Translation.Get("new-reminder.invalid-time");
                }
                else
                {
                    Labels[1].name = "";
                }
            }
        }

        private bool IsValidTime()
        {
            int reminderTime = Convert.ToInt32($"{Hours}{Minutes}");
            if (Meridiem == "PM")
                reminderTime += 1200;
            if (reminderTime <= 2600 && reminderTime >= 600)
                return true;
            else
                return false;
        }

        public override void performHoverAction(int x, int y)
        {
            OkButton.scale = OkButton.containsPoint(x, y) && IsOkButtonReady()
                ? Math.Min(OkButton.scale + 0.02f, OkButton.baseScale + 0.1f)
                : Math.Max(OkButton.scale - 0.02f, OkButton.baseScale);

            foreach (ClickableTextureComponent button in HoursButtons)
            {
                button.scale = button.containsPoint(x, y)
                    ? Math.Min(button.scale + 0.02f, button.baseScale + 0.1f)
                    : Math.Max(button.scale - 0.02f, button.baseScale);
            }

            foreach (ClickableTextureComponent button in MinutesAndMeridiemList)
            {
                button.scale = button.containsPoint(x, y)
                    ? Math.Min(button.scale + 0.02f, button.baseScale + 0.1f)
                    : Math.Max(button.scale - 0.02f, button.baseScale);
            }
        }

        private bool IsOkButtonReady()
        {
            if (Hours != -1 && Meridiem != null && Minutes != null)
            {
                int reminderTime = Convert.ToInt32($"{Hours}{Minutes}");
                if (Meridiem == "PM")
                    reminderTime += 1200;
                if (reminderTime <= 2600 && reminderTime >= 600)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            xPositionOnScreen = Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
            yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
            SetupPositions();
        }

        public override void draw(SpriteBatch b)
        {
            // supress the Menu button
            Utilities.Globals.Helper.Input.Suppress(Utilities.Globals.MenuButton);

            // draw screen fade
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);

            // draw menu box
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, false, true);

            // draw CurrentChoiceDisplay
            if (Hours == 0 && string.IsNullOrEmpty(Minutes) && string.IsNullOrEmpty(Meridiem))
            {
                CurrentChoiceDisplay.name = Utilities.Globals.Helper.Translation.Get("new-reminder.instruction");
            }
            else
            {
                CurrentChoiceDisplay.name = $"{Hours}:{Minutes} {Meridiem}";
            }
            Utility.drawTextWithShadow(b, CurrentChoiceDisplay.name, Game1.smallFont, new Vector2(CurrentChoiceDisplay.bounds.X, CurrentChoiceDisplay.bounds.Y), Color.Black);

            // draw the 12 hour buttons
            foreach (ClickableTextureComponent button in HoursButtons)
            {
                button.draw(b);
            }

            // draw title and error message labels
            foreach (ClickableComponent label in Labels)
            {
                if (label.label == "error")
                    Utility.drawTextWithShadow(b, label.name, Game1.dialogueFont, new Vector2(label.bounds.X, label.bounds.Y), Color.Red);
                else
                    Utility.drawTextWithShadow(b, label.name, Game1.dialogueFont, new Vector2(label.bounds.X, label.bounds.Y), Color.Black);
            }

            // draw AM/PM and Minutes buttons
            foreach (ClickableTextureComponent button in MinutesAndMeridiemList)
            {
                button.draw(b);
            }

            // draw OK button
            if (IsOkButtonReady())
                OkButton.draw(b);
            else
            {
                OkButton.draw(b);
                OkButton.draw(b, Color.Black * 0.5f, 0.97f);
            }

            // draw cursor
            drawMouse(b);
        }
    }
}