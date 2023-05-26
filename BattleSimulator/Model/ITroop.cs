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
    public float Health { get; } 
    public float Damage { get; }
    public float MoveSpeed { get; }
    public float AttackSpeed { get; }
    public float AttackDistance { get; }
    public int Cost { get; }
    public Vector2 InitialPosition { get; }
    public Vector2 CurrentPosition { get; }
    public int Width { get; }
    public int Height { get; }
    public ITroop OverrideTroop(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0);
    public void Move(float angle, GameTime gameTime);
    public bool TryAttackEnemy(ITroop troop, GameTime gameTime);
    public void DecreaseHealth(float deltaHealth);
}
