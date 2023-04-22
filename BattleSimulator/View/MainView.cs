using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleSimulator.View;

public class MainView : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Field field;
    private readonly int fieldWidth;
    private readonly int fieldHeight;

    private readonly List<IEnvironmentView> environmentView;
    private readonly Dictionary<Type, ITroopView> troopsView;
    private List<ITextView> textsView;

    private List<Button> generalButtons;
    private Dictionary<ClickedTroopButtonEnum, Button> troopButtons;
    private ClickedTroopButtonEnum clickedTroopType;

    public MainView()
    {
        fieldWidth = 1920;
        fieldHeight = 1080;

        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = fieldWidth,
            PreferredBackBufferHeight = fieldHeight,
            IsFullScreen = true,
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        environmentView = new List<IEnvironmentView>
        {
            new BackgroundView()
        };
        textsView = new List<ITextView>
        {
            new MoneyTextView(
                "",
                new Vector2(fieldWidth / 100, fieldHeight),
                Color.Gold
                )
        };
        troopsView = new Dictionary<Type, ITroopView>
        {
            { typeof(Peasant), new PeasantView() },
        };
        generalButtons = new List<Button>
        {
            new Button(
                new Vector2(fieldWidth / 2, 0),
                "Start",
                Color.Black,
                fieldWidth / 10,
                fieldHeight / 10)
        };        

        troopButtons = GenerateTroopsButtons();

        field = new Field(
            fieldWidth,
            fieldHeight
            );
        field.AddAcceptableArea(new Rectangle(
            0,
            0,
            field.LineSeparator.X,
            (int)troopButtons.FirstOrDefault().Value.Position.Y)
            );

        clickedTroopType = ClickedTroopButtonEnum.None;
    }

    protected override void Initialize()
    {
        foreach (var generalButton in generalButtons)
        {
            generalButton.AddButtonEvent((sender, e) => 
            {
                generalButton.Text = "Start1";
            });
        }
        foreach (var troopButton in troopButtons.Values)
        {
            troopButton.AddButtonEvent((sender, e) =>
            {
                clickedTroopType = (ClickedTroopButtonEnum)Enum.Parse(typeof(ClickedTroopButtonEnum), troopButton.Text);
                troopButton.IsChosen = true;
                            
                foreach (var previousTroopButton in troopButtons.Values)
                {
                    previousTroopButton.IsChosen = previousTroopButton == troopButton;
                }
            });
        }
        foreach (var environmentElement in environmentView)
        {
            environmentElement.Initialize(_graphics, Window);
        }
        foreach (var sprite in troopsView.Values)
        {
            sprite.Initialize(_graphics, Window);
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var generalButton in generalButtons)
        {
            var textureContent = Content.Load<Texture2D>("Button_Sample");
            var fontContent = Content.Load<SpriteFont>("ButtonFont_Sample");
            generalButton.LoadContent(textureContent, fontContent);
        }
        foreach (var troopButton in troopButtons.Values)
        {
            var textureContent = Content.Load<Texture2D>("Button_Sample");
            var fontContent = Content.Load<SpriteFont>("ButtonFont_Sample");
            troopButton.LoadContent(textureContent, fontContent);
        }
        foreach (var environmentElement in environmentView)
        {
            var content = Content.Load<Texture2D>(environmentElement.SpriteAssetName);
            environmentElement.LoadContent(content);
        }
        foreach (var sprite in troopsView.Values)
        {
            var content = Content.Load<Texture2D>(sprite.SpriteAssetName);
            sprite.LoadContent(content);
        }
        foreach (var text in textsView)
        {
            var content = Content.Load<SpriteFont>(text.TextAssetName);
            text.LoadContent(content);
        }
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        foreach (var generalButton in generalButtons)
        {
            generalButton.Update(gameTime);
        }
        foreach (var troopButton in troopButtons.Values)
        {
            troopButton.Update(gameTime);
        }

        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            field.AddTroopEvent(() =>
            {
                switch (clickedTroopType)
                {
                    case ClickedTroopButtonEnum.None:
                        return null;
                    case ClickedTroopButtonEnum.Peasant:
                        var position = new Vector2(mouseState.X, mouseState.Y);
                        var sprite = troopsView[typeof(Peasant)].Sprite;
                        return new Peasant(position, sprite.Width, sprite.Height);
                    default:
                        return null;
                }
            });

        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        foreach (var environmentView in environmentView)
        {
            environmentView.Draw(_spriteBatch);
        }
        _spriteBatch.DrawRectangle(field.LineSeparator, Color.Red);
        DrawUnderMouseRectangle();
        foreach (var troop in field.Troops)
        {
            var viewType = troopsView[troop.GetType()];
            viewType.Draw(_spriteBatch, troop);
        }
        foreach (var text in textsView)
        {
            var newText = string.Empty;

            var textType = text.GetType();
            if (textType == typeof(MoneyTextView))
                newText = field.Money.ToString();

            text.Draw(_spriteBatch, Window, newText);
        }
        foreach (var generalButton in generalButtons)
        {
            generalButton.Draw(gameTime, _spriteBatch);
        }
        foreach (var troopButton in troopButtons.Values)
        {
            troopButton.Draw(gameTime, _spriteBatch);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private Dictionary<ClickedTroopButtonEnum, Button> GenerateTroopsButtons()
    {
        var troopsButtons = new Dictionary<ClickedTroopButtonEnum, Button>();
        var buttonWidth = fieldWidth / 10;
        var buttonHeight = fieldHeight / 10;
        var leftPositionX = fieldWidth / 5;

        var previousPosition = new Vector2(leftPositionX, fieldHeight - buttonHeight);
        foreach (var troopName in Enum.GetNames(typeof(ClickedTroopButtonEnum)))
        {
            if (troopName == "None") continue;
            troopsButtons.Add(
                (ClickedTroopButtonEnum)Enum.Parse(typeof(ClickedTroopButtonEnum), troopName),
                new Button(previousPosition, troopName, buttonWidth, buttonHeight));
            previousPosition.X += buttonWidth;
        }

        return troopsButtons;
    }

    private void DrawUnderMouseRectangle()
    {
        if (clickedTroopType == ClickedTroopButtonEnum.None) return;
        var mousePosition = Mouse.GetState().Position.ToVector2();
        var underMouseRectangle = GetUnderMouseRectangle(mousePosition);        
        var troopType = Type.GetType($"BattleSimulator.Model.{clickedTroopType}");
        var currentTroopSprite = troopsView[troopType].Sprite;
        _spriteBatch.DrawRectangle(
            underMouseRectangle,
            field.CanPlaceTroop(mousePosition, currentTroopSprite.Width, currentTroopSprite.Height)
            ? Color.Green
            : Color.Red);
    }

    private Rectangle GetUnderMouseRectangle(Vector2 mousePosition)
    {
        var size = fieldHeight / 20;
        var halfSize = size / 2;
        var x = (int)mousePosition.X;
        var y = (int)mousePosition.Y;

        return new Rectangle(x - halfSize, y - halfSize, size, size);
    }


}