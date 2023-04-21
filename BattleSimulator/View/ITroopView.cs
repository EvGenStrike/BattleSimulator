using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public interface ITroopView
{
    public string SpriteAssetName { get; }
    public Texture2D Sprite { get; }

    public void Initialize(
        GraphicsDeviceManager graphics,
        GameWindow gameWindow);
    public void LoadContent(Texture2D spriteTexture);
    public void Draw(
        SpriteBatch spriteBatch,
        ITroop troop = null);
}
