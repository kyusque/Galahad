using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class Pdb:ISerializationCallbackReceiver
    {
        [SerializeField] private List<Atom> atoms;
        [SerializeField] private List<Hetatm> hetatms;
        public Pdb(Atoms atoms)
        {
            Atoms = atoms;
        }

        public Pdb(Atoms atoms, Hetatms hetatms) : this(atoms)
        {
            Hetatms = hetatms;
        }
        public Atoms Atoms { get; private set; }

        public Hetatms Hetatms{get; private set;}

        public  int TotalCharge()
        {
            var totalCharge = 0;
            foreach (var atom in atoms)
            {
                totalCharge += atom.FormalCharge.Value;
            }

            foreach (var hetatm in hetatms)
            {
                totalCharge += hetatm.FormalCharge.Value;
            }
            return totalCharge;
        }

        public int FirstFragmentId()
        {
            return Atoms.ToList().First().ResidueSequencsNumber.Value;
        }

        public int DefaultFragmentMount()
        {
            return Atoms.ToList().Last().ResidueSequencsNumber.Value - Atoms.ToList().First().ResidueSequencsNumber.Value 
                   + Hetatms.ToList().Last().ResidueSequencsNumber.Value-Hetatms.ToList().First().ResidueSequencsNumber.Value+2;
        }

        public void OnBeforeSerialize()
        {
            atoms = Atoms?.ToList()??new List<Atom>();
            hetatms = Hetatms?.ToList()??new List<Hetatm>();
        }

        public void OnAfterDeserialize()
        {
            Atoms=new Atoms(atoms);
            Hetatms=new Hetatms(hetatms);
        }
    }

    public enum RecordName
    {
        ATOM,
        HETATM
        
    }
}