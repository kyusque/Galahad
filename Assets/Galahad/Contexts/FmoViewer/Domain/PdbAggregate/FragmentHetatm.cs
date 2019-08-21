using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class FragmentHetatm:ISerializationCallbackReceiver
    {
        [SerializeField] private string name;
        [SerializeField] private int fragmentId;
        [SerializeField] private int residueSequencsNumber;
        [SerializeField] private List<Hetatm> hetatms;

        
        public FragmentHetatm( ResidueSequencsNumber residueSequencsNumber,
            Hetatms hetatms,FragmentId fragmentId)
        {
            ResidueSequencsNumber = residueSequencsNumber;
            Hetatms = hetatms;
            FragmentId = fragmentId;
        }
        public FragmentHetatm(ResidueSequencsNumber residueSequencsNumber,Hetatms hetatms)
            :this(residueSequencsNumber,hetatms,new FragmentId())
        {
        }
        public FragmentHetatm( ResidueSequencsNumber residueSequencsNumber) : this(residueSequencsNumber,new Hetatms())
        {
        }
        public FragmentHetatm():this(new ResidueSequencsNumber()){}
        public FragmentId FragmentId { get; set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; set; }
        public Hetatms Hetatms { get; set; }
        public Bonds Bonds { get; set; }

        public FragmentHetatm Add(Hetatm hetatm)
        {
            Hetatms.Add(hetatm);
            return new FragmentHetatm(hetatm.ResidueSequencsNumber,Hetatms);
        }
        public void OnBeforeSerialize()
        {
            name = Hetatms.HetatmResidueName().Value;
            fragmentId = FragmentId?.Value ?? -1;
            residueSequencsNumber = ResidueSequencsNumber?.Value ?? -1;
            hetatms = Hetatms?.ToList()??new List<Hetatm>();
        }

        public void OnAfterDeserialize()
        {
            FragmentId=new FragmentId(fragmentId);
            ResidueSequencsNumber=new ResidueSequencsNumber(residueSequencsNumber);
            Hetatms=new Hetatms(hetatms);
        }
    }

    public class FragmentHetatms
    {
        private List<FragmentHetatm> _fragmentHetatms;

        public FragmentHetatms(List<FragmentHetatm> fragmentHetatms)
        {
            _fragmentHetatms = fragmentHetatms;
        }
        public FragmentHetatms():this(new List<FragmentHetatm>()){}

        public FragmentHetatm this[ResidueSequencsNumber residueSequencsNumber]
            => _fragmentHetatms.FirstOrDefault(x => x.ResidueSequencsNumber.Value == residueSequencsNumber.Value);

        public List<FragmentHetatm> ToList() => _fragmentHetatms;

        public FragmentHetatms Add(ResidueSequencsNumber residueSequencsNumber)
        {
            _fragmentHetatms.Add(new FragmentHetatm(residueSequencsNumber));
            return new FragmentHetatms(_fragmentHetatms);
        }
        public bool Exists(ResidueSequencsNumber residueSequencsNumber)
        {
            return _fragmentHetatms.Exists(x => x.ResidueSequencsNumber.Value == residueSequencsNumber.Value);
        }

    }
}