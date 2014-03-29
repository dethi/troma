using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden
    }

    public abstract class GameScreen
    {
        public ScreenManager ScreenManager;
        public ScreenState ScreenState { get; private set; }

        public bool IsExiting;

        public GameScreen()
        {
            ScreenState = ScreenState.TransitionOn;
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Draw(GameTime gameTime) { }
    }
}