﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Arrow.Map
{
    static class SFXManager
    {
        private static Dictionary<string, SoundEffect> soundEffects =
            new Dictionary<string, SoundEffect>();

        public static void AddSFX(string name, SoundEffect effect)
        {
            soundEffects.Add(name, effect);
        }

        public static void Play(string name)
        {
            if (soundEffects.ContainsKey(name))
                soundEffects[name].Play();
        }
    }
}
