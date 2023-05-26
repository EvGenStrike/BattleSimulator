using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace BattleSimulator.View;

public class LoseMenu
{
    public MainView _MainView;
    public Button TryAgainButton { get; set; }
    public Button ExitButton { get; set; }
    public TextView LoseText { get; set; }

    private int fieldWidth;
    private int fieldHeight;

    public LoseMenu(MainView mainView)
    {
        _MainView = mainView;
        fieldWidth = mainView.fieldWidth;
        fieldHeight = mainView.fieldHeight;
        GenerateTryAgainButton();
        GenerateCongratulationsText();
        GenerateExitButton();
    }

    public void GenerateTryAgainButton()
    {
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var positionX = fieldWidth / 2 - (buttonWidth / 2);
        var positionY = buttonHeight * 2;

        TryAgainButton = new Button(
                new Vector2(positionX, positionY),
                "Try again",
                buttonWidth,
                buttonHeight
            );
    }

    public void GenerateCongratulationsText()
    {
        LoseText = new(
            "You've lost!",
            new Vector2(fieldWidth / 2, fieldHeight / 2 - fieldHeight / 10),
            Color.Red
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
        TryAgainButton.Click += (sender, e) =>
        {
            _MainView.ChangeLevelTo(_MainView.Levels.IndexOf(_MainView.currentField));
        };
        ExitButton.Click += (sender, e) =>
        {
            _MainView.Exit();
        };
    }

    public void LoadContent()
    {

        var continueButtonTextureContent = _MainView.Content.Load<Texture2D>("Button_Sample");
        var continueButtonFontContent = _MainView.Content.Load<SpriteFont>("ButtonFont_Sample");
        TryAgainButton.LoadContent(continueButtonTextureContent, continueButtonFontContent);


        var exitButtonTextureContent = _MainView.Content.Load<Texture2D>("Button_Sample");
        var exitButtonFontContent = _MainView.Content.Load<SpriteFont>("ButtonFont_Sample");
        ExitButton.LoadContent(exitButtonTextureContent, exitButtonFontContent);

        var content = _MainView.Content.Load<SpriteFont>(LoseText.TextAssetName);
        LoseText.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        TryAgainButton.Update(gameTime);
        ExitButton.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var blackRect = new Rectangle(0, 0, fieldWidth, fieldHeight);
        spriteBatch.DrawRectangle(blackRect, Color.Black * 0.5f);

        TryAgainButton.Draw(gameTime, spriteBatch);
        LoseText.Draw(spriteBatch, _MainView.Window);
        ExitButton.Draw(gameTime, spriteBatch);
    }
}
