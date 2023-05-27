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

public class IntroductionMenu
{
    public MainView _MainView;
    public Button ContinueButton { get; set; }
    public TextView IntroductionText { get; set; }
    public bool IsFirstLaunch = true;

    private int fieldWidth;
    private int fieldHeight;

    public IntroductionMenu(MainView mainView)
    {
        _MainView = mainView;
        fieldWidth = mainView.fieldWidth;
        fieldHeight = mainView.fieldHeight;

        GenerateContinueButton();
        GenerateCongratulationsText();
    }


    public void GenerateContinueButton()
    {
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var positionX = fieldWidth / 2 - (buttonWidth / 2);

        ContinueButton = new Button(
                new Vector2(positionX, 0),
                "Continue",
                buttonWidth,
                buttonHeight
            );


    }

    public void GenerateCongratulationsText()
    {
        IntroductionText = new(
            @"
Left corner - your money,
Click LMB on the down button to choose unit and then place it
Click RMB to delete chosen unit,
Press TAB to reset current level
Press ESC to Pause the game
In the pause menu you can choose between any level
",
            new Vector2(fieldWidth / 2, fieldHeight / 2 - fieldHeight / 10),
            Color.White
            );
    }

    public void Initialize()
    {
        ContinueButton.Click += (sender, e) =>
        {
            _MainView.ChangeLevelTo(_MainView.Levels.IndexOf(_MainView.currentField) + 1);
            IsFirstLaunch = false;
        };
    }

    public void LoadContent()
    {
        var continueButtonTextureContent = _MainView.Content.Load<Texture2D>("Button_Upgraded_Sample");
        var continueButtonFontContent = _MainView.Content.Load<SpriteFont>("ButtonFont_Sample");
        ContinueButton.LoadContent(continueButtonTextureContent, continueButtonFontContent);

        var content = _MainView.Content.Load<SpriteFont>(IntroductionText.TextAssetName);
        IntroductionText.LoadContent(content);
    }

    public void Update(GameTime gameTime)
    {
        ContinueButton.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var blackRect = new Rectangle(0, 0, fieldWidth, fieldHeight);
        spriteBatch.DrawRectangle(blackRect, Color.Black * 0.5f);

        ContinueButton.Draw(gameTime, spriteBatch);
        IntroductionText.Draw(spriteBatch, _MainView.Window);
    }
}
