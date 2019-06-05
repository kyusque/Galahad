using System;
using System.Collections.Generic;
using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Domain.ValueObject
{
    [Serializable]
    public class Bond: ISerializationCallbackReceiver
    {
        [SerializeField] private int beforeAtomIndex;
        [SerializeField] private int endAtomIndex;
        
        private AtomIndex _beforeAtomIndex;
        private AtomIndex _endAtomIndex;

        public Bond(AtomIndex beforeAtomIndex, AtomIndex endAtomIndex)
        {
            _beforeAtomIndex = beforeAtomIndex;
            _endAtomIndex = endAtomIndex;
        }

        public void OnBeforeSerialize()
        {
            beforeAtomIndex = _beforeAtomIndex?.Value ?? 0;
            endAtomIndex = _endAtomIndex?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            _beforeAtomIndex = new AtomIndex(beforeAtomIndex);
            _endAtomIndex = new AtomIndex(endAtomIndex);
        }
    }

    public class Bonds
    {
        private readonly List<Bond> _bonds;

        public Bonds(List<Bond> bonds)
        {
            _bonds = bonds;
        }

        public List<Bond> ToList() => _bonds;
        
        public Bond this[int i] => _bonds[i];
    }
}