using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using BattleSimulator.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BattleSimulator.View;

internal class PeasantView : ITroopView
{
    public string SpriteAssetName => "Peasant_Sample";
    public Texture2D Sprite { get; private set; }

    private Dictionary<ITroop, Color> TroopColors;

    public void Initialize(GraphicsDeviceManager graphics, GameWindow gameWindow)
    {
        TroopColors = new Dictionary<ITroop, Color>();
    }

    public void LoadContent(Texture2D spriteTexture)
    {
        Sprite = spriteTexture;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        ITroop troop)
    {
        var color = default(Color);
        if (!TroopColors.ContainsKey(troop))
        {
            color = GetTeamColor(troop.Team);
        }
        else
        {
            color = TroopColors[troop];
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
                SpriteEffects.None,
                0f
            );
    }

    public void SetColorForTroopUnderMouse(Color color, ITroop troop)
    {
        if (troop != null)      
            TroopColors[troop] = color;
        foreach (var troopColor in TroopColors)
        {
            TroopColors[troopColor.Key] = troopColor.Key == troop
                ? color
                : GetTeamColor(troopColor.Key.Team);
        }
    }

    private Color GetTeamColor(TeamEnum team)
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

}
