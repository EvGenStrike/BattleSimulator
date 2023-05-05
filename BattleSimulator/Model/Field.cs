using BattleSimulator.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Field
{
    public double Angle;

    public List<ITroop> Troops { get; }
    public List<ITroop> EnemyTroops { get; }
    public List<ITroop> AllyTroops { get; }
    public Rectangle AcceptableArea { get; private set; }
    public Rectangle LineSeparator { get; }
    public int Money { get; private set; }
    public int FieldWidth { get; }
    public int FieldHeight { get; }
    public GameStateEnum GameState { get; private set; }

    private List<Rectangle> TroopsCollisions { get; set; }

    public event Func<ITroop> addTroopEvent;
    private event Action<Vector2> removeTroopEvent;

    public Field(
        int fieldWIdth,
        int fieldHeight,
        Rectangle lineSeparator,
        Rectangle acceptableArea,
        List<ITroop> enemyTroops,
        int money)
    {
        Troops = new();
        AllyTroops = new();
        EnemyTroops = enemyTroops;
        TroopsCollisions = new();
        Money = money;
        FieldWidth = fieldWIdth;
        FieldHeight = fieldHeight;
        GameState = GameStateEnum.ArrangingTroops;
        LineSeparator = lineSeparator;
        AcceptableArea = acceptableArea;    
    }

    public void AddTroopEvent(Func<ITroop> addTroopEvent)
    {
        if (addTroopEvent() == null)
            return;
        this.addTroopEvent += addTroopEvent;
        AddTroop();   
    }

    private void AddTroop()
    {
        var troop = addTroopEvent();
        addTroopEvent = null;
        if (troop.Team == TeamEnum.Red && !CanPlaceTroop(troop))
            return;
        if (troop.Team == TeamEnum.Blue && !CanPlaceEnemyTroop(troop))
            return;
        Troops.Add(troop); 
        AllyTroops.Add(troop);
        if (troop.Team == TeamEnum.Red)
            Money -= troop.Cost;
        TroopsCollisions.Add(new Rectangle(
            (int)troop.InitialPosition.X, 
            (int)troop.InitialPosition.Y, 
            troop.Width, 
            troop.Height));
    }

    private bool CanPlaceTroop(ITroop troop)
    {
        return troop.Cost <= Money
            && !CausesCollision(troop)
            && IsTroopWithinBorders(troop)
            && GameState == GameStateEnum.ArrangingTroops;
    }

    private bool CanPlaceEnemyTroop(ITroop troop)
    {
        return !CausesCollision(troop)
            && !IsTroopWithinBorders(troop);
    }

    private bool CausesCollision(ITroop troop)
    {
        var troopRectangle = new Rectangle(
            (int)troop.InitialPosition.X,
            (int)troop.InitialPosition.Y,
            troop.Width,
            troop.Height);
        foreach (var previousTroop in TroopsCollisions)
        {
            if (troopRectangle.Intersects(previousTroop))
                return true;
        }
        return false;
    }

    private bool IsTroopWithinBorders(ITroop troop)
    {
        var middleWidthLength = troop.Width / 2;
        var middleHeightLength = troop.Height / 2;

        if (AcceptableArea != default)
            return ((troop.InitialPosition.X) >= AcceptableArea.X
                && (troop.InitialPosition.Y) >= AcceptableArea.Y
                && (troop.InitialPosition.X + troop.Width) <= (AcceptableArea.X + AcceptableArea.Width)
                && (troop.InitialPosition.Y + troop.Height) <= (AcceptableArea.Y + AcceptableArea.Height));
        return ((troop.InitialPosition.X - middleWidthLength) >= 0
            && (troop.InitialPosition.Y - middleHeightLength) >= 0
            && (troop.InitialPosition.X + middleWidthLength) <= LineSeparator.X
            && (troop.InitialPosition.Y + middleHeightLength) <= FieldHeight);
    }

    public bool CanPlaceTroop(
        Vector2 mousePosition,
        int width,
        int height)
    {
        return CanPlaceTroop(new Peasant(
            TeamEnum.Red,
            new Vector2(
                mousePosition.X - width / 2,
                mousePosition.Y - height / 2
                ),
            width,
            height
            ));
    }

    public void RemoveTroopEvent(Action<Vector2> removeTroopEvent)
    {
        this.removeTroopEvent += removeTroopEvent;
        
    }

    public void RemoveTroop(Vector2 position)
    {
        var i = GetTroopIndexByPosition(position);
        if (i == -1) return;
        Money += Troops[i].Cost;
        Troops.RemoveAt(i);
        TroopsCollisions.RemoveAt(i);
    }

    public ITroop GetTroopByPosition(Vector2 position)
    {
        for (var i = 0; i < TroopsCollisions.Count; i++)
        {
            if (TroopsCollisions[i].Contains(position))
            {
                return Troops[i];
            }
        }
        
        return null;
    }

    public int GetTroopIndexByPosition(Vector2 position)
    {
        for (var i = 0; i < TroopsCollisions.Count; i++)
        {
            if (TroopsCollisions[i].Contains(position))
            {
                return i;
            }
        }

        return -1;
    }

    public void ChangeGameState(GameStateEnum gameState)
    {
        GameState = gameState;
    }


    private void GenerateEnemyTroopsCollisions()
    {
        foreach (var enemyTroop in EnemyTroops)
        {
            TroopsCollisions.Add(new Rectangle(
            (int)enemyTroop.InitialPosition.X,
            (int)enemyTroop.InitialPosition.Y,
            enemyTroop.Width,
            enemyTroop.Height));
        }
    }

    public void BeginGame()
    {
        TroopsCollisions.Clear();
        foreach (var troop in Troops)
        {
            var closestEnemy = GetClosestEnemyTroop(troop);
            if (closestEnemy is null) continue;
            var angle = GetAngleBetweenVectors(troop.CurrentPosition, closestEnemy.CurrentPosition);
            troop.Move((float)angle);
            Angle = angle;
        }
    }

    private ITroop GetClosestEnemyTroop(ITroop troop)
    {
        var minDistance = double.MaxValue;
        var closestTroop = default(ITroop);
        foreach (var otherTroop in Troops)
        {
            if (otherTroop.Team != troop.Team)
            {
                var distance =
                    GetDistanceBetweenVectors(otherTroop.CurrentPosition, troop.CurrentPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTroop = otherTroop;
                }
            }
        }

        return closestTroop;
    }

    private double GetDistanceBetweenVectors(Vector2 vector1, Vector2 vector2)
    {
        var dx = vector1.X - vector2.X;
        var dy = vector1.Y - vector2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private double GetAngleBetweenVectors(Vector2 vector1, Vector2 vector2)
    {
        var angle = Math.Atan2(vector2.Y - vector1.Y, vector2.X - vector1.X);
        return angle;
    }

    private double GetVectorLength(Vector2 vector)
    {
        return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    }

}
