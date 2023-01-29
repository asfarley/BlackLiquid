using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class AtomCollection : ObservableCollection<Atom>
    {
        public bool PositionIsFree(int x, int y, int maxX, int maxY)
        {
            return !this.Any(a => a.X == x && a.Y == y) && x >= 0 && y >= 0 && x < GlobalConstants.Width && y < GlobalConstants.Height;
        }
    }
}
