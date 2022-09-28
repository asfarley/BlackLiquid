using System;
using System.Collections.Generic;
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

        public override void Update()
        {
            if(energy < 100)
            {
                var deficit = 100 - energy;
                var newEnergy = r.Next(deficit)/10;
                energy += newEnergy;
            }
        }

        public override void Interact(Atom a)
        {
            var share = 0;

            var distance = Math.Sqrt(Math.Pow(X - a.X, 2) + Math.Pow(Y - a.Y, 2));
            if (distance > 30)
            {
                    return;   
            }

            switch(a)
            {
                case MotorAtom:
                    Debug.WriteLine("Sending energy to motor");
                    var m = (MotorAtom)a;
                    share = Math.Min(r.Next(energy), 100 - m.energy);
                    m.energy += share;
                    energy -= share;
                    break;
                case EnergyAtom:
                    var e = (EnergyAtom)a;
                    share = Math.Min(r.Next(energy), 110 - e.energy);
                    e.energy += (int)( share*1.1);
                    energy -= share;
                    break;
            }
        }
    }
}
