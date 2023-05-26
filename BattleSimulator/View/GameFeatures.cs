using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public class GameFeatures
{
    public bool IsGamePaused {get; set;}
    public EventHandler<GameStateEnum> escPress;

    private Game _Game;

    public GameFeatures(Game game)
    {
        _Game = game;
    }

    KeyboardState currentKB, previousKB;
    public bool TryPauseGame()
    {
        previousKB = currentKB;
        currentKB = Keyboard.GetState();


        if ((currentKB.IsKeyUp(Keys.P) && previousKB.IsKeyDown(Keys.P))
            || (currentKB.IsKeyUp(Keys.Escape) && previousKB.IsKeyDown(Keys.Escape)))
        {
            IsGamePaused = !IsGamePaused;
        }

        return IsGamePaused;
    }

    public void OnEscPressed(object sender, GameStateEnum gameState)
    {
        if (gameState == GameStateEnum.Paused)
        {
            //_Game.Exit();
        }
        else
        {
            IsGamePaused = true;
        }
    }
}
