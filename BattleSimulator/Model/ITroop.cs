using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

public interface ITroop
{
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
}
