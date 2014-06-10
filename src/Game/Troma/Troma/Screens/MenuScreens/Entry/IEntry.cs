using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Troma
{
    public enum EntryType
    {
        Button,
        Stepper,
        Switch
    }

    public interface IEntry
    {
        string Text { get; set; }
        Vector2 Position { get; set; }
        float Scale { get; set; }
        Vector2 ColumnsPos { get; set; }
        EntryType Type { get; }


        void Draw(GameTime gameTime, MenuScreen screen, bool isSelected);
    }
}
