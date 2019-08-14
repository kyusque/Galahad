using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class FragmentCut:ISerializationCallbackReceiver
    {
        [SerializeField] private List<Atom> atoms;
        [SerializeField] private List<Hetatm> hetatms;
        [SerializeField] private int fragmentId;
        [SerializeField] private int residueSequencsNumber;
  
        public FragmentCut()
        {
            
        }
        public FragmentCut(FragmentId fragmentId, Atoms atoms,Hetatms hetatms, ResidueSequencsNumber residueSequencsNumber)
        {
            FragmentId = fragmentId;
            Atoms = atoms;
            ResidueSequencsNumber = residueSequencsNumber;
            Hetatms = hetatms;
        }
        public FragmentCut(FragmentId fragmentId):this(fragmentId,new Atoms() ,new Hetatms(),new ResidueSequencsNumber(0) )
        {
        }

        public FragmentCut(FragmentId fragmentId, Atoms atoms) : this(fragmentId)
        {
            Atoms=atoms;
        }
        public FragmentCut(FragmentId fragmentId, Hetatms hetatms,ResidueSequencsNumber residueSequencsNumber):this(fragmentId)
        {
            Hetatms = hetatms;
            ResidueSequencsNumber = residueSequencsNumber;
        }
       

        public Atoms Atoms{ get; protected internal set; }
        public Hetatms Hetatms{ get; internal set; }
        public FragmentId FragmentId{ get; private set; }
        public ResidueSequencsNumber ResidueSequencsNumber{ get; private set; }
        public void OnBeforeSerialize()
        {
            atoms = Atoms?.ToList()?? new List<Atom>();
            hetatms = Hetatms?.ToList()??new List<Hetatm>();
            fragmentId = FragmentId?.Value ?? 0;
            residueSequencsNumber = ResidueSequencsNumber?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            Atoms=new Atoms(atoms);
            Hetatms=new Hetatms(hetatms);
            FragmentId=new FragmentId(fragmentId);
            ResidueSequencsNumber=new ResidueSequencsNumber(residueSequencsNumber);
        }

        public FragmentCut Add(Atom atom)
        {
            Atoms.Add(atom);
            return new FragmentCut(FragmentId,Atoms,Hetatms,ResidueSequencsNumber);
        }

        public FragmentCut Add(Hetatm hetatm)
        {
            Hetatms.Add(hetatm);
            return this;
        }

        public FragmentCut Remove(Atom atom)
        {
            Atoms.Remove(atom);
            return new FragmentCut(FragmentId,Atoms,Hetatms,ResidueSequencsNumber);
        }

        public FragmentCut Remove(Hetatm hetatm)
        {
            Hetatms.Remove(hetatm);
            return this;
        }

        public bool Exisits(ResidueName residueName)
        {
            return Atoms.Exists(residueName);
        }
    }
    public class FragmentCuts
    {
        private readonly List<FragmentCut> _fragmentCuts;
        

        public FragmentCuts(List<FragmentCut> fragmentCuts)
        {
            _fragmentCuts = fragmentCuts;
        }

        public FragmentCuts() : this(new List<FragmentCut>())
        {
            
        }

        public FragmentCut this[int fragmentId] => _fragmentCuts[fragmentId - 1];

        public FragmentCut this[ResidueSequencsNumber residueSequencsNumber] =>
            _fragmentCuts.FirstOrDefault(x => x.ResidueSequencsNumber.Value == residueSequencsNumber.Value);

        public List<FragmentCut> ToList()
        {
            return _fragmentCuts;
        }

        public FragmentId FragmentId(Atom atom)
        {
            Debug.Log(_fragmentCuts.Find(x => x.Atoms.Exists(atom)).FragmentId.Value);
            return _fragmentCuts.Find(x => x.Atoms.Exists(atom)).FragmentId;
        }

        public FragmentCuts Add(FragmentCut fragmentCut)
        {
            _fragmentCuts.Add(fragmentCut);
            return new FragmentCuts(_fragmentCuts);
        }


        public bool Contains(FragmentCut fragmentCut)
        {
            return _fragmentCuts.Contains(fragmentCut);
        }

        public FragmentCuts BeforeCutAtoms(FragmentId fragmentId)
        {
            if(!_fragmentCuts[fragmentId.Value-1].Atoms.Contains())return new FragmentCuts(_fragmentCuts);
            if (_fragmentCuts[fragmentId.Value-1].Atoms.Exists(ResidueName.GLY))
            {
                return new FragmentCuts(_fragmentCuts);
            }
            _fragmentCuts.Add(new FragmentCut(new FragmentId(_fragmentCuts.Count+1)));
            for (var i =_fragmentCuts.Count-1; i > fragmentId.Value; i--)
            {
                
                _fragmentCuts[i].Atoms.MoveTo(_fragmentCuts[i - 1].Atoms);
                _fragmentCuts[i].Hetatms.MoveTo(_fragmentCuts[i - 1].Hetatms);
            }
            return new FragmentCuts(_fragmentCuts);
        }

        public FragmentCuts AtomMoveToNextFragmentCut(Atom atom,FragmentId fragmentId)
        {
             _fragmentCuts[fragmentId.Value-1].Remove(atom);
             _fragmentCuts[fragmentId.Value].Add(atom);
             return new FragmentCuts(_fragmentCuts);
        }

        public int Count()
        {
            return _fragmentCuts.Count;
        }
    }
}