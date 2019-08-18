using System;
using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class FragmentBond:ISerializationCallbackReceiver
    {
        private readonly FragmentBond _fragmentBond;
        [SerializeField] private int fragmentIdCa;
        [SerializeField] private int fragmentIdCo;
        [SerializeField] private Atom ca;
        [SerializeField] private Atom co;
        public FragmentBond(){}
        public FragmentBond(FragmentId beginfragmentIdCa):this()
        {
            FragmentIdCa = beginfragmentIdCa;
        }

        public FragmentBond(FragmentId fragmentIdCa, Atom ca) : this(fragmentIdCa)
        {
            CA = ca;
        }

        public FragmentBond(FragmentId fragmentIdCa, Atom ca, FragmentId fragmentIdCo, Atom co) : this(fragmentIdCa, ca)
        {
            FragmentIdCo = fragmentIdCo;
            CO = co;
        }

        public FragmentId FragmentIdCa { get; private set; }
        public FragmentId FragmentIdCo { get; private set; }
        public Atom CA { get; set; } //give
        public Atom CO { get; set; } //get
        public FragmentBond AddGiveAtom(Atom ca)
        {
            CA = ca;
            return new FragmentBond(FragmentIdCa,CA);
        }
        public FragmentBond AddGetAtom(Atom co)
        {
            CO = co;
            return new FragmentBond(FragmentIdCa,CA,FragmentIdCo,CO);
        }

        public FragmentBond AddAtomBoth(Atom ca, Atom co)
        {
            CA = ca;
            CO = co;
            return new FragmentBond(FragmentIdCa, CA, FragmentIdCo, CO);
            
        }

        public void OnBeforeSerialize()
        {
            fragmentIdCa = FragmentIdCa?.Value??-1;
            fragmentIdCo = FragmentIdCo?.Value ?? -1;
            ca = CA;
            co = CO;
        }

        public void OnAfterDeserialize()
        {
            FragmentIdCa=new FragmentId(fragmentIdCa);
            FragmentIdCo=new FragmentId(fragmentIdCo);
            CA=ca;
            CO = co;
        }
    }

    public class FragmentBonds
    {
        private readonly List<FragmentBond> _fragmentBond;

        public FragmentBonds(List<FragmentBond> fragmentBonds)
        {
            _fragmentBond = fragmentBonds;
        }

        public FragmentBonds() : this(new List<FragmentBond>())
        {
            
        }

        public List<FragmentBond> ToList()
        {
            return _fragmentBond;
        }

        public FragmentBond this[FragmentId fragmentId] => _fragmentBond.Find(x => x.FragmentIdCa.Value == fragmentId.Value);

        public FragmentBonds Add(FragmentId fragmentId)
        {
            if (_fragmentBond.Exists(x => x.FragmentIdCa == fragmentId)) return new FragmentBonds(_fragmentBond);
            _fragmentBond.Add(new FragmentBond(fragmentId));
            return new FragmentBonds(_fragmentBond);
        }


        public FragmentBonds AddCa(Atom atom,FragmentId fragmentId)
        {
            Add(fragmentId);
            this[fragmentId].AddGiveAtom(atom);
            return this;
        }

        
        
    }
}