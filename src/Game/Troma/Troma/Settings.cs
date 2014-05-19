﻿using System;
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

        public static void Initialize()
        {
            _musicVolume = 0.8f;
            Keyboard = "AZERTY";
            Language = "Francais";

            FullScreen = true;
            Vsync = true;
            Multisampling = false;
        }
    }
}
