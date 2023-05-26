using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleSimulator.View;

public class PauseMenu
{
    public MainView _MainView;
    public Button[] LevelsButtons { get; set; } = new Button[7];
    public Button ContinueButton { get; set; }
    public Button ExitButton { get; set; }

    private int fieldWidth;
    private int fieldHeight;
    
    public PauseMenu(MainView mainView, int width, int height) 
    {
        _MainView = mainView;
        fieldWidth = width;
        fieldHeight = height;
        GenerateLevelsButtons();
        GenerateContinueButton();
        GenerateExitButton();
    }

    public void GenerateLevelsButtons()
    {
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var positionX = fieldWidth - buttonWidth;
        var positionY = buttonHeight;
        for (var i = 0; i < LevelsButtons.Length; i++)
        {
            LevelsButtons[i] = new Button(
                new Vector2(positionX, positionY * (i + 1)),
                $"Level {i + 1}",
                buttonWidth,
                buttonHeight
                );
        }
    }

    public void GenerateContinueButton()
    {
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var positionX = fieldWidth / 2 - (buttonWidth / 2);
        var positionY = buttonHeight * 2;

        ContinueButton = new Button(
                new Vector2(positionX, positionY),
                "Continue",
                buttonWidth,
                buttonHeight
            );

        
    }

    public void GenerateExitButton()
    {
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var positionX = fieldWidth / 2 - (buttonWidth / 2);
        var positionY = buttonHeight * 5;

        ExitButton = new Button(
                new Vector2(positionX, positionY),
                "Exit",
                buttonWidth,
                buttonHeight
            );
    }

    public void Initialize()
    {
        foreach (var pauseLevelButton in LevelsButtons)
        {
            pauseLevelButton.Click += (sender, e) =>
            {
                _MainView.ChangeLevelTo(int.Parse(pauseLevelButton.InitialText.Last().ToString()) - 1);
            };
        }
        ContinueButton.Click += (sender, e) =>
        {
            _MainView._gameFeatures.IsGamePaused = false;
        };
        ExitButton.Click += (sender, e) =>
        {
            _MainView.Exit();
        };
    }

    public void LoadContent()
    {
        foreach (var pauseLevelButton in LevelsButtons)
        {
            var levelTextureContent = _MainView.Content.Load<Texture2D>("Button_Sample");
            var levelFontContent = _MainView.Content.Load<SpriteFont>("ButtonFont_Sample");
            pauseLevelButton.LoadContent(levelTextureContent, levelFontContent);
        }

        var continueButtonTextureContent = _MainView.Content.Load<Texture2D>("Button_Sample");
        var continueButtonFontContent = _MainView.Content.Load<SpriteFont>("ButtonFont_Sample");
        ContinueButton.LoadContent(continueButtonTextureContent, continueButtonFontContent);


        var exitButtonTextureContent = _MainView.Content.Load<Texture2D>("Button_Sample");
        var exitButtonFontContent = _MainView.Content.Load<SpriteFont>("ButtonFont_Sample");
        ExitButton.LoadContent(exitButtonTextureContent, exitButtonFontContent);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var pauseLevelButton in LevelsButtons)
        {
            pauseLevelButton.Update(gameTime);
        }
        ContinueButton.Update(gameTime);
        ExitButton.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var blackRect = new Rectangle(0, 0, fieldWidth, fieldHeight);
        spriteBatch.DrawRectangle(blackRect, Color.Black * 0.5f);

        foreach (var levelButton in LevelsButtons)
        {
            levelButton.Draw(gameTime, spriteBatch);
        }
        ContinueButton.Draw(gameTime, spriteBatch);
        ExitButton.Draw(gameTime, spriteBatch);
    }
}
