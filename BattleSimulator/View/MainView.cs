using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleSimulator.View;

public class MainView : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly ISpriteView[] allSprites;

    public MainView()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080,
            IsFullScreen = true
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        allSprites = new ISpriteView[]
        {
            new BackgroundView(),
            new PeasantView()
        };
    }

    protected override void Initialize()
    {
        foreach (var sprite in allSprites)
        {
            sprite.Initialize(_graphics);
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        foreach (var sprite in allSprites)
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

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        foreach (var sprite in allSprites)
        {
            sprite.Draw(_spriteBatch, Window);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}