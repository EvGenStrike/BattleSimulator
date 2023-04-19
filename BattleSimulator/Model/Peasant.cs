﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Peasant : ITroop
{
    public int Health { get; }
    public int Damage { get; }
    public int MoveSpeed { get; }
    public int AttackSpeed { get; }
    public int AttackDistance { get; }
    public int Cost { get; }
    public Vector2 InitialPosition { get; }
    public Vector2 CurrentPosition { get; private set; }

    public Peasant(Vector2 initialPosition)
    {
        Health = 50;
        Damage = 25;
        MoveSpeed = 3;
        AttackSpeed = 3;
        AttackDistance = 3;
        Cost = 50;

        InitialPosition = initialPosition;
    }

    public void Move()
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Up))
        {
            //CurrentPosition.Y -= MoveSpeed * (float)
        }
    }
}