using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Field
{
    public List<ITroop> Troops { get; }
    public Rectangle LineSeparator { get; }
    public int Money { get; private set; }
    public int FieldWidth { get; }
    public int FieldHeight { get; }

    private List<Rectangle> TroopsCollisions { get; set; }

    private event Func<ITroop> addTroopEvent;

    public Field(int fieldWIdth, int fieldHeight)
    {
        Troops = new();
        TroopsCollisions = new();
        Money = 10000;
        FieldWidth = fieldWIdth;
        FieldHeight = fieldHeight;
        LineSeparator = GenerateLineSeparator();        
    }

    public Rectangle GenerateLineSeparator()
    {
        var width = FieldWidth / 100;
        var height = FieldHeight;
        var x = (FieldWidth / 2) - width / 2;
        var y = 0;
        return new Rectangle(x, y, width, height);
    }

    public void AddTroopEvent(Func<ITroop> addTroopEvent)
    {
        this.addTroopEvent += addTroopEvent;
        AddTroop();
    }

    public void AddTroop()
    {
        if (addTroopEvent == null)
            throw new Exception("AddTroopEvent is not subscribed");
        var troop = addTroopEvent();
        addTroopEvent = null;
        if (!CanPlaceTroop(troop))
            return;
        Troops.Add(troop);
        Money -= troop.Cost;
        TroopsCollisions.Add(new Rectangle(
            (int)troop.InitialPosition.X, 
            (int)troop.InitialPosition.Y, 
            troop.Width, 
            troop.Height));
    }

    public bool CanPlaceTroop(ITroop troop)
    {
        return troop.Cost <= Money
            && !CausesCollision(troop)
            && IsTroopWithinBorders(troop);

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
        return ((troop.InitialPosition.X - middleWidthLength) >= 0 
            && (troop.InitialPosition.Y - middleHeightLength) >= 0
            && (troop.InitialPosition.X + middleWidthLength) <= LineSeparator.X
            && (troop.InitialPosition.Y + middleHeightLength) <= FieldHeight);
    }

    public void RemoveTroop()
    {
        throw new NotImplementedException();
    }

    public void GetTroops()
    {
        throw new NotImplementedException();
    }
}
