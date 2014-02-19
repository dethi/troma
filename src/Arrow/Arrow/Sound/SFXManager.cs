using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Arrow
{
    static class SFXManager
    {
        private static Dictionary<string, SoundEffect> soundEffects =
            new Dictionary<string, SoundEffect>();
        private static double lastTime = 0;

        /// <summary>
        /// Adds sound effect
        /// </summary>
        public static void AddSFX(string name, SoundEffect effect)
        {
            soundEffects.Add(name, effect);
        }

        /// <summary>
        /// Play sound effect
        /// </summary>
        public static void Play(string name, GameTime gameTime)
        {
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;

            /*if (soundEffects.ContainsKey(name) &&
                (lastTime + soundEffects[name].Duration.TotalMilliseconds <= currentTime))
            {*/
                soundEffects[name].Play();
                lastTime = currentTime;
            //}
        }
    }
}
