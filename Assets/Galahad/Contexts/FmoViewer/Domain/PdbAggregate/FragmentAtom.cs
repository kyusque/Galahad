using System.Collections.Generic;
using System.Linq;
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

        public FragmentAtom(FragmentId fragmentId):this(){}
        

        public FragmentId FragmentId { get; set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; set; }
        public Atoms Atoms { get; set; }
        
        public FragmentAtom Add(Atom atom)
        {
            Atoms.Add(atom);
            return new FragmentAtom(FragmentId,ResidueSequencsNumber,Atoms);
        }


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

        public FragmentAtom this[ResidueSequencsNumber residueSequencsNumber] =>
            _fragmentAtoms.FirstOrDefault(x => x.ResidueSequencsNumber.Value == residueSequencsNumber.Value);

        public List<FragmentAtom> ToList() => _fragmentAtoms;


        public FragmentAtoms AddCa(int fragmentId, Atom atomCa)
        {
            _fragmentAtoms.Add(new FragmentAtom(new FragmentId(fragmentId),new ResidueSequencsNumber(atomCa.ResidueSequencsNumber.Value),new Atoms().Add(atomCa)));
            return new FragmentAtoms(_fragmentAtoms);
        }

        public bool Contains(FragmentId fragmentId)
        {
            return true;
        }

    }
}