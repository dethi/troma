﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Arrow
{
    public class MenuSong
    {
        private Game game;
        private Song bgSong;
        private bool songPlayed;

        public bool SongPlayed
        {
            get { return songPlayed; }
            set
            {
                if (value && !songPlayed)
                {
                    PlaySong();
                    songPlayed = value;
                }
                else if (!value && songPlayed)
                {
                    StopSong();
                    songPlayed = value;
                }
            }
        }

        public MenuSong(Game game)
        {
            this.game = game;
        }

        public void LoadContent()
        {
            bgSong = game.Content.Load<Song>("Sounds/BgSong");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
        }

        private void PlaySong()
        {
            MediaPlayer.Play(bgSong);
        }

        private void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}