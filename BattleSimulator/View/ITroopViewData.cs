using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public interface ITroopViewData
{
    public Color Color { get; set; }
    public float SpentTime { get; set; }
}
