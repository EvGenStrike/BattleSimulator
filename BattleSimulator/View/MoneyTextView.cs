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

    private string userText;
    private string text;
    private Vector2 textSize;

    private Vector2 userPosition;
    private Vector2 position;

    private Color color;

    public MoneyTextView(string text, Vector2 position, Color color)
    {
        userText = text;
        userPosition = position;
        this.color = color;
    }

    public void LoadContent(SpriteFont font)
    {
        TextFont = font;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        GameWindow gameWindow,
        string newText = "")
    {
        text = $"{userText}{newText}";
        textSize = TextFont.MeasureString(text);
        position = new Vector2(userPosition.X, userPosition.Y - textSize.Y);
        spriteBatch.DrawString(
            TextFont,
            text,
            position,
            color,
            0,
            Vector2.One,
            1.0f,
            SpriteEffects.None,
            0.5f
            );
    }
}
