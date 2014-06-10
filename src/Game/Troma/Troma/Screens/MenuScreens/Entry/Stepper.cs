﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Stepper : IEntry
    {
        public List<string> Choices;
        private int selectedChoice;

        private Vector2 _position;

        private readonly Vector2 originPos;
        private readonly Color color;
        private readonly Color selectedColor;

        public string Text { get; set; }
        public float Scale { get; set; }
        public Vector2 ColumnsPos { get; set; }
        public EntryType Type { get; private set; }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Stepper(string text, float scale, Vector2 pos)
            : this(text, scale, pos, new List<string>())
        { }

        public Stepper(string text, float scale, Vector2 pos, List<string> choices)
        {
            Type = EntryType.Stepper;

            Text = text;
            Scale = scale;
            originPos = pos;

            color = Color.Black;
            selectedColor = Color.White;

            Choices = choices;
        }

        public void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            Color c = (isSelected) ? selectedColor : color;
            c *= screen.TransitionAlpha;

            float widthScale = (float)GameServices.GraphicsDevice.Viewport.Width / 1920;
            float heightScale = (float)GameServices.GraphicsDevice.Viewport.Height / 1080;

            _position.X = originPos.X * widthScale;
            _position.Y = originPos.Y * heightScale;

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Text, Position, c, 0,
                Vector2.Zero, Scale * (widthScale + heightScale) / 2, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Choices[selectedChoice], ColumnsPos, c, 0,
                Vector2.Zero, Scale * (widthScale + heightScale) / 2, SpriteEffects.None, 0);
        }

        public void OnChangeValue(int choice)
        {
            if (choice < 0)
            {
                selectedChoice++;

                if (selectedChoice >= Choices.Count)
                    selectedChoice = 0;
            }
            else if (choice > 0)
            {
                selectedChoice--;

                if (selectedChoice < 0)
                    selectedChoice = Choices.Count - 1;
            }
        }
    }
}
