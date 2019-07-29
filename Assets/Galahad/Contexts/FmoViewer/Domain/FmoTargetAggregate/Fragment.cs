using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.FmoTargetAggregate
{
    [Serializable]
    public class Fragment: ISerializationCallbackReceiver
    {
        [SerializeField] private int fragmentId;
        [SerializeField] private List<Atom> atoms;

        public FragmentId FragmentId { get; private set; }
        public Atoms Atoms { get; private set; }
        public Fragment(FragmentId fragmentId, Atoms atoms)
        {
            FragmentId = fragmentId;
            Atoms = atoms;
        }

        public void OnBeforeSerialize()
        {
            fragmentId = FragmentId?.Value??0;
            atoms = Atoms?.ToList() ?? new List<Atom>();
        }

        public void OnAfterDeserialize()
        {
            FragmentId=new FragmentId(fragmentId);
            Atoms=new Atoms(atoms);
        }
    }
    public class Fragments
    {
        public readonly List<Fragment> _Fragments;
        public Fragments(List<Fragment> fragments)
        {
            _Fragments = fragments;
        }
        public Fragment this[FragmentId index] => _Fragments.FirstOrDefault(x => x.FragmentId.Value == index.Value);
        public List<Fragment> ToList()
        {
            return _Fragments;
        }
    }
    
}