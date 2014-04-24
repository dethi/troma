using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameEngine
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
        #region Fields

        protected Game game;
        protected ContentManager content;

        public ScreenManager ScreenManager { get; internal set; }
        public ScreenState ScreenState { get; protected set; }

        protected bool hasFocus;
        public bool IsExiting { get; protected internal set; }
        public bool IsHUD { get; protected set; }

        public TimeSpan TransitionOnTime { get; protected set; }
        public TimeSpan TransitionOffTime { get; protected set; }
        public float TransitionPosition { get; protected set; }

        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        public bool IsActive
        {
            get
            {
                return hasFocus &&
                    (ScreenState == ScreenState.TransitionOn ||
                    ScreenState == ScreenState.Active);
            }
        }

        #endregion

        public GameScreen(Game game)
        {
            this.game = game;

            ScreenState = ScreenState.TransitionOn;
            IsExiting = false;
            IsHUD = false;
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;
            TransitionPosition = 1;
        }

        public virtual void LoadContent() 
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            FileManager.Content = content;
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Draw(GameTime gameTime) { }
        public virtual void HandleInput(GameTime gameTime, InputState input) { }

        public virtual void Update(GameTime gameTime, bool hasFocus,
            bool isVisible)
        {
            this.hasFocus = hasFocus;

            if (IsExiting)
            {
                ScreenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, TransitionOffTime, 1))
                    ScreenManager.RemoveScreen(this);
            }
            else if (!isVisible)
            {
                if (UpdateTransition(gameTime, TransitionOffTime, 1))
                    ScreenState = ScreenState.TransitionOff;
                else
                    ScreenState = ScreenState.Hidden;
            }
            else
            {
                if (UpdateTransition(gameTime, TransitionOnTime, -1))
                    ScreenState = ScreenState.TransitionOn;
                else
                    ScreenState = ScreenState.Active;
            }
        }

        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                    time.TotalMilliseconds);

            TransitionPosition += transitionDelta * direction;

            if (((direction < 0) && (TransitionPosition <= 0)) ||
                ((direction > 0) && (TransitionPosition >= 1)))
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return false;
            }
            else
                return true;
        }

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
                ScreenManager.RemoveScreen(this);
            else
                IsExiting = true;
        }
    }
}