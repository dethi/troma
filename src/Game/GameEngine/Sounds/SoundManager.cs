using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine
{
    public static class SoundManager
    {
        private static AudioEngine audioEngine;
        private static WaveBank waveBank;
        private static SoundBank soundBank;
        public static Cue CurrentSong { get; private set; }

        public static void Initialize()
        {
            string root = GameServices.Game.Content.RootDirectory;

            audioEngine = new AudioEngine(root + "/Sounds/Audio.xgs");
            waveBank = new WaveBank(audioEngine, root + "/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, root + "/Sounds/Sound Bank.xsb");
        }

        public static void Update()
        {
            audioEngine.Update();
        }

        public static void SetVolume(float musicVolume)
        {
            audioEngine.GetCategory("Music").SetVolume(musicVolume);
        }

        public static void Play(string sound)
        {
            CurrentSong = soundBank.GetCue(sound);
            CurrentSong.Play();
        }

        public static void Resume()
        {
            CurrentSong.Resume();
        }

        public static void Pause()
        {
            CurrentSong.Pause();
        }

        public static void Stop()
        {
            CurrentSong.Stop(AudioStopOptions.AsAuthored);
        }
    }
}
