using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public abstract class Atom : ActiveObject
    {
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; NotifyPropertyChanged(); }
        }

        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; NotifyPropertyChanged(); }
        }

        private System.Windows.Media.Brush pixelBrush = System.Windows.Media.Brushes.White;

        public System.Windows.Media.Brush PixelBrush
        {
            get { return pixelBrush; }
            set
            {
                pixelBrush = value;
                NotifyPropertyChanged();
            }
        }

        public virtual AtomsDelta Update(AtomCollection atoms)
        {
            return new AtomsDelta();
        }

        public virtual AtomsDelta Interact(Atom a, AtomCollection atoms)
        {
            return new AtomsDelta();
        }
    }
}
