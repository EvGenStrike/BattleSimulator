using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public interface ITroopView
{
    public string SpriteAssetName { get; }
    public string HitSoundName { get; }
    public Texture2D Sprite { get; }
    public Dictionary<ITroop, ViewData> TroopsData { get; }
    public Song HitSound { get; }
    public Color HurtColor { get; }

    public void Initialize(
        GraphicsDeviceManager graphics,
        GameWindow gameWindow);
    public void LoadContent(Texture2D spriteTexture, Song hitSound);
    public void Draw(
        SpriteBatch spriteBatch,
        ITroop troop);
    public void SetColorForTroopUnderMouse(Color color, ITroop troop);
    public void SetColor(ITroop troop, Color color);
    public Color GetTeamColor(TeamEnum team);
    public Color GetHurtColor(TeamEnum team);
}
