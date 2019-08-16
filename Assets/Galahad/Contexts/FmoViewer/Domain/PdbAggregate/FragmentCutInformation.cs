using System;
using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class FragmentCutInformation:ISerializationCallbackReceiver
    {
        [SerializeField] private List<FragmentCut> fragmentCuts;
        [SerializeField] private List<FragmentBond> fragmentBonds;
        public FragmentCutInformation(FragmentCuts fragmentCuts, FragmentBonds fragmentBonds)
        {
            FragmentBonds = fragmentBonds;
            FragmentCuts = fragmentCuts;
        }
        public FragmentCutInformation()
        public FragmentCutInformation():this(new FragmentCuts(),new FragmentBonds() ){}
        public FragmentCutInformation(FragmentCuts fragmentCuts):this(){}

        public FragmentCuts FragmentCuts{ get; private set; }
        public FragmentBonds FragmentBonds{ get; private set; }
        public void OnBeforeSerialize()
        {
            fragmentCuts = FragmentCuts.ToList();
            fragmentBonds = FragmentBonds.ToList();
        }

        public void OnAfterDeserialize()
        {
            FragmentCuts=new FragmentCuts(fragmentCuts);
            FragmentBonds=new FragmentBonds(fragmentBonds);
        }
        
    }
}