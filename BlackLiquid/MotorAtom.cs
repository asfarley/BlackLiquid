using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLiquid
{
    public class MotorAtom : Atom
    {
        public int energy = 20;
        public int energyMax = 30;

        private Random r = new Random();
        public MotorAtom()
        {
            PixelBrush = System.Windows.Media.Brushes.Yellow;
        }

        public override AtomsDelta Update(AtomCollection atoms)
        {
            if(energy > 0)
            {
                var sign1 = r.Next(2) > 0;
                var sign2 = r.Next(2) > 0;

                var X_new = X + ( sign1 ?  r.Next(2) : -r.Next(2));
                var Y_new = Y + ( sign2 ? r.Next(2) : -r.Next(2));

                if(atoms.PositionIsFree(X_new, Y_new, 640, 480))
                {
                    X = X_new;
                    Y = Y_new;
                    energy--;
                }
            }

            var ad = new AtomsDelta();
            //if (energy <= 0)
            //{
            //    ad.DeletedAtoms.Add(this);
            //}
            return ad;
        }

        public override AtomsDelta Interact(Atom a, AtomCollection atoms)
        {
            var share = 0;

            if(energy <= 0)
            {
                return new AtomsDelta();
            }

            var distance = Math.Sqrt(Math.Pow(X - a.X, 2) + Math.Pow(Y - a.Y, 2));
            if (distance > 30)
            {
                return new AtomsDelta();
            }

            var movement = r.Next(energy) / 3;
            energy -= movement;

            switch (a)
            {
                case MotorAtom: //Repulsive effect
                    if(a.X >= X)
                    {
                        X -= movement;
                        a.X += movement;
                    }
                    else
                    {
                        X += movement;
                        a.X -= movement;
                    }
                    
                    if(a.Y >= Y)
                    {
                        Y -= movement;
                        a.Y += movement;
                    }
                    else
                    {
                        Y += movement;
                        a.Y -= movement;
                    }
                    break;
                case EnergyAtom:
                    break;
                case InfoAtom:
                    break;
                case EnergySource:
                    break;
                case StructureAtom: //Pushing effect
                    if (a.X >= X)
                    {
                        X += movement;
                        a.X += movement;
                    }
                    else
                    {
                        X -= movement;
                        a.X -= movement;
                    }

                    if (a.Y >= Y)
                    {
                        Y += movement;
                        a.Y += movement;
                    }
                    else
                    {
                        Y -= movement;
                        a.Y -= movement;
                    }
                    break;
            }

            return new AtomsDelta();
        }
    }
}
