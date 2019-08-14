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
        [SerializeField] private int fragmentId;
        [SerializeField] private Atom ca;
        [SerializeField] private Atom co;
        public FragmentBond(FragmentId beginfragmentId)
        {
            FragmentId = beginfragmentId;
        }

        public FragmentBond(FragmentId fragmentId, Atom ca) : this(fragmentId)
        {
            CA = ca;
        }
        public FragmentBond(FragmentId fragmentId,Atom ca, Atom co):this(fragmentId,ca)
        {
            CO = co;
        }
        public FragmentBond(){}
        public FragmentId FragmentId { get; private set; }
        public Atom CA { get; set; } //give
        public Atom CO { get; set; } //get
        public FragmentBond AddGiveAtom(Atom ca)
        {
            CA = ca;
            return new FragmentBond(FragmentId,CA);
        }
        public FragmentBond AddGetAtom(Atom co)
        {
            CO = co;
            return new FragmentBond(FragmentId,CA,CO);
        }

        public FragmentBond AddAtomBoth(Atom ca, Atom co)
        {
            CA = ca;
            CO = co;
            return  new FragmentBond(FragmentId,CA,CO);;
        }

        public void OnBeforeSerialize()
        {
            fragmentId = FragmentId?.Value??0;
            ca = CA;
            co = CO;
        }

        public void OnAfterDeserialize()
        {
            FragmentId=new FragmentId(fragmentId);
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

        public FragmentBond this[FragmentId fragmentId] => _fragmentBond.Find(x => x.FragmentId.Value == fragmentId.Value);

        public FragmentBonds Add(FragmentId fragmentId)
        {
            if (_fragmentBond.Exists(x => x.FragmentId == fragmentId)) return new FragmentBonds(_fragmentBond);
            _fragmentBond.Add(new FragmentBond(fragmentId));
            return new FragmentBonds(_fragmentBond);
        }

        
        
    }
}