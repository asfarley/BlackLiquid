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
        private AtomCollection atoms = new AtomCollection();
        public AtomCollection Atoms
        {
            get { return atoms; }
            set { atoms = value; NotifyPropertyChanged(); }
        }

        private Random random = new Random();
        public void Update()
        {
            var deltas = new List<AtomsDelta>();
            foreach(var atom in atoms)
            {
                var ad = atom.Update(atoms);
                deltas.Add(ad);
            }

            foreach(var ad in deltas)
            {
                foreach (var a in ad.NewAtoms)
                {
                    Atoms.Add(a);
                }
                foreach (var a in ad.DeletedAtoms)
                {
                    Atoms.Remove(a);
                }
            }

            for (int i=0;i<10000;i++)
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

            int maxStructureAtoms = (int)( GlobalConstants.Width * GlobalConstants.Height * 0.1);

            var structureSeeds = new List<Atom>();

            for (int i = 0; i < numStructureSeeds; i++)
            {
                var sa = new StructureAtom();
                sa.X = random.Next(GlobalConstants.Width);
                sa.Y = random.Next(GlobalConstants.Height);
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
                    var outOfrange = s_new.X >= GlobalConstants.Width || s_new.X < 0 || s_new.Y >= GlobalConstants.Height || s_new.Y < 0;
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
            int maxMotorAtoms = 1000;
            int minMotorAtoms = 200;
            int nMotorAtoms = random.Next(minMotorAtoms, maxMotorAtoms);

            for (int i = 0; i < nMotorAtoms; i++)
            {
                var es = new MotorAtom();
                es.X = random.Next(GlobalConstants.Width);
                es.Y = random.Next(GlobalConstants.Height);
                es.energy = 20;
                es.energyMax = 20;
                if (Atoms.PositionIsFree(es.X, es.Y, GlobalConstants.Width, GlobalConstants.Height))
                {
                    Atoms.Add(es);
                }
            }
        }

        public void InitializeEnergySources()
        {
            int maxEnergySources = 200;
            int minEnergySources = 50;
            int nEnergySources = random.Next(minEnergySources, maxEnergySources);

            for(int i=0;i<nEnergySources;i++)
            {
                var es = new EnergySource();
                es.X = random.Next(GlobalConstants.Width);
                es.Y = random.Next(GlobalConstants.Height);
                es.PowerOutput = random.NextDouble();
                if(Atoms.PositionIsFree(es.X, es.Y, GlobalConstants.Width, GlobalConstants.Height))
                {
                    Atoms.Add(es);
                }
            }
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

            var ad = a1.Interact(a2, Atoms);

            foreach(var a in ad.NewAtoms)
            {
                Atoms.Add(a);
            }

            foreach(var a in ad.DeletedAtoms)
            {
                Atoms.Remove(a);
            }
        }
    }
}
