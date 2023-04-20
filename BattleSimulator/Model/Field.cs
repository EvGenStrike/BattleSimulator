using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Model;

internal class Field
{
    public List<ITroop> Troops { get; set; }

    private List<Rectangle> TroopsCollisions { get; set; }

    private event Func<ITroop> addTroopEvent;

    public Field()
    {
        Troops = new();
        TroopsCollisions = new();
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
        if (CausesCollision(troop))
            return;
        Troops.Add(troop);
        TroopsCollisions.Add(new Rectangle(
            (int)troop.InitialPosition.X, 
            (int)troop.InitialPosition.Y, 
            troop.Width, 
            troop.Height));
    }

    public bool CausesCollision(ITroop troop)
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

    public void RemoveTroop()
    {
        throw new NotImplementedException();
    }

    public void GetTroops()
    {
        throw new NotImplementedException();
    }
}
