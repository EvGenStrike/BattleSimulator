using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public static class SpriteBatchExtensions
{
    public static void DrawRectange(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        var rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        rect.SetData(new[] { Color.White });
        spriteBatch.Draw(rect, rectangle, color);
    }
}
