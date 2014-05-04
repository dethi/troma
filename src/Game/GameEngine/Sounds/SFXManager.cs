using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine
{
    public static class SFXManager
    {
        private static Dictionary<string, SoundEffect> soundEffects =
            new Dictionary<string, SoundEffect>();

        public static void Add(string name, SoundEffect sound)
        {
            soundEffects.Add(name, sound);
        }

        public static void Play(string name)
        {
            if (soundEffects.ContainsKey(name))
                soundEffects[name].Play();
        }
    }
}
