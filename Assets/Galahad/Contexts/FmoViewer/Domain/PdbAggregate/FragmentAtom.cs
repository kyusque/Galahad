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
        [SerializeField] private string residueName;
        [SerializeField] private int fragmentId;
        [SerializeField] private int residuwSequencsNumber;
        [SerializeField] private List<Bond> bonds;
        [SerializeField] private List<Atom> atoms;
        
        public FragmentAtom(FragmentId fragmentId, ResidueSequencsNumber residueSequencsNumber, Atoms atoms,
            Bonds bonds)
        {
            FragmentId = fragmentId;
            ResidueSequencsNumber = residueSequencsNumber;
            Atoms = atoms;
            Bonds = bonds;
        }
        public FragmentAtom(FragmentId fragmentId,ResidueSequencsNumber residueSequencsNumber, Atoms atoms):this(fragmentId,residueSequencsNumber,atoms,new Bonds())
        {
        }

        public FragmentAtom(FragmentId fragmentId):this(fragmentId,new ResidueSequencsNumber(),new Atoms(),new Bonds()){}
        
        public FragmentAtom():this(new FragmentId(),new ResidueSequencsNumber(),new Atoms(),new Bonds()){}

        public FragmentId FragmentId { get; set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; set; }
        public Atoms Atoms { get; set; }
        public Bonds Bonds { get; set; }
        public State State { get; set; }

        public FragmentAtom Init()
        {
            this.State=new State();
            return this;
        }

        public FragmentAtom Inject(Bonds Bonds)
        {
            this.Bonds = Bonds;
            return this;
        }
        public FragmentAtom Add(Bond bond)
        {
            this.Bonds.Add(bond);
            return this;
        }

        public string ResidueName => residueName;

        public FragmentAtom Add(Atom atom)
        {
            Atoms.Add(atom);
            return new FragmentAtom(FragmentId,ResidueSequencsNumber,Atoms,Bonds);
        }

        public FragmentAtom Add(FragmentAtom fragmentAtom)
        {
            Atoms = fragmentAtom.Atoms;
            ResidueSequencsNumber = new ResidueSequencsNumber(fragmentAtom.ResidueSequencsNumber.Value);
            return new FragmentAtom(FragmentId,ResidueSequencsNumber,Atoms,Bonds);
        }

        public FragmentAtom Remove(Atom atom)
        {
            return new FragmentAtom(FragmentId,ResidueSequencsNumber,Atoms.Remove(atom));
        }

        public FragmentAtom Remove()
        {
            ResidueSequencsNumber=new ResidueSequencsNumber(-1);
            Atoms=new Atoms();
            Bonds=new Bonds();
            return new FragmentAtom(FragmentId,ResidueSequencsNumber,Atoms,Bonds);
        }
        public void OnBeforeSerialize()
        {
            residueName = Atoms.ResidueName().ToString();
            fragmentId = FragmentId?.Value ?? -1;
            residuwSequencsNumber = ResidueSequencsNumber?.Value ?? -1;
            atoms = Atoms?.ToList() ?? new List<Atom>();
            bonds = Bonds?.Tolist() ?? new List<Bond>();
        }
        public void OnAfterDeserialize()
        {
            FragmentId=new FragmentId(fragmentId);
            ResidueSequencsNumber=new ResidueSequencsNumber(residuwSequencsNumber);
            Atoms=new Atoms(atoms);
            Bonds=new Bonds(bonds);
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
        public FragmentAtom AddCa(FragmentId fragmentId, Atom atomCa)
        {
            _fragmentAtoms.Add(new FragmentAtom(fragmentId,atomCa.ResidueSequencsNumber,new Atoms().Add(atomCa)));
            return _fragmentAtoms[_fragmentAtoms.Count-1];
        }

        private FragmentAtoms AtomsMoveToNextFragmentAtoms(FragmentId fragmentId)
        {
            this[new FragmentId(fragmentId.Value+1)].Add(this[fragmentId]).Inject(this[fragmentId].Bonds);
            this[fragmentId].Remove();
            return this;
        }
        public FragmentAtoms AtomMoveToNext(FragmentId fragmentId,Atom atomCo)
        {
            this[new FragmentId(fragmentId.Value + 1)].Add(atomCo);
            this[fragmentId].Remove(atomCo);
            return this;
        }

        public FragmentAtoms AddLastNew()
        {
            _fragmentAtoms.Add(new FragmentAtom());
            _fragmentAtoms[_fragmentAtoms.Count-1].FragmentId=new FragmentId(_fragmentAtoms.Count);
            return new FragmentAtoms(_fragmentAtoms);
        }

        public FragmentAtoms MoveOther(FragmentId fragmentId)
        {
            if (_fragmentAtoms.Count==fragmentId.Value)
            {
                return this;
            }
            var frag = _fragmentAtoms.Count;
            for (var i = 0; i <frag-fragmentId.Value-1; i++)
            {
                this.AtomsMoveToNextFragmentAtoms(new FragmentId(frag - i - 1));
            }

            return this;
        }
        public Bonds OneResidueCut(FragmentId fragmentId,int bondNumber)
        {
            var atoms = this[fragmentId].Atoms.Count();
            var bonds=new Bonds();
            for (var i = 0; i < atoms; i++)
            {
                switch (this[fragmentId].Atoms[atoms-i-1].AtomName)
                {
                    case AtomName.CA:
                        this[fragmentId].Bonds.Add(new Bond(bondNumber, this[fragmentId].Atoms[atoms - i - 1], false));
                        bonds.Add(new Bond(bondNumber, this[fragmentId].Atoms[atoms - i - 1], false));
                        break;
                    case AtomName.HA:
                        break;
                    case AtomName.C:
                        break;
                    case AtomName.H:
                        break;
                    case AtomName.N:
                        break;
                    case AtomName.H1:
                        break;
                    case AtomName.H2:
                        break;
                    case AtomName.H3:
                        break;
                    case AtomName.O:
                        break;
                    case AtomName.OX:
                        break;
                    case AtomName.CB:
                        this[new FragmentId(fragmentId.Value + 1)].ResidueSequencsNumber =
                            this[fragmentId].Atoms[atoms - i - 1].ResidueSequencsNumber;
                        this[new FragmentId(fragmentId.Value + 1)].Bonds.Add(new Bond(bondNumber,
                            this[fragmentId].Atoms[atoms - i - 1], false));
                        bonds.Add(new Bond(bondNumber,
                            this[fragmentId].Atoms[atoms - i - 1], false));
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.CD:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.CE:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.CG:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.CH:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.CZ:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HB:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HC:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HD:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HE:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HG:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HH:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HZ:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.ND:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.NE:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.NH:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.NZ:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.OD:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.OE:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.OG:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.OH:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.SD:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.SG:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HD1:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HD2:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HE2:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HG1:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HG2:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HH1:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HH2:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    case AtomName.HH3:
                        AtomMoveToNext(fragmentId, this[fragmentId].Atoms[atoms - i - 1]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return bonds;
        }
        

    }
}