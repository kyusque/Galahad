using System;
using System.Collections.Generic;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.FmoTargetAggregate
{
    [Serializable]
    public class FmoTarget:ISerializationCallbackReceiver
    {
        [SerializeField] private List<Fragment> fragments;
        public Fragments Fragments { get; private set; }
        public FmoTarget(Fragments fragments)
        {
            Fragments = fragments;
        }

        public void OnBeforeSerialize()
        {
            fragments = Fragments?.ToList()??new List<Fragment>();
        }

        public void OnAfterDeserialize()
        {
            Fragments=new Fragments(fragments);
        }
    }
}