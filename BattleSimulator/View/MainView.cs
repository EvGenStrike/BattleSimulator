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

    private readonly List<IEnvironmentView> environmentView;
    private readonly Dictionary<Type, ITroopView> troopsView;
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

        environmentView = new List<IEnvironmentView>
        {
            new BackgroundView()
        };
        textsView = new List<ITextView>
        {
            new MoneyTextView()
        };
        troopsView = new Dictionary<Type, ITroopView>
        {
            { typeof(Peasant), new PeasantView() },
        };

        field = new Field(1920, 1080);
    }

    protected override void Initialize()
    {     
        foreach (var environmentElement in  environmentView)
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

        foreach (var environmentView in environmentView)
        {
            environmentView.Draw(_spriteBatch);
        }
        _spriteBatch.DrawRectange(field.LineSeparator, Color.Red);
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
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}