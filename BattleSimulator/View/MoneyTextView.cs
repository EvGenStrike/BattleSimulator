using BattleSimulator.Model;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleSimulator.View;

internal class MoneyTextView : ITextView
{
    public string TextAssetName { get; } = "MyFont";
    public SpriteFont TextFont { get; private set; }

    private string text;
    private Vector2 textSize;
    private Vector2 position;


    public void LoadContent(SpriteFont font)
    {
        TextFont = font;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        GameWindow gameWindow,
        string newText = "")
    {
        text = $"Money : {newText}";
        textSize = TextFont.MeasureString(text);
        position = new Vector2(
            gameWindow.ClientBounds.Width / 100,
            gameWindow.ClientBounds.Height - textSize.Y
        );
        spriteBatch.DrawString(
            TextFont,
            text,
            position,
            Color.White,
            0,
            Vector2.One,
            1.0f,
            SpriteEffects.None,
            0.5f
            );
    }
}
