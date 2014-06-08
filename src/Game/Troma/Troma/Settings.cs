using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;
using System.Globalization;
using System.Threading;

namespace Troma
{
    public static class Settings
    {
        private static float _musicVolume;
        private static string _keyboard;
        private static string _language;
        private static bool _fullScreen;
        private static bool _vsync;
        private static bool _multisampling;

        public static float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                _musicVolume = MathHelper.Clamp(_musicVolume, 0, 2f);
                SoundManager.SetVolume(_musicVolume);
                SFXManager.SetVolume(_musicVolume);
            }
        }

        public static string Keyboard
        {
            get { return _keyboard; }
            set
            {
                _keyboard = value;
                InputConfiguration.ChangeKeyboard(_keyboard);
            }
        }

        public static string Language
        {
            get { return _language; }
            set
            {
                _language = value;

                if (_language == "Francais")
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
                else
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            }
        }

        public static bool FullScreen
        {
            get { return _fullScreen; }
            set
            {
                _fullScreen = value;
                GameServices.FullScreen(_fullScreen);
            }
        }

        public static bool Vsync
        {
            get { return _vsync; }
            set
            {
                _vsync = value;
                GameServices.Vsync(_vsync);
            }
        }

        public static bool Multisampling
        {
            get { return _multisampling; }
            set
            {
                _multisampling = value;
                GameServices.Multisampling(_multisampling);
            }
        }

        public static bool DynamicClouds { get; set; }

        public static void Initialize()
        {
            MusicVolume = App.Default.MusicVolume;
            Keyboard = App.Default.Keyboard;
            Language = App.Default.Language;

            FullScreen = App.Default.FullScreen;
            Vsync = App.Default.Vsync;
            Multisampling = App.Default.Multisampling;
            DynamicClouds = App.Default.DynamicClouds;
        }

        public static void Save()
        {
            App.Default.MusicVolume = _musicVolume;
            App.Default.Keyboard = _keyboard;
            App.Default.Language = _language;

            App.Default.FullScreen = _fullScreen;
            App.Default.Vsync = _vsync;
            App.Default.Multisampling = _multisampling;
            App.Default.DynamicClouds = DynamicClouds;

            App.Default.Save();
        }
    }
}
