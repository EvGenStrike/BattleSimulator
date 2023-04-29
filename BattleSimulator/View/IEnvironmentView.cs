using BattleSimulator.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public interface IEnvironmentView
{
    public string SpriteAssetName { get; }
    public Texture2D Sprite { get; }

    public void Initialize(
        GraphicsDeviceManager graphics,
        GameWindow gameWindow);
    public void LoadContent(Texture2D spriteTexture);
    public void Draw(
        SpriteBatch spriteBatch);
}
