using System;
using System.Collections.Generic;
using Galahad.MoleculeViewerContext.Domain.ValueObject;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Domain.MoleculeAggregate
{
    [Serializable]
    public class Bond : ISerializationCallbackReceiver
    {
        [SerializeField] private int beginAtomIndex;
        [SerializeField] private int bondIndex;
        [SerializeField] private int bondOrder;
        [SerializeField] private int endAtomIndex;

        public Bond(AtomIndex beginAtomIndex, AtomIndex endAtomIndex, BondOrder bondOrder)
        {
            BeginAtomIndex = beginAtomIndex;
            EndAtomIndex = endAtomIndex;
            BondOrder = bondOrder;
        }

        public BondIndex BondIndex { get; private set; }
        public BondOrder BondOrder { get; private set; }
        public AtomIndex BeginAtomIndex { get; private set; }
        public AtomIndex EndAtomIndex { get; private set; }

        public void OnBeforeSerialize()
        {
            bondIndex = BondIndex?.Value ?? 0;
            beginAtomIndex = BeginAtomIndex?.Value ?? 0;
            endAtomIndex = EndAtomIndex?.Value ?? 0;
            bondOrder = BondOrder?.Value ?? 1;
        }

        public void OnAfterDeserialize()
        {
            BondIndex = new BondIndex(bondIndex);
            BeginAtomIndex = new AtomIndex(beginAtomIndex);
            EndAtomIndex = new AtomIndex(endAtomIndex);
            BondOrder = new BondOrder(bondOrder);
        }
    }

    public class Bonds
    {
        private readonly List<Bond> _bonds;

        public Bonds(List<Bond> bonds)
        {
            _bonds = bonds;
        }

        public Bond this[int i] => _bonds[i];

        public List<Bond> ToList()
        {
            return _bonds;
        }
    }
}