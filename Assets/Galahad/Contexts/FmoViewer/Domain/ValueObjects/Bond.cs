using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    [Serializable]
    public class Bond:ISerializationCallbackReceiver
    {
        [SerializeField] private string atomName;
        [SerializeField] private int bondNumber;
        [SerializeField] private string electron;
        public Bond(int bondNumber,Atom atom,bool electron)//if bool is true,this atom get electron:Ca.false means lose electron:Co 
        {
            BondNumber = bondNumber;
            Atom = atom;
            Electron = electron;
        }
        public int BondNumber { get; }
        public Atom Atom { get; }
        public bool Electron { get; }
        public void OnBeforeSerialize()
        {
            atomName = Atom?.ToString() ?? "??";
            bondNumber = BondNumber;
            electron = Electron.ToString();
        }

        public void OnAfterDeserialize()
        {
            throw new NotImplementedException();
        }
    }

    public class Bonds
    {
        private readonly List<Bond> _bonds;

        public Bonds(List<Bond> bonds)
        {
            _bonds = bonds;
        }
        public Bonds():this(new List<Bond>()){}
        public List<Bond> Tolist() => _bonds;

        public bool Contains(Atom atom) =>
            _bonds.Exists(x => x.Atom.AtomSerialNumber.Value == atom.AtomSerialNumber.Value);

        public Bonds Add(Bond bond)
        {
            _bonds.Add(bond);
            return new Bonds(_bonds);
        }

        public bool Contains()
        {
            return _bonds.Count != 0;
        }

        public Atom False() => _bonds.FirstOrDefault(x => x.Electron == false)?.Atom;
        public Atom True() => _bonds.FirstOrDefault(x => x.Electron == true)?.Atom;
        public int BondNum() => _bonds.Count(x => x.Electron == true);
    }
}