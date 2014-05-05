using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine
{
    public static class SFXManager
    {
        private static float _volume = 1;
        private static Dictionary<string, SoundEffect> soundEffects =
            new Dictionary<string, SoundEffect>();

        public static void Add(string name, SoundEffect sound)
        {
            soundEffects.Add(name, sound);
        }

        public static void SetVolume(float volume)
        {
            _volume = volume;
        }

        public static void Play(string name)
        {
            if (soundEffects.ContainsKey(name))
                soundEffects[name].Play(MathHelper.Clamp(_volume, 0f, 1f), 0, 0);
        }

        public static void Play(string name, float volume, float pan)
        {
            if (soundEffects.ContainsKey(name))
                soundEffects[name].Play(MathHelper.Clamp(volume * _volume, 0f, 1f), 0, pan);
        }
    }
}
