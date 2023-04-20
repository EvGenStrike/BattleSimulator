using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

internal class BackgroundView : ISpriteView
{
    public string SpriteAssetName { get; } = "Grass_Sample";

    public Texture2D Sprite { get; private set; }
    private Vector2 position;

    public void Initialize(GraphicsDeviceManager graphics)
    {
        position = new Vector2(
            graphics.PreferredBackBufferWidth / 2,
            graphics.PreferredBackBufferHeight / 2
        );
    }

    public void LoadContent(Texture2D spriteTexture)
    {
        Sprite = spriteTexture;
    }

    public void Draw(SpriteBatch spriteBatch, GameWindow gameWindow, ITroop troop = null)
    {
        var backgroundScale = new Vector2(
            Sprite.Width * (gameWindow.ClientBounds.Width / (gameWindow.ClientBounds.Width - Sprite.Width)),
            Sprite.Height * (gameWindow.ClientBounds.Height / (gameWindow.ClientBounds.Height - Sprite.Height))
            );
        spriteBatch.Draw(
                Sprite,
                position,
                null,
                Color.White,
                0f,
                new Vector2(Sprite.Width / 2, Sprite.Height / 2),
                backgroundScale,
                SpriteEffects.None,
                0f
            );
    }
}
