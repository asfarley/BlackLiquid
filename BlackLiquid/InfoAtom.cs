using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class InfoAtom : Atom
    {
        public int[] information = new int[32];

        public InfoAtom()
        {
            PixelBrush = System.Windows.Media.Brushes.Lime;
        }
    }
}
