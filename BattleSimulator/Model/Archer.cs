using BattleSimulator.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Archer : ITroop
{
    public TeamEnum Team { get; private set; }
    public float Health { get; private set; }
    public float Damage { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackSpeed { get; private set; }
    public float AttackDistance { get; private set; }
    public int Cost { get; private set; }
    public Vector2 InitialPosition { get; private set; }
    public Vector2 CurrentPosition { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float SpentTime { get; private set; }

    private Random random = new();

    public Archer()
    {
        SetStats();
    }

    public Archer(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0)
    {
        Team = team;
        SetStats();

        InitialPosition = initialPosition;
        CurrentPosition = InitialPosition;
        Width = width;
        Height = height;
    }

    public void SetStats()
    {
        Health = 40 + random.Next(9) - 4;
        Damage = 100 + random.Next(5) - 2;
        MoveSpeed = 5 + random.Next(3) - 1;
        AttackSpeed = 6 + (float)((random.NextDouble() - 0.5) / 2);
        AttackDistance = 1000;
        Cost = 150;
    }

    public void Move(float angle, GameTime gameTime)
    {
        angle = angle + (float)((random.NextDouble() - 0.5) / 3);
        CurrentPosition
            = new Vector2(
                CurrentPosition.X
                    + (float)Math.Cos(angle)
                    * MoveSpeed
                    * 10
                    * (float)gameTime.ElapsedGameTime.TotalSeconds,
                CurrentPosition.Y + (float)Math.Sin(angle) * MoveSpeed * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public bool TryAttackEnemy(ITroop enemy, GameTime gameTime)
    {
        if (SpentTime >= (AttackSpeed + (new Random().NextDouble() - 0.5) * 0.5))
        {
            SpentTime = 0;
            enemy.DecreaseHealth(Damage);
            return true;
        }
        else
        {
            SpentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            return false;
        }
    }

    public ITroop OverrideTroop(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0)
    {
        return new Archer(team, initialPosition, width, height);
    }

    public void DecreaseHealth(float deltaHealth)
    {
        Health -= deltaHealth;
    }
}
