using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public class PauseMenu
{
    public Button[] LevelsButtons { get; set; } = new Button[7];

    private int fieldWidth;
    private int fieldHeight;
    
    public PauseMenu(int width, int height) 
    {
        fieldWidth = width;
        fieldHeight = height;
    }

    public Button[] GenerateLevelsButtons()
    {
        var troopsButtons = new Dictionary<ClickedTroopButtonEnum, Button>();
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var positionX = fieldWidth - buttonWidth;
        var positionY = buttonHeight;

        var previousPosition = new Vector2(positionX, fieldHeight - buttonHeight);
        for (var i = 0; i < LevelsButtons.Length; i++)
        {
            LevelsButtons[i] = new Button(
                new Vector2(positionX, positionY * (i + 1)),
                $"Level {i + 1}",
                buttonWidth,
                buttonHeight
                );
            previousPosition.X += buttonWidth;
        }

        return LevelsButtons;
    }
}
