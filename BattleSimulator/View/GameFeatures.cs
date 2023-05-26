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
    public bool IsGamePaused {get; private set;}
    public EventHandler escPress;

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

        if (currentKB.IsKeyDown(Keys.Escape)) escPress?.Invoke(this, new EventArgs());
        if (currentKB.IsKeyUp(Keys.P) && previousKB.IsKeyDown(Keys.P))
        {
            IsGamePaused = !IsGamePaused;
        }

        return IsGamePaused;
    }

    public void OnEscPressed(object sender, EventArgs eventArgs)
    {
        _Game.Exit();
    }
}
