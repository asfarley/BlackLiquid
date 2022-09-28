using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace BlackLiquid
{
    public class SimulationWorld: ActiveObject
    {
        private ObservableCollection<Atom> atoms = new ObservableCollection<Atom>();

        public ObservableCollection<Atom> Atoms
        {
            get { return atoms; }
            set { atoms = value; NotifyPropertyChanged(); }
        }

        private Random random = new Random();
        public void Update()
        {
            foreach(var atom in atoms)
            {
                atom.Update();
            }

            for(int i=0;i<1000;i++)
            {
                int a1index = random.Next(atoms.Count);
                int a2index = random.Next(atoms.Count);
                var a1 = atoms[a1index];
                var a2 = atoms[a2index];
                //Debug.WriteLine("Interacting: " + a1index + " and " + a2index);
                Interact(a1, a2);
            }
        }

        public void Initialize()
        {
            Random r = new Random();
            for(int i= 0; i < 300; i++)
            {
                Atom a = null;
                var atomTypeRand = r.Next(4);
                switch(atomTypeRand)
                {
                    case 0:
                        a = new InfoAtom();
                        break;
                    case 1:
                        a = new EnergyAtom();
                        break;
                    case 2:
                        a = new StructureAtom();
                        break;
                    case 3:
                        a = new MotorAtom();
                        break;
                    default:
                        a = new StructureAtom();
                        break;
                }
                
                a.X = r.Next(640);
                a.Y = r.Next(480);
                atoms.Add(a);
            }
        }

        public void Interact(Atom a1, Atom a2)
        {
            if(a1 == a2)
            {
                return;
            }

            if(a1 == null || a2 == null)
            {
                return;
            }

            a1.Interact(a2);
        }
    }
}
