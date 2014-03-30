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


namespace Arrow
{
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        // Prelaod any assets using by UI rendering
        static readonly string[] preloadAssets =
        {
        };

        #endregion

        /*
        #if EDITOR_MODE

        private EntityPos entityPos;
        private int currentEntity;
        private int maxEntity;
        private DisplayPosObject HUDPosObj;
        
        #endif

        private MenuPause menuPause;
        private MenuStart menuStart;
         * */

        #region Initialization

        /// <summary>
        /// The main game constructor
        /// </summary>
        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            //ActivateFullScreen();
            //DisableVsync();

            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new GameplayScreen(this));

            #if !BUILD

            screenManager.AddScreen(new DebugScreen(this));

            #endif
        }

        /*
        protected override void Initialize()
        {
            #if EDITOR_MODE

            HUDPosObj = new DisplayPosObject(this);
            Components.Add(HUDPosObj);
            
            entityPos = new EntityPos(input);

            #endif

            menuPause = new MenuPause(this);
            menuPause.Initialize();

            menuStart = new MenuStart(this);
            menuStart.Initialize();

            base.Initialize();
        }
        */

        /// <summary>
        /// Load graphics content
        /// </summary>
        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
                Content.Load<object>(asset);

            SFXManager.AddSFX("Walk", Content.Load<SoundEffect>("Sounds/Walk"));
            SFXManager.AddSFX("Run", Content.Load<SoundEffect>("Sounds/Run"));

            /*
            #if EDITOR_MODE

            currentEntity = 0;
            maxEntity = entities.Entities.Count - 1;
            HUDPosObj.AssociateModel(entities);

            #endif

            #region Menu

            menuPause.LoadContent();
            menuStart.LoadContent();

            #endregion
             * */
        }

        #endregion

        /*
        protected override void Update(GameTime gameTime)
        {
            
            #if EDITOR_MODE

            entityPos.Change_i(ref currentEntity, maxEntity);
            entities.MoveEntity(new Vector4(
                entityPos.Change_x_z(entities.Entities.ElementAt(currentEntity).Value.position.Translation),
                currentEntity));
            HUDPosObj.Upieme(currentEntity);

            #endif

            if (menuStart.GameStart)
            {
                if (!menuPause.DisplayMenu)
                    //player.Update(gameTime, mapManager);
                menuPause.Update(gameTime);
            }
            else
                menuStart.Update(gameTime);
            

            base.Update(gameTime);
        }
         * */


        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.DarkOliveGreen);
            graphics.GraphicsDevice.Clear(Color.Black);
            
            /*
            if (menuStart.GameStart)
            {
                menuPause.Draw(gameTime);
            }
            else
                menuStart.Draw(gameTime);
            */

            base.Draw(gameTime);
        }

        #endregion
    }
}
