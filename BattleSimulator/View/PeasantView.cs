using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using BattleSimulator.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Xna.Framework.Media;

namespace BattleSimulator.View;

internal class PeasantView : ITroopView
{
    public string SpriteAssetName => "Peasant_Sample";
    public string HitSoundName => "Peasant_Hit";
    public Texture2D Sprite { get; private set; }
    public Song HitSound { get; private set; }
    public Dictionary<ITroop, ViewData> TroopsData { get; private set; }
    public Color HurtColor { get; private set; }

    

    public void Initialize(GraphicsDeviceManager graphics, GameWindow gameWindow)
    {
        TroopsData = new();
    }

    public void LoadContent(Texture2D spriteTexture, Song hitSound)
    {
        Sprite = spriteTexture;
        HitSound = hitSound;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        ITroop troop)
    {
        TroopsData.TryAdd(troop, new());
        var color = default(Color);
        if (!TroopsData.ContainsKey(troop))
        {
            color = GetTeamColor(troop.Team);
        }
        else
        {
            color = TroopsData[troop].Color;
        }

        spriteBatch.Draw(
                Sprite,
                troop.CurrentPosition,
                null,
                color,
                0f,
                //new Vector2(Sprite.Width / 2, Sprite.Height / 2),
                Vector2.One,
                Vector2.One,
                troop.Team == TeamEnum.Red
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None,
                0f
            );
    }

    public void SetColor(ITroop troop, Color color)
    {
        TroopsData.TryAdd(troop, new());
        TroopsData[troop].Color = color;
    }

    public void SetColorForTroopUnderMouse(Color color, ITroop troop)
    {
        if (troop != null)
        {
            TroopsData.TryAdd(troop, new());
            TroopsData[troop].Color = color;
        }
        foreach (var troopColor in TroopsData)
        {
            TroopsData[troopColor.Key].Color = troopColor.Key == troop
                ? color
                : GetTeamColor(troopColor.Key.Team);
        }
    }

    public Color GetTeamColor(TeamEnum team)
    {
        switch (team)
        {
            case TeamEnum.Red:
                return Color.Red;
            case TeamEnum.Blue:
                return Color.Blue;
            default:
                return Color.White;
        }
    }

    public Color GetHurtColor(TeamEnum team)
    {
        switch (team)
        {
            case TeamEnum.Red:
                return Color.IndianRed;
            case TeamEnum.Blue:
                return Color.CornflowerBlue;
            default:
                return Color.White;
        }
    }

}
