using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using BattleSimulator.Model;

namespace BattleSimulator.View;

internal class PeasantView : ISpriteView
{
    public string SpriteAssetName => "Peasant_Sample";
    public Texture2D Sprite { get; private set; }

    public void Initialize(GraphicsDeviceManager graphics, GameWindow gameWindow)
    {
        //position = new Vector2(
        //    graphics.PreferredBackBufferWidth / 2,
        //    graphics.PreferredBackBufferHeight / 2
        //);
    }

    public void LoadContent(Texture2D spriteTexture)
    {
        Sprite = spriteTexture;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        ITroop troop = null)
    {
        spriteBatch.Draw(
                Sprite,
                troop.CurrentPosition,
                null,
                Color.White,
                0f,
                new Vector2(Sprite.Width / 2, Sprite.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
    }
}
