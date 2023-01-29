using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class EnergyAtom : Atom
    {
        public int energy = 100;

        private Random r = new Random();

        public EnergyAtom()
        {
            PixelBrush = System.Windows.Media.Brushes.Red;
        }

        public override AtomsDelta Update(AtomCollection atoms)
        {
            var ad = new AtomsDelta();
            if(energy <= 0)
            {
                ad.DeletedAtoms.Add(this);
            }
            return ad;
        }

        public override AtomsDelta Interact(Atom a, AtomCollection atoms)
        {
            var share = 0;

            var distance = Math.Sqrt(Math.Pow(X - a.X, 2) + Math.Pow(Y - a.Y, 2));
            if (distance > 30)
            {
                    return new AtomsDelta();   
            }

            switch(a)
            {
                case MotorAtom:
                    var m = (MotorAtom)a;
                    share = Math.Min(r.Next(energy), m.energyMax - m.energy);
                    m.energy += share;
                    energy -= share;
                    break;
                case EnergyAtom:
                    break;
            }

            return new AtomsDelta();
        }
    }
}
