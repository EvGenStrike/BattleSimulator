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

    private BackgroundView backgroundView; 
    private readonly Dictionary<Type, ISpriteView> allSprites;

    public MainView()
    {
        _graphics = new GraphicsDeviceManager(this)
        //;
        {
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080,
            IsFullScreen = true
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        backgroundView = new BackgroundView();
        allSprites = new Dictionary<Type, ISpriteView>
        {
            { typeof(Peasant), new PeasantView() },
        };

        field = new Field();
    }

    protected override void Initialize()
    {
        backgroundView.Initialize(_graphics);
        foreach (var sprite in allSprites.Values)
        {
            sprite.Initialize(_graphics);
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        backgroundView.LoadContent(Content.Load<Texture2D>(backgroundView.SpriteAssetName));
        foreach (var sprite in allSprites.Values)
        {
            var content = Content.Load<Texture2D>(sprite.SpriteAssetName);
            sprite.LoadContent(content);
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
                var sprite = allSprites[typeof(Peasant)].Sprite;
                return new Peasant(position, sprite.Width, sprite.Height);
            });
            
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        backgroundView.Draw(_spriteBatch, Window);
        foreach (var troop in field.Troops)
        {
            var viewType = allSprites[troop.GetType()];
            viewType.Draw(_spriteBatch, Window, troop);
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}