using BattleSimulator.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BattleSimulator.View;

public class MainView : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Field field;

    private readonly BackgroundView backgroundView; 
    private readonly Dictionary<Type, ISpriteView> troopsView;
    private List<ITextView> textsView;

    public MainView()
    {
        _graphics = new GraphicsDeviceManager(this)
        //;
        {
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080,
            IsFullScreen = true,
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        backgroundView = new BackgroundView();
        textsView = new List<ITextView>
        {
            new MoneyTextView()
        };
        troopsView = new Dictionary<Type, ISpriteView>
        {
            { typeof(Peasant), new PeasantView() },
        };

        field = new Field();
    }

    protected override void Initialize()
    {
        backgroundView.Initialize(_graphics, Window);       

        foreach (var sprite in troopsView.Values)
        {
            sprite.Initialize(_graphics, Window);
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        backgroundView.LoadContent(Content.Load<Texture2D>(backgroundView.SpriteAssetName));
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

        var mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            field.AddTroopEvent(() =>
            {
                var random = new Random();
                var position = new Vector2(mouseState.X, mouseState.Y);
                var sprite = troopsView[typeof(Peasant)].Sprite;
                return new Peasant(position, sprite.Width, sprite.Height);
            });
            
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        backgroundView.Draw(_spriteBatch);
        foreach (var troop in field.Troops)
        {
            var viewType = troopsView[troop.GetType()];
            viewType.Draw(_spriteBatch, troop);
        }
        foreach (var text in textsView)
        {
            var newText = text.GetType() == typeof(MoneyTextView) ? field.Money.ToString() : "";
            text.Draw(_spriteBatch, Window, newText);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}