using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class EnergySource : Atom
    {
        public double PowerOutput = 0;

        public EnergySource()
        {
            PixelBrush = System.Windows.Media.Brushes.White;
        }
    }
}
