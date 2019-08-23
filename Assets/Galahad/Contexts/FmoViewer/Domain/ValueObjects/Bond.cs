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
        [SerializeField] private int atomSerialNumber;
        [SerializeField] private int bondNumber;
        [SerializeField] private string electron;
        public Bond(int bondNumber,string atomName,bool electron)//if bool is true,this atom get electron:Ca.false means lose electron:Co 
        {
            BondNumber = bondNumber;
            AtomName = atomName;
            Electron = electron;
        }
        public int BondNumber { get; set; }
        public string AtomName { get; set; }
        public AtomSerialNumber AtomSerialNumber { get; set; }
        public bool Electron { get; set; }
        public void OnBeforeSerialize()
        {
            atomName = AtomName ?? "??";
            bondNumber = BondNumber;
            electron = Electron.ToString();
            atomSerialNumber = AtomSerialNumber?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            BondNumber = bondNumber;
            Electron=bool.Parse(electron);
            AtomName = atomName;
            AtomSerialNumber=new AtomSerialNumber(atomSerialNumber);
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
            _bonds.Exists(x => x.AtomSerialNumber.Value == atom.AtomSerialNumber.Value);

        public Bonds Add(Bond bond)
        {
            _bonds.Add(bond);
            return new Bonds(_bonds);
        }

        public bool Contains()
        {
            return _bonds.Count != 0;
        }

        public AtomSerialNumber False() => _bonds.FirstOrDefault(x => x.Electron == false)?.AtomSerialNumber;
        public AtomSerialNumber True() => _bonds.FirstOrDefault(x => x.Electron == true)?.AtomSerialNumber;
        public int BondNum() => _bonds.Count(x => x.Electron == true);
    }
}