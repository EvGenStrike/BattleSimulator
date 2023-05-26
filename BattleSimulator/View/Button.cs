using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BattleSimulator.View;

public class Button : Component
{
    #region Fields

    private MouseState _currentMouse;

    private SpriteFont _font;

    private bool _isHovering;

    private MouseState _previousMouse;

    private Texture2D _texture;

    #endregion

    #region Properties

    public event EventHandler Click;

    public bool Clicked { get; private set; }

    public bool IsChosen { get; set; }

    public Color PenColour { get; set; }

    public Vector2 Position { get; set; }

    public int Width { get; }

    public int Height { get; }

    public Rectangle Rectangle
    {
        get
        {
            //return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
    }

    public string Text { get; set; }
    public string InitialText { get; set; }
    public string HoveredText { get; set; }

    public Button(Vector2 position, string text, int width, int height, string hoveredText = null)
    {
        Position = position;
        Text = text;
        InitialText = text;
        HoveredText = hoveredText ?? text;
        PenColour = Color.Black;
        Width = width;
        Height = height;
    }

    public Button(Vector2 position, string text, Color color, int width, int height, string hoveredText = null)
    {
        Position = position;
        Text = text;
        InitialText = text;
        HoveredText = hoveredText ?? text;
        PenColour = color;
        Width = width;
        Height = height;
    }

    public void LoadContent(Texture2D textureContent, SpriteFont fontContent)
    {
        _texture = textureContent;

        _font = fontContent;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var colour = Color.White;

        Text = _isHovering ? HoveredText : InitialText;
        if (_isHovering)
        {
            colour = Color.Gray;
        }
        else if (IsChosen)
        {
            colour = Color.DarkGray;
        }

        spriteBatch.Draw(_texture, Rectangle, colour);

        if (!string.IsNullOrEmpty(Text))
        {
            var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
            var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

            spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
        }
    }

    public void Update(GameTime gameTime)
    {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();

        var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

        _isHovering = false;

        if (mouseRectangle.Intersects(Rectangle))
        {
            _isHovering = true;

            if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(this, new EventArgs());
            }
        }
    }
    #endregion
}