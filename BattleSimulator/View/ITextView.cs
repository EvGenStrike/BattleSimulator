using BattleSimulator.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public interface ITextView
{
    public string TextAssetName { get; }
    public SpriteFont TextFont { get; }

    public void LoadContent(SpriteFont uiSource);
    public void Draw(
        SpriteBatch spriteBatch,
        GameWindow gameWindow,
        string text = "");
}
