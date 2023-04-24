using BattleSimulator.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Field
{
    public List<ITroop> Troops { get; }
    public Rectangle AcceptableArea { get; private set; }
    public Rectangle LineSeparator { get; }
    public int Money { get; private set; }
    public int FieldWidth { get; }
    public int FieldHeight { get; }

    private List<Rectangle> TroopsCollisions { get; set; }

    public event Func<ITroop> addTroopEvent;
    private event Action<Vector2> removeTroopEvent;

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
        if (addTroopEvent() == null)
            return;
        this.addTroopEvent += addTroopEvent;
        AddTroop();
    }

    private void AddTroop()
    {
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

    private bool CanPlaceTroop(ITroop troop)
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

    public void GetTroops()
    {
        throw new NotImplementedException();
    }

    public void AddAcceptableArea(Rectangle accebtableArea)
    {
        AcceptableArea = accebtableArea;
    }
}
