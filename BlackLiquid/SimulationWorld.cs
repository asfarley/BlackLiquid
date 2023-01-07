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

        private ObservableCollection<EnergySource> energySources = new ObservableCollection<EnergySource>();
        public ObservableCollection<EnergySource> EnergySources
        {
            get { return energySources; }
            set { energySources = value; NotifyPropertyChanged(); }
        }

        private Random random = new Random();
        public void Update()
        {
            foreach(var atom in atoms)
            {
                atom.Update();
            }

            for(int i=0;i<10000;i++)
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
            Atoms.Clear();
            InitializeStructureAtoms();
            InitializeEnergySources();
            InitializeMotorAtoms();
            InitializeInfoAtoms();
        }

        public void InitializeStructureAtoms()
        {
            int maxStructureSeeds = 100;
            int numStructureSeeds = random.Next(1, maxStructureSeeds);

            int maxStructureAtoms = (int)( 640 * 480 * 0.1);

            var structureSeeds = new List<Atom>();

            for (int i = 0; i < numStructureSeeds; i++)
            {
                var sa = new StructureAtom();
                sa.X = random.Next(640);
                sa.Y = random.Next(480);
                structureSeeds.Add(sa);
            }

            int maxGrowthCycles = 20;
            int numGrowthCycles = random.Next(maxGrowthCycles);

            int nStructureAtoms = 0;

            for(int i=0;i<numGrowthCycles;i++)
            {
                var newStructureAtoms = new List<Atom>();
                foreach (var s in structureSeeds)
                {
                    var s_new = new StructureAtom();
                    s_new.X = s.X + random.Next(-1, 2);
                    s_new.Y = s.Y + random.Next(-1, 2);
                    var blocked = structureSeeds.Any(s => s.X == s_new.X && s.Y == s_new.Y);
                    var outOfrange = s_new.X > 639 || s_new.X < 0 || s_new.Y > 479 || s_new.Y < 0;
                    if(!blocked && !outOfrange)
                    {
                        newStructureAtoms.Add(s_new);
                        nStructureAtoms++;
                        if (nStructureAtoms >= maxStructureAtoms)
                        {
                            break;
                        }
                    }
                    
                }
                structureSeeds.AddRange(newStructureAtoms);

                if (nStructureAtoms >= maxStructureAtoms)
                {
                    break;
                }
            }

            foreach(var a in structureSeeds)
            {
                Atoms.Add(a);
            }
        }

        public void InitializeMotorAtoms()
        {
            
        }

        public void InitializeEnergySources()
        {
            int maxEnergySources = 100;
            int nEnergySources = random.Next(maxEnergySources);

            for(int i=0;i<nEnergySources;i++)
            {
                var es = new EnergySource();
                es.X = random.Next(640);
                es.Y = random.Next(480);
                es.PowerOutput = random.NextDouble();
                if(PositionIsFree(es.X, es.Y))
                {
                    Atoms.Add(es);
                }
            }
        }

        public bool PositionIsFree(int x, int y)
        {
            return !Atoms.Any(a => a.X== x && a.Y == y);
        }

        public void InitializeInfoAtoms()
        {

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
