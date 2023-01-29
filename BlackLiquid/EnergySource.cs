using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class EnergySource : Atom
    {
        public double PowerOutput = 0;

        public double SpawnProbability = 0.01;

        private Random r = new Random();

        public EnergySource()
        {
            PixelBrush = System.Windows.Media.Brushes.White;
        }

        public override AtomsDelta Update(AtomCollection atoms)
        {
            var ad = new AtomsDelta();
            var spawnRoll = (r.NextDouble() < SpawnProbability);
            if(spawnRoll)
            {
                var energy = new EnergyAtom();
                energy.X = X + r.Next(-1, 2);
                energy.Y = Y + r.Next(-1, 2);
                if(atoms.PositionIsFree(energy.X, energy.Y, GlobalConstants.Width, GlobalConstants.Height))
                {
                    ad.NewAtoms.Add(energy);
                }
            }

            return ad;
        }
    }
}
