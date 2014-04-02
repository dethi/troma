using System;
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

        private Song song;
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
            song = game.Content.Load<Song>("Sounds/MenuSong");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
        }

        private void PlaySong()
        {
            MediaPlayer.Play(song);
        }

        private void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
