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

    private Texture2D sprite;
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
        sprite = spriteTexture;
    }

    public void Draw(SpriteBatch spriteBatch, GameWindow gameWindow)
    {
        var backgroundScale = new Vector2(
            sprite.Width * (gameWindow.ClientBounds.Width / (gameWindow.ClientBounds.Width - sprite.Width)),
            sprite.Height * (gameWindow.ClientBounds.Height / (gameWindow.ClientBounds.Height - sprite.Height))
            );
        spriteBatch.Draw(
                sprite,
                position,
                null,
                Color.White,
                0f,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                backgroundScale,
                SpriteEffects.None,
                0f
            );
    }
}
