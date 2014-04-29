using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public static class XConsole
    {
        #region Fields

        private static List<Func<GameTime, string>> _masterList = new List<Func<GameTime, string>>();
        private static List<Func<GameTime, string>> _currentList = new List<Func<GameTime, string>>();

        private static string _console = "";
        private static SpriteFont _spriteFont;
        private static Vector2 _position;
        private static Vector2 _size;

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes the manager
        /// </summary>
        public static void Initialize()
        {
            Clear();
            _spriteFont = FileManager.Load<SpriteFont>("Fonts/Debug");
            NbVects.Reset();

            AddDebug(SysDebug.Debug);
            AddDebug(NbVects.Debug);
        }

        #endregion

        #region Update and Draw

        public static void Update(GameTime gameTime)
        {
            Clear();
            _currentList.Clear();
            _currentList.AddRange(_masterList);

            foreach (Func<GameTime, string> func in _currentList)
                _console += func(gameTime) + "\n";

            if (_console.EndsWith("\n"))
                _console = _console.Remove(_console.Length - 1);

            _size = _spriteFont.MeasureString(_console);
        }

        public static void DrawHUD(GameTime gameTime)
        {
            _position = new Vector2(5,
                GameServices.GraphicsDevice.Viewport.Height - _size.Y - 5);

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.DrawString(_spriteFont, _console, _position, Color.Gold);
            GameServices.SpriteBatch.End();
        }

        #endregion

        #region XConsole Methods

        /// <summary>
        /// Clear the console
        /// </summary>
        public static void Clear()
        {
            _console = "";
        }

        #endregion

        #region Manage Methods

        public static void AddDebug(Func<GameTime, string> func)
        {
            _masterList.Add(func);
        }

        public static void AddDebug(IEnumerable<Func<GameTime, string>> func)
        {
            _masterList.AddRange(func);
        }

        public static bool Contains(Func<GameTime, string> func)
        {
            return _masterList.Contains(func);
        }

        public static void Remove(Func<GameTime, string> func)
        {
            _masterList.Remove(func);
            _currentList.Remove(func);
        }

        /// <summary>
        /// Clear all the lists in the manager.
        /// </summary>
        public static void Reset()
        {
            _masterList.Clear();
            _currentList.Clear();
        }

        #endregion
    }
}
