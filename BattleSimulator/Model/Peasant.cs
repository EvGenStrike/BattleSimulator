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
    public TeamEnum Team { get; }
    public int Health { get; private set; }
    public int Damage { get; }
    public int MoveSpeed { get; }
    public int AttackSpeed { get; }
    public int AttackDistance { get; }
    public int Cost { get; }
    public Vector2 InitialPosition { get; }
    public Vector2 CurrentPosition { get; private set; }
    public int Width { get; }
    public int Height { get; }
    public GameTime CurrentGameTime { get; set; }
    public float SpentTime { get; set; }

    private Random random;

    public Peasant(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0, GameTime gameTime = null)
    {
        Team = team;
        Health = 50;
        Damage = 20;
        MoveSpeed = 12;
        AttackSpeed = 2;
        AttackDistance = 3;
        Cost = 50;

        InitialPosition = initialPosition;
        CurrentPosition = InitialPosition;
        Width = width;
        Height = height;
        CurrentGameTime = gameTime;

        random = new();
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
