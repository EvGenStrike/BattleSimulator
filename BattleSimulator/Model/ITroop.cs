using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

public interface ITroop
{
    public TeamEnum Team { get; }
    public int Health { get; } 
    public int Damage { get; }
    public int MoveSpeed { get; }
    public int AttackSpeed { get; }
    public int AttackDistance { get; }
    public int Cost { get; }
    public Vector2 InitialPosition { get; }
    public Vector2 CurrentPosition { get; }
    public int Width { get; }
    public int Height { get; }
    public ITroop OverrideTroop(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0, GameTime gameTime = null);
    public void Move(float angle, GameTime gameTime);
    public bool TryAttackEnemy(ITroop troop, GameTime gameTime);
    public void DecreaseHealth(int deltaHealth);
}
