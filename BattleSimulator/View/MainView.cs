using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static BattleSimulator.Model.Field;

namespace BattleSimulator.View;

public class MainView : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameFeatures _gameFeatures;

    private List<Field> levels;
    private Field field;
    private readonly int fieldWidth;
    private readonly int fieldHeight;

    private readonly List<IEnvironmentView> environmentView;
    private readonly Dictionary<Type, ITroopView> troopsView;
    private List<ITextView> textsView;
    private List<Rectangle> rectanglesView;
    private PauseMenu pauseMenu;

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
            //IsFullScreen = true,
        };
        _gameFeatures = new((Game)this);

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
                new Vector2(fieldWidth / 2 - (fieldWidth / 20), 0),
                "Start",
                Color.Black,
                fieldWidth / 10,
                fieldHeight / 10)
        };
        troopButtons = GenerateTroopsButtons();
        var troopButtonY = (int)troopButtons.FirstOrDefault().Value.Position.Y;

        rectanglesView = new List<Rectangle>
        {
            new Rectangle(0, troopButtonY, fieldWidth, fieldHeight - troopButtonY),
            new Rectangle(0, 0, fieldWidth, generalButtons[0].Height),
        };

        pauseMenu = new(fieldWidth, fieldHeight);
        pauseMenu.GenerateLevelsButtons();

        var middleLineSeparator = GenerateLineSeparator();
        var leftAcceptableArea = new Rectangle(
            0,
            rectanglesView[1].Height,
            middleLineSeparator.X,
            troopButtonY - rectanglesView[1].Height);
        levels = new List<Field>
        {
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 600)),
                    new Peasant(TeamEnum.Blue, new Vector2(1350, 450)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 330)),
                    new Peasant(TeamEnum.Blue, new Vector2(1250, 210)),
                },
                250
            ),

            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 600)),
                },
                1000
            )
        };


        field = levels[0];

    }

    //Вызовет после атаки юнита
    private void FieldEventTroopSuccessfulAttack(object sender, TroopAttackHandler args)
    {
        //потом заменю на анимацию
        //if (args.SuccessfullAttack)
        //{
        //    var troopsGeneralView = this.troopsView[typeof(Peasant)];
        //    var troopViewData = troopsGeneralView.TroopsData[args.troop];
        //    if (troopViewData.SpentTime < 0.5f)
        //    {
        //        troopsGeneralView.SetColor(args.troop, Color.White);
        //        troopViewData.SpentTime
        //            += (float)args.gameTime.ElapsedGameTime.TotalSeconds;
        //    }
        //    else
        //    {
        //        troopViewData.SpentTime = 0;
        //        troopsGeneralView.SetColor(
        //            args.troop, troopsGeneralView.GetTeamColor(args.troop.Team)
        //            );
        //        args.SuccessfullAttack = false;
        //    }
        //}
    }

    private void FieldEventTroopFailedAttack(object sender, ITroop troop)
    {
        var troopsView = this.troopsView[typeof(Peasant)];
        troopsView.SetColor(troop, troopsView.GetTeamColor(troop.Team));
    }

    protected override void Initialize()
    {
        _gameFeatures.escPress += _gameFeatures.OnEscPressed;
        clickedTroopType = ClickedTroopButtonEnum.None;
        field.TroopSuccessfulAttack += FieldEventTroopSuccessfulAttack;
        field.TroopFailedAttack += FieldEventTroopFailedAttack;
        foreach (var generalButton in generalButtons)
        {
            generalButton.Click += (sender, e) =>
            {
                field.ChangeGameState(GameStateEnum.Started);
            };
        }
        foreach (var troopButton in troopButtons.Values)
        {
            troopButton.Click += (sender, e) =>
            {
                clickedTroopType = (ClickedTroopButtonEnum)Enum.Parse(typeof(ClickedTroopButtonEnum), troopButton.InitialText);
                troopButton.IsChosen = true;

                foreach (var previousTroopButton in troopButtons.Values)
                {
                    previousTroopButton.IsChosen = previousTroopButton == troopButton;
                }
            };
        }
        foreach (var pauseLevelButton in pauseMenu.LevelsButtons)
        {
            pauseLevelButton.Click += (sender, e) =>
            {
                ChangeLevelTo(int.Parse(pauseLevelButton.InitialText.Last().ToString()) - 1);
            };
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
        foreach (var pauseLevelButton in pauseMenu.LevelsButtons)
        {
            var textureContent = Content.Load<Texture2D>("Button_Sample");
            var fontContent = Content.Load<SpriteFont>("ButtonFont_Sample");
            pauseLevelButton.LoadContent(textureContent, fontContent);
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

    bool areEnemyTroopsDrawnOnTheStart;
    GameStateEnum previousGameState = GameStateEnum.ArrangingTroops;
    bool flag;
    protected override void Update(GameTime gameTime)
    {
        _gameFeatures.TryPauseGame();
        if (_gameFeatures.IsGamePaused)
        {
            foreach (var pauseLevelButton in pauseMenu.LevelsButtons)
            {
                pauseLevelButton.Update(gameTime);
            }
            if (!flag)
            {
                previousGameState = field.GameState;
                flag = true;
            }
            field.ChangeGameState(GameStateEnum.Paused);
            return;
        }
        else
        {
            if (previousGameState != GameStateEnum.None && flag) field.ChangeGameState(previousGameState);
            flag = false;
            previousGameState = GameStateEnum.None;
        }


        if (!areEnemyTroopsDrawnOnTheStart)
        {
            foreach (var enemyTroop in field.EnemyTroops)
            {
                var troopView = troopsView[typeof(Peasant)];
                var sprite = troopView.Sprite;
                var troop = enemyTroop.OverrideTroop(
                            enemyTroop.Team, enemyTroop.InitialPosition, sprite.Width, sprite.Height);
                field.AddTroopEvent(() => troop);
            }
            areEnemyTroopsDrawnOnTheStart = true;
        }

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

                        var troop = new Peasant(
                            TeamEnum.Red,
                            new Vector2(
                                position.X - sprite.Width / 2,
                                position.Y - sprite.Height / 2
                                ),
                            sprite.Width,
                            sprite.Height,
                            gameTime);
                        return troop;
                    default:
                        return null;
                }
            });
        }
        else if (mouseState.RightButton == ButtonState.Pressed)
        {
            field.RemoveTroop(mouseState.Position.ToVector2());
        }

        if (field.GameState == GameStateEnum.Started)
        {
            field.PlayGame(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        foreach (var environmentView in environmentView)
        {
            environmentView.Draw(_spriteBatch);
        }
        if (field.GameState == GameStateEnum.ArrangingTroops)
        {
            _spriteBatch.DrawRectangle(field.LineSeparator, Color.Red);
            DrawUnderMouseRectangle();
            foreach (var troop in field.Troops)
            {
                var viewType = troopsView[troop.GetType()];
                var troopBehindMouse =
                    field.GetTroopByPosition(Mouse.GetState().Position.ToVector2());
                viewType.SetColorForTroopUnderMouse(Color.Gray, troopBehindMouse);
                viewType.Draw(_spriteBatch, troop);
            }
            foreach (var rectangle in rectanglesView)
            {
                _spriteBatch.DrawRectangle(rectangle, Color.Black * 0.5f);
            }
            foreach (var generalButton in generalButtons)
            {
                generalButton.Draw(gameTime, _spriteBatch);
            }
            foreach (var troopButton in troopButtons.Values)
            {
                troopButton.Draw(gameTime, _spriteBatch);
            }
            foreach (var text in textsView)
            {
                var money = string.Empty;

                var textType = text.GetType();
                if (textType == typeof(MoneyTextView))
                    money = field.Money.ToString();

                text.Draw(_spriteBatch, Window, money);
            }
        }
        else if (field.GameState == GameStateEnum.Started)
        {
            foreach (var troop in field.Troops)
            {
                var viewType = troopsView[troop.GetType()];
                viewType.Draw(_spriteBatch, troop);
            }
        }
        else if (field.GameState == GameStateEnum.Paused)
        {
            DrawPauseMenu(gameTime);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public Rectangle GenerateLineSeparator()
    {
        var width = fieldWidth / 100;
        var height = fieldHeight;
        var x = (fieldWidth / 2) - width / 2;
        var y = 0;
        return new Rectangle(x, y, width, height);
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
                new Button(previousPosition, troopName, buttonWidth, buttonHeight, new Peasant().Cost.ToString()));
            previousPosition.X += buttonWidth;
        }

        return troopsButtons;
    }

    private void DrawUnderMouseRectangle()
    {
        if (clickedTroopType == ClickedTroopButtonEnum.None) return;
        var mousePosition = Mouse.GetState().Position.ToVector2();
        var troopType = Type.GetType($"BattleSimulator.Model.{clickedTroopType}");
        var currentTroopSprite = troopsView[troopType].Sprite;
        var underMouseRectangle = GetUnderMouseRectangle(mousePosition, currentTroopSprite);
        _spriteBatch.DrawRectangle(
            underMouseRectangle,
            field.CanPlaceTroop(mousePosition, currentTroopSprite.Width, currentTroopSprite.Height)
            ? Color.Green
            : Color.Red);
    }

    private Rectangle GetUnderMouseRectangle(Vector2 mousePosition, Texture2D chosenTroopSprite)
    {
        var halfWidth = chosenTroopSprite.Width / 2;
        var halfHeight = chosenTroopSprite.Height / 2;
        var x = (int)mousePosition.X;
        var y = (int)mousePosition.Y;

        return new Rectangle(
            x - halfWidth, y - halfHeight, chosenTroopSprite.Width, chosenTroopSprite.Height
            );
    }

    private void ChangeLevelTo(int id)
    {

        field = levels[id];
        areEnemyTroopsDrawnOnTheStart = false;
        _gameFeatures.IsGamePaused = false;
        field.ResetField();
        previousGameState = GameStateEnum.ArrangingTroops;
    }

    private void DrawPauseMenu(GameTime gameTime)
    {
        var blackRect = new Rectangle(0, 0, fieldWidth, fieldHeight);
        _spriteBatch.DrawRectangle(blackRect, Color.Black * 0.5f);

        foreach (var levelButton in pauseMenu.LevelsButtons)
        {
            levelButton.Draw(gameTime, _spriteBatch);
        }
    }
}