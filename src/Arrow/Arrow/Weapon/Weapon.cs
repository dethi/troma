using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public abstract partial class Weapon
    {
        protected Game game;
        protected Camera camera;

        protected int nb_munition;
        protected int nb_munition_per_loader;
        protected int nb_loader;

        protected bool automatic_weapon;
        protected float rof; // Cadence de tir

        protected string sfx_shoot;
        protected string sfx_empty_loader;
        protected string sfx_reload;

        protected GameObject model;

        protected bool shoot_pressed;
        protected bool reload_pressed;

        protected double dt_last_shoot;

        protected bool empty_loader
        {
            get { return (nb_munition == 0); }
        }

        public Weapon(Game game)
        {
            this.game = game;
            this.camera = Camera.Instance;
            this.dt_last_shoot = 0;

            shoot_pressed = false;
            reload_pressed = false;
        }

        protected void Initialize()
        {
            if (automatic_weapon)
                rof = 0;

            nb_munition = nb_munition_per_loader;
            nb_loader--;

            SFXManager.AddSFX(
                sfx_shoot,
                game.Content.Load<SoundEffect>("Sounds/" + sfx_shoot));
            SFXManager.AddSFX(
                sfx_empty_loader,
                game.Content.Load<SoundEffect>("Sounds/" + sfx_empty_loader));
            SFXManager.AddSFX(
                sfx_reload,
                game.Content.Load<SoundEffect>("Sounds/" + sfx_reload));
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                #region GamePad

                GamePadState gps = GamePad.GetState(PlayerIndex.One);

                if (gps.IsButtonDown(Buttons.RightTrigger) || shoot_pressed)
                {
                    if (!shoot_pressed)
                    {
                        Shoot(gameTime);
                        shoot_pressed = true;
                    }
                    else if (gps.IsButtonUp(Buttons.RightTrigger))
                        shoot_pressed = false;
                }
                else if (gps.IsButtonDown(Buttons.X) || reload_pressed)
                {
                    if (!reload_pressed)
                    {
                        Reload();
                        reload_pressed = true;
                    }
                    else if (gps.IsButtonUp(Buttons.X))
                        reload_pressed = false;
                }

                #endregion
            }
            else
            {
                #region Mouse/Keyboard

                MouseState mouseState = Mouse.GetState();
                KeyboardState kbs = Keyboard.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed || shoot_pressed)
                {
                    if (!shoot_pressed)
                    {
                        Shoot(gameTime);
                        shoot_pressed = true;
                    }
                    else if (mouseState.LeftButton == ButtonState.Released)
                        shoot_pressed = false;
                }
                else if (kbs.IsKeyDown(KB_RELOAD) || reload_pressed)
                {
                    if (!reload_pressed)
                    {
                        Reload();
                        reload_pressed = true;
                    }
                    else if (kbs.IsKeyUp(KB_RELOAD))
                        reload_pressed = false;
                }

                #endregion
            }
        }

        //public void Draw() { }

        protected void Shoot(GameTime gameTime)
        {
            double dt_current = gameTime.TotalGameTime.TotalSeconds;

            if (empty_loader)
                SFXManager.Play(sfx_empty_loader);
            else if (IsRespectROF(dt_current))
            {
                SFXManager.Play(sfx_shoot);
                dt_last_shoot = dt_current;
                nb_munition--;
            }

        }

        protected bool IsRespectROF(double dt_current)
        {
            return automatic_weapon || (Math.Abs(dt_current - dt_last_shoot) >= rof);
        }

        protected void Reload()
        {
            if (nb_loader > 0)
            {
                SFXManager.Play(sfx_reload);
                nb_loader--;
                nb_munition = nb_munition_per_loader;
            }
        }
    }
}
