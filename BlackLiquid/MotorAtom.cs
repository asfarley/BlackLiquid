using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class MotorAtom : Atom
    {
        public int energy = 100;

        private Random r = new Random();
        public MotorAtom()
        {
            PixelBrush = System.Windows.Media.Brushes.Yellow;
        }

        public override void Update()
        {
            if(energy > 0)
            {
                var sign1 = r.Next(2) > 0;
                var sign2 = r.Next(2) > 0;

                X += sign1 ?  r.Next(2) : -r.Next(2);
                Y += sign2 ? r.Next(2) : -r.Next(2);
                energy--;
            }
        }
    }
}
