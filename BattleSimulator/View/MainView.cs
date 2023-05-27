using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using static BattleSimulator.Model.Field;

namespace BattleSimulator.View;

public class MainView : Game
{
    public GameFeatures _gameFeatures { get; private set; }
    public GraphicsDeviceManager _graphics { get; private set; }
    public SpriteBatch _spriteBatch { get; private set; }

    public List<Field> Levels { get; private set; }
    public Field currentField { get; private set; }
    public readonly int fieldWidth;
    public readonly int fieldHeight;

    private readonly List<IEnvironmentView> environmentView;
    private readonly Dictionary<Type, ITroopView> troopsView;
    private List<ITextView> textsView;
    private List<Rectangle> rectanglesView;
    private Song BackgroundMusic;

    private PauseMenu pauseMenu;
    private WinMenu winMenu;
    private LoseMenu loseMenu;
    private IntroductionMenu introductionMenu;

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
        _gameFeatures = new((Game)this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        environmentView = new List<IEnvironmentView>
        {
            new BackgroundView()
        };
        textsView = new List<ITextView>
        {
            new TextView(
                "",
                new Vector2(fieldWidth / 20, fieldHeight - fieldHeight / 30),
                Color.Gold
                )
        };
        troopsView = new Dictionary<Type, ITroopView>
        {
            { typeof(Peasant), new PeasantView() },
            { typeof(Boxer), new BoxerView() },
            { typeof(Zombie), new ZombieView() },
            { typeof(Archer), new ArcherView() },
            { typeof(GoblinGiant), new GoblinGiantView() },
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

        pauseMenu = new(this);
        winMenu = new(this);
        loseMenu = new(this);
        introductionMenu = new(this);

        var middleLineSeparator = GenerateLineSeparator();
        var leftAcceptableArea = new Rectangle(
            0,
            rectanglesView[1].Height,
            middleLineSeparator.X,
            troopButtonY - rectanglesView[1].Height);
        Levels = new List<Field>
        {
            //1
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
            
            //2
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 900)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 800)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 700)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 600)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 500)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 400)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 300)),
                    new Peasant(TeamEnum.Blue, new Vector2(1300, 200)),
                },
                350
            ),

            //3
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 130)),
                    new Zombie(TeamEnum.Blue, new Vector2(1300, 800)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 500)),
                    new Peasant(TeamEnum.Blue, new Vector2(1530, 500)),
                    new Peasant(TeamEnum.Blue, new Vector2(1660, 500)),
                    new Zombie(TeamEnum.Blue, new Vector2(1300, 300)),
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 930)),
                },
                400
            ),

            //4
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Boxer(TeamEnum.Blue, new Vector2(1200, 300)),
                    new Boxer(TeamEnum.Blue, new Vector2(1500, 300)),
                    new Boxer(TeamEnum.Blue, new Vector2(1200, 700)),
                    new Boxer(TeamEnum.Blue, new Vector2(1500, 700)),
                    new Archer(TeamEnum.Blue, new Vector2(1600, 450)),
                    new Zombie(TeamEnum.Blue, new Vector2(1700, 200)),
                    new Zombie(TeamEnum.Blue, new Vector2(1700, 800)),
                },
                750
            ),

            //5
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Archer(TeamEnum.Blue, new Vector2(1800, 150)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 300)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 450)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 600)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 750)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 900)),
                    new Zombie(TeamEnum.Blue, new Vector2(1500, 150)),
                    new Zombie(TeamEnum.Blue, new Vector2(1500, 300)),
                    new Zombie(TeamEnum.Blue, new Vector2(1500, 450)),
                    new Zombie(TeamEnum.Blue, new Vector2(1500, 600)),
                    new Zombie(TeamEnum.Blue, new Vector2(1500, 750)),
                    new Zombie(TeamEnum.Blue, new Vector2(1500, 900)),
                },
                1400
            ),

            //6
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 150)),
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 300)),
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 450)),
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 600)),
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 750)),
                    new Peasant(TeamEnum.Blue, new Vector2(1800, 900)),
                    new Peasant(TeamEnum.Blue, new Vector2(1600, 150)),
                    new Peasant(TeamEnum.Blue, new Vector2(1600, 300)),
                    new Peasant(TeamEnum.Blue, new Vector2(1600, 450)),
                    new Peasant(TeamEnum.Blue, new Vector2(1600, 600)),
                    new Peasant(TeamEnum.Blue, new Vector2(1600, 750)),
                    new Peasant(TeamEnum.Blue, new Vector2(1600, 900)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 150)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 300)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 450)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 600)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 750)),
                    new Peasant(TeamEnum.Blue, new Vector2(1400, 900)),
                    new GoblinGiant(TeamEnum.Blue, new Vector2(1200, 500))
                },
                1100
            ),

            //7
            new Field
            (
                fieldWidth,
                fieldHeight,
                middleLineSeparator,
                leftAcceptableArea,
                new List<ITroop>
                {
                    new Peasant(TeamEnum.Blue, new Vector2(1450, 150)),
                    new Peasant(TeamEnum.Blue, new Vector2(1450, 250)),
                    new Peasant(TeamEnum.Blue, new Vector2(1450, 750)),
                    new Peasant(TeamEnum.Blue, new Vector2(1450, 850)),
                    new Peasant(TeamEnum.Blue, new Vector2(1550, 150)),
                    new Peasant(TeamEnum.Blue, new Vector2(1550, 250)),
                    new Peasant(TeamEnum.Blue, new Vector2(1550, 750)),
                    new Peasant(TeamEnum.Blue, new Vector2(1550, 850)),
                    new Peasant(TeamEnum.Blue, new Vector2(1350, 150)),
                    new Peasant(TeamEnum.Blue, new Vector2(1350, 250)),
                    new Peasant(TeamEnum.Blue, new Vector2(1350, 750)),
                    new Peasant(TeamEnum.Blue, new Vector2(1350, 850)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 150)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 270)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 390)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 510)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 630)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 750)),
                    new Archer(TeamEnum.Blue, new Vector2(1800, 870)),
                    new Boxer(TeamEnum.Blue, new Vector2(1600, 450)),
                    new Boxer(TeamEnum.Blue, new Vector2(1600, 600)),
                    new Zombie(TeamEnum.Blue, new Vector2(1400, 150)),
                    new Zombie(TeamEnum.Blue, new Vector2(1400, 300)),
                    new Zombie(TeamEnum.Blue, new Vector2(1400, 450)),
                    new Zombie(TeamEnum.Blue, new Vector2(1400, 600)),
                    new Zombie(TeamEnum.Blue, new Vector2(1400, 750)),
                    new Zombie(TeamEnum.Blue, new Vector2(1400, 900)),
                    new GoblinGiant(TeamEnum.Blue, new Vector2(1100, 300)),
                    new GoblinGiant(TeamEnum.Blue, new Vector2(1100, 500)),
                    new GoblinGiant(TeamEnum.Blue, new Vector2(1100, 700)),
                },
                2900
            )
        };


        currentField = Levels[0];

    }

    private void FieldEventTroopSuccessfulAttack(object sender, TroopAttackHandler args)
    {
        var troopView = troopsView[(args.troop).GetType()];
        var troopData = troopView.TroopsData[args.troop];
        if (!args.SuccessfullAttack)
        {
            troopView.SetColor(args.troop, troopView.GetTeamColor(args.troop.Team));
            return;
        }
        if (troopData.SpentTime < args.troop.AttackSpeed)
        {
            troopData.SpentTime += (float)args.gameTime.ElapsedGameTime.TotalSeconds;
            if (troopData.SpentTime > 0.5)
            {
                troopView.SetColor(args.troop, troopView.GetTeamColor(args.troop.Team));
            }
        }
        else
        {
            MediaPlayer.Play(troopView.HitSound);
            troopView.SetColor(args.troop, troopView.GetHurtColor(args.troop.Team));
            troopData.SpentTime = 0;
        }
    }

    protected override void Initialize()
    {
        MediaPlayer.Volume = 0.1f;

        _gameFeatures.escPress += _gameFeatures.OnEscPressed;
        clickedTroopType = ClickedTroopButtonEnum.None;
        currentField.TroopEventAttack += FieldEventTroopSuccessfulAttack;

        foreach (var generalButton in generalButtons)
        {
            generalButton.Click += (sender, e) =>
            {
                if (currentField.Troops.Any(x => x.Team == TeamEnum.Red))
                    currentField.ChangeGameState(GameStateEnum.Started);
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

        pauseMenu.Initialize();
        winMenu.Initialize();
        loseMenu.Initialize();
        introductionMenu.Initialize();

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
            var textureContent = Content.Load<Texture2D>("Button_Upgraded_Sample");
            var fontContent = Content.Load<SpriteFont>("ButtonFont_Sample");
            generalButton.LoadContent(textureContent, fontContent);
        }
        foreach (var troopButton in troopButtons.Values)
        {
            var textureContent = Content.Load<Texture2D>("Button_Upgraded_Sample");
            var fontContent = Content.Load<SpriteFont>("ButtonFont_Sample");
            troopButton.LoadContent(textureContent, fontContent);
        }

        pauseMenu.LoadContent();
        winMenu.LoadContent();
        loseMenu.LoadContent();
        introductionMenu.LoadContent();

        PlayBackgroundMusic();
        foreach (var environmentElement in environmentView)
        {
            var content = Content.Load<Texture2D>(environmentElement.SpriteAssetName);
            environmentElement.LoadContent(content);
        }
        foreach (var view in troopsView.Values)
        {
            var spriteAsset = Content.Load<Texture2D>(view.SpriteAssetName);
            var hitSound = Content.Load<Song>(view.HitSoundName);
            view.LoadContent(spriteAsset, hitSound);
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
        if (introductionMenu.IsFirstLaunch)
        {
            introductionMenu.Update(gameTime);
            return;
        }
        if (currentField.GameState == GameStateEnum.Finished)
        {
            if (currentField.Troops.Any(x => x.Team == TeamEnum.Red))
                winMenu.Update(gameTime);
            else
                loseMenu.Update(gameTime);
            return;
        }
        _gameFeatures.TryPauseGame();
        if (_gameFeatures.IsGamePaused)
        {
            pauseMenu.Update(gameTime);
            if (!flag)
            {
                previousGameState = currentField.GameState;
                flag = true;
            }
            currentField.ChangeGameState(GameStateEnum.Paused);
            return;
        }
        else
        {
            if (previousGameState != GameStateEnum.None && flag) currentField.ChangeGameState(previousGameState);
            flag = false;
            previousGameState = GameStateEnum.None;
        }


        if (!areEnemyTroopsDrawnOnTheStart)
        {
            foreach (var enemyTroop in currentField.EnemyTroops)
            {
                var troopView = troopsView[enemyTroop.GetType()];
                var sprite = troopView.Sprite;
                var troop = enemyTroop.OverrideTroop(
                            enemyTroop.Team, enemyTroop.InitialPosition, sprite.Width, sprite.Height);
                currentField.AddTroopEvent(() => troop);
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
            currentField.AddTroopEvent(() =>
            {
                ITroop CreateTroop<T>()
                {
                    var position = new Vector2(mouseState.X, mouseState.Y);
                    var troopType = typeof(T);
                    var sprite = troopsView[troopType].Sprite;

                    var constructor = troopType.GetConstructor(new Type[]
                    {
                        typeof(TeamEnum),
                        typeof(Vector2),
                        typeof(int),
                        typeof(int)
                    }
                    );
                    var troop = constructor.Invoke(new object[]
                    {
                         TeamEnum.Red,
                        new Vector2(
                            position.X - sprite.Width / 2,
                            position.Y - sprite.Height / 2
                            ),
                        sprite.Width,
                        sprite.Height
                    });

                    return (ITroop)troop;
                }

                switch (clickedTroopType)
                {
                    case ClickedTroopButtonEnum.None:
                        return null;
                    case ClickedTroopButtonEnum.Peasant:
                        return CreateTroop<Peasant>();
                    case ClickedTroopButtonEnum.Boxer:
                        return CreateTroop<Boxer>();
                    case ClickedTroopButtonEnum.Zombie:
                        return CreateTroop<Zombie>();
                    case ClickedTroopButtonEnum.Archer:
                        return CreateTroop<Archer>();
                    case ClickedTroopButtonEnum.GoblinGiant:
                        return CreateTroop<GoblinGiant>();
                    default:
                        return null;
                }
            });
        }
        else if (mouseState.RightButton == ButtonState.Pressed)
        {
            currentField.RemoveTroop(mouseState.Position.ToVector2());
        }

        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Tab))
        {
            ChangeLevelTo(Levels.IndexOf(currentField));
        }

        if (currentField.GameState == GameStateEnum.Started)
        {
            currentField.PlayGame(gameTime);
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
        if (currentField.GameState == GameStateEnum.ArrangingTroops)
        {
            _spriteBatch.DrawRectangle(currentField.LineSeparator, Color.Red);
            DrawUnderMouseRectangle();
            foreach (var troop in currentField.Troops)
            {
                var viewType = troopsView[troop.GetType()];
                var troopBehindMouse =
                    currentField.GetTroopByPosition(Mouse.GetState().Position.ToVector2());
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
                if (textType == typeof(TextView))
                    money = currentField.Money.ToString();

                text.Draw(_spriteBatch, Window, money);
            }
        }
        else if (currentField.GameState == GameStateEnum.Started)
        {
            foreach (var troop in currentField.Troops)
            {
                var viewType = troopsView[troop.GetType()];
                viewType.Draw(_spriteBatch, troop);
            }
        }
        else if (currentField.GameState == GameStateEnum.Paused)
        {
            pauseMenu.Draw(gameTime, _spriteBatch);
        }
        else if (currentField.GameState == GameStateEnum.Finished)
        {
            foreach (var troop in currentField.Troops)
            {
                var viewType = troopsView[troop.GetType()];
                viewType.Draw(_spriteBatch, troop);
            }
            if (currentField.Troops.Any(x => x.Team == TeamEnum.Red))
                winMenu.Draw(gameTime, _spriteBatch);
            else
                loseMenu.Draw(gameTime, _spriteBatch);
        }

        if (introductionMenu.IsFirstLaunch)
        {
            introductionMenu.Draw(gameTime, _spriteBatch);
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

    public void ChangeLevelTo(int id)
    {
        currentField = Levels[id];
        areEnemyTroopsDrawnOnTheStart = false;
        _gameFeatures.IsGamePaused = false;
        currentField.ResetField();
        previousGameState = GameStateEnum.ArrangingTroops;
        currentField.TroopEventAttack += FieldEventTroopSuccessfulAttack;
    }

    private Dictionary<ClickedTroopButtonEnum, Button> GenerateTroopsButtons()
    {
        ITroop CreateTestTroop(string troopName)
        {
            var instance = Activator.CreateInstance(null, $"BattleSimulator.Model.{troopName}");
            return (ITroop)instance.Unwrap();
        }

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
                new Button(previousPosition, troopName, buttonWidth, buttonHeight, CreateTestTroop(troopName).Cost.ToString()));
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
            currentField.CanPlaceTroop(mousePosition, currentTroopSprite.Width, currentTroopSprite.Height)
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

    private void PlayBackgroundMusic()
    {
        BackgroundMusic = Content.Load<Song>("Background_Music");
        MediaPlayer.Play(BackgroundMusic);
    }
}