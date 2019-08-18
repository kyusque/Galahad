using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
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

        public FragmentAtom Remove(Atom atom)
        {
            return new FragmentAtom(FragmentId,ResidueSequencsNumber,Atoms.Remove(atom));
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

        public FragmentAtoms(List<FragmentAtom> fragmentAtom)
        {
            _fragmentAtoms = fragmentAtom;
        }
        public FragmentAtoms():this(new List<FragmentAtom>()){}

        public FragmentAtom this[ResidueSequencsNumber residueSequencsNumber] =>
            _fragmentAtoms.FirstOrDefault(x => x.ResidueSequencsNumber.Value == residueSequencsNumber.Value);

        public FragmentAtom this[FragmentId fragmentId] =>
            _fragmentAtoms.FirstOrDefault(x => x.FragmentId.Value == fragmentId.Value);

        public List<FragmentAtom> ToList() => _fragmentAtoms;
        public int Count() => _fragmentAtoms.Count;
        public bool Contains(FragmentId fragmentId) =>
            _fragmentAtoms.Exists(x => x.FragmentId.Value == fragmentId.Value);
        public FragmentAtoms AddCa(FragmentId fragmentId, Atom atomCa)
        {
            _fragmentAtoms.Add(new FragmentAtom(fragmentId,atomCa.ResidueSequencsNumber,new Atoms().Add(atomCa)));
            return new FragmentAtoms(_fragmentAtoms);
        }

        public FragmentAtoms CutFragmentAtoms(FragmentId fragmentId, Atoms atomsCa, Atoms atomsCo, Atom atomCa, Atom atomCo)
        {
            this.MakeNewFragmentAtom().MoveOthersCut(fragmentId);
            
        }

        private FragmentAtoms MakeNewFragmentAtom()
        {
            _fragmentAtoms.Add(new FragmentAtom(new FragmentId(_fragmentAtoms.Count+1)));
            return new FragmentAtoms(_fragmentAtoms);
        }

        private FragmentAtoms MoveOthersCut(FragmentId fragmentId)
        {
            var n = _fragmentAtoms.Count;
            for (var i = 0; i <n-fragmentId.Value+1; i++)
            {
                _fragmentAtoms[n - 1 - i] = _fragmentAtoms[n - 2 - i];
            }
            return new FragmentAtoms(_fragmentAtoms);
        }

        FragmentAtoms AtomsMovetTo(FragmentAtom atom)
        {
            
        }

        public FragmentAtoms MoveToNext(FragmentId fragmentId,Atoms atomsCa, Atoms atomsCo, Atom atomCa, Atom atomCo)
        {
            this[fragmentId]
        }
        
     }
}