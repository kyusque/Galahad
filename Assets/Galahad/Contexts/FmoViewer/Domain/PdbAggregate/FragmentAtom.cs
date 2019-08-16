using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    public class FragmentAtom:ISerializationCallbackReceiver
    {
        [SerializeField] private int fragmentId;
        [SerializeField] private int residuwSequencsNumber;
        [SerializeField] private List<Atom> atoms;
        public FragmentAtom(){}
        public FragmentAtom(FragmentId fragmentId,ResidueSequencsNumber residueSequencsNumber, Atoms atoms):this()
        {
            FragmentId = fragmentId;
            ResidueSequencsNumber = residueSequencsNumber;
            Atoms = atoms;
        }

        public FragmentId FragmentId { get; set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; set; }
        public Atoms Atoms { get; set; }


        public void OnBeforeSerialize()
        {
            fragmentId = FragmentId?.Value ?? -1;
            residuwSequencsNumber = ResidueSequencsNumber?.Value ?? -1;
            atoms = Atoms?.ToList() ?? new List<Atom>();
        }

        public void OnAfterDeserialize()
        {
            FragmentId=new FragmentId(fragmentId);
            ResidueSequencsNumber=new ResidueSequencsNumber(residuwSequencsNumber);
            Atoms=new Atoms(atoms);
        }
    }

    public class FragmentAtoms
    {
        private List<FragmentAtom> _fragmentAtoms;
        public FragmentAtoms(){}

        public FragmentAtoms(List<FragmentAtom> fragmentAtom)
        {
            _fragmentAtoms = fragmentAtom;
        }
        public 
    }
}