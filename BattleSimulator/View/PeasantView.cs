using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BattleSimulator.View;

internal class PeasantView : ISpriteView
{
    public string SpriteAssetName => "Peasant_Sample";

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
        spriteBatch.Draw(
                sprite,
                position,
                null,
                Color.White,
                0f,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
    }
}
