using BattleSimulator.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Peasant : ITroop
{
    public TeamEnum Team { get; private set; }
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public int MoveSpeed { get; private set; }
    public int AttackSpeed { get; private set; }
    public int AttackDistance { get; private set; }
    public int Cost { get; private set; }
    public Vector2 InitialPosition { get; private set; }
    public Vector2 CurrentPosition { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float SpentTime { get; private set; }

    private Random random = new();

    public Peasant()
    {
        SetStats();
    }

    public Peasant(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0, GameTime gameTime = null)
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
        Health = 50 + random.Next(5) - 2;
        Damage = 20 + random.Next(3) - 1;
        MoveSpeed = 12 + random.Next(3) - 1;
        AttackSpeed = 2;
        AttackDistance = 3;
        Cost = 50;
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
        //enemy.DecreaseHealth()
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

    public ITroop OverrideTroop(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0, GameTime gameTime = null)
    {
        return new Peasant(team, initialPosition, width, height, gameTime);
    }

    public void DecreaseHealth(int deltaHealth)
    {
        Health -= deltaHealth;
    }
}
