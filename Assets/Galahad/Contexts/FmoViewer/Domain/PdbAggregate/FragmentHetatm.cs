using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    public class FragmentHetatm:ISerializationCallbackReceiver
    {
        [SerializeField] private int fragmentId;
        [SerializeField] private int residueSequencsNumber;
        [SerializeField] private List<Hetatm> hetatms;
        public FragmentHetatm(){}

        public FragmentHetatm(FragmentId fragmentId, ResidueSequencsNumber residueSequencsNumber,
            Hetatms hetatms) : this()
        {
            FragmentId = fragmentId;
            ResidueSequencsNumber = residueSequencsNumber;
            Hetatms = hetatms;
        }
        public FragmentId FragmentId { get; set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; set; }
        public Hetatms Hetatms { get; set; }
        public void OnBeforeSerialize()
        {
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
}