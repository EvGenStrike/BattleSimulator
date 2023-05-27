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

internal class BackgroundView : IEnvironmentView
{
    public string SpriteAssetName { get; } = "Cobblestone_Background";
    public Texture2D Sprite { get; private set; }

    private GameWindow gameWindow;
    private Vector2 position;

    public void Initialize(GraphicsDeviceManager graphics, GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
        position = new Vector2(
            graphics.PreferredBackBufferWidth / 2,
            graphics.PreferredBackBufferHeight / 2
        );
    }

    public void LoadContent(Texture2D spriteTexture)
    {
        Sprite = spriteTexture;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //var backgroundScale = new Vector2(
        //    Sprite.Width * (gameWindow.ClientBounds.Width / (gameWindow.ClientBounds.Width - Sprite.Width)),
        //    Sprite.Height * (gameWindow.ClientBounds.Height / (gameWindow.ClientBounds.Height - Sprite.Height))
        //    );
        spriteBatch.Draw(
                Sprite,
                position,
                null,
                Color.White,
                0f,
                new Vector2(Sprite.Width / 2, Sprite.Height / 2),
                1,
                SpriteEffects.None,
                0f
            );
    }
}
