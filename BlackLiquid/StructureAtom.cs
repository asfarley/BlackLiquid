using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class StructureAtom : Atom
    {

        public StructureAtom()
        {
            PixelBrush = System.Windows.Media.Brushes.Gray;
        }

        public override AtomsDelta Interact(Atom a, AtomCollection atoms)
        {
            return new AtomsDelta();
        }
    }
}
