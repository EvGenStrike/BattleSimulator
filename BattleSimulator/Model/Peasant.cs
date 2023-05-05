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
    public int Health { get; }
    public int Damage { get; }
    public int MoveSpeed { get; }
    public int AttackSpeed { get; }
    public int AttackDistance { get; }
    public int Cost { get; }
    public Vector2 InitialPosition { get; }
    public Vector2 CurrentPosition { get; private set; }
    public int Width { get; }
    public int Height { get; }

    public Peasant(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0)
    {
        Team = team;
        Health = 50;
        Damage = 25;
        MoveSpeed = 3;
        AttackSpeed = 3;
        AttackDistance = 3;
        Cost = 50;

        InitialPosition = initialPosition;
        CurrentPosition = InitialPosition;
        Width = width;
        Height = height;
    }

    public void Move(float angle)
    {
        CurrentPosition
            = new Vector2(
                CurrentPosition.X + (float)Math.Cos(angle) * MoveSpeed,
                CurrentPosition.Y + (float)Math.Sin(angle) * MoveSpeed);
    }

    public ITroop OverrideTroop(TeamEnum team, Vector2 initialPosition, int width = 0, int height = 0)
    {
        return new Peasant(team, initialPosition, width, height);
    }

}
