using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Domain.MoleculeAggregate
{
    [Serializable]
    public class Atom : ISerializationCallbackReceiver
    {
        [SerializeField] private AtomicNumber atomicNumber;
        [SerializeField] private int atomIndex;
        [SerializeField] private int formalCharge;
        [SerializeField] private Vector3 position;

        public Atom(AtomIndex id, AtomicNumber atomicNumber, Position position, FormalCharge formalCharge)
        {
            AtomIndex = id;
            AtomicNumber = atomicNumber;
            Position = position;
            FormalCharge = formalCharge;
        }

        public AtomIndex AtomIndex { get; private set; }
        public Position Position { get; private set; }
        public FormalCharge FormalCharge { get; private set; }
        public AtomicNumber AtomicNumber { get; private set; }


        public void OnBeforeSerialize()
        {
            atomIndex = AtomIndex?.Value ?? 0;
            atomicNumber = AtomicNumber;
            position = Position?.Value ?? Vector3.zero;
            formalCharge = FormalCharge?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            AtomIndex = new AtomIndex(atomIndex);
            AtomicNumber = atomicNumber;
            Position = new Position(position);
            FormalCharge = new FormalCharge(formalCharge);
        }
    }

    public class Atoms
    {
        private readonly List<Atom> _atoms;

        public Atoms(List<Atom> atoms)
        {
            _atoms = atoms;
        }

        public Atom this[AtomIndex index] => _atoms.FirstOrDefault(x => x.AtomIndex.Value == index.Value);

        public List<Atom> ToList()
        {
            return _atoms;
        }
    }
}