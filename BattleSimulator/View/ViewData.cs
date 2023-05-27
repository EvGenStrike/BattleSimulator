using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.View;

public class ViewData : ITroopViewData
{
    public Color Color { get; set; }
    public float SpentTime { get; set; }
}
