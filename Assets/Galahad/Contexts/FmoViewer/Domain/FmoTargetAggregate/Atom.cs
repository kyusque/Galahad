using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.FmoTargetAggregate
{
    [Serializable]
    public class Atom:ISerializationCallbackReceiver
    {
        [SerializeField] private int atomId;
        [SerializeField] private AtomicNumber atomicNumber;
        [SerializeField] private Vector3 position;
        [SerializeField] private int charge;
        public Atom( AtomId atomId,AtomicNumber atomicNumber,Position position,FormalCharge formalCharge)
        {
            this.FormalCharge = formalCharge;
            this.AtomId = atomId;
            this.Position = position;
            this.AtomicNumber = atomicNumber;
        }
        public Position Position { get; private set; }
        public AtomicNumber AtomicNumber { get; private set; }
        public FormalCharge FormalCharge { get; private set; }
        public AtomId AtomId { get; private set; }
        public void OnBeforeSerialize()
        {
            atomId = AtomId?.Value ?? 0;
            atomicNumber = AtomicNumber;
            position = Position?.Value ?? Vector3.zero;
            charge = FormalCharge?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            AtomId=new AtomId(atomId);
            AtomicNumber = atomicNumber;
            Position=new Position(position);
            FormalCharge=new FormalCharge(charge);
        }
    }

    public class Atoms
    {
        private readonly List<Atom> _atoms;

        public Atoms(List<Atom> atoms)
        {
            _atoms = atoms;
        }

        public Atom this[AtomId index] => _atoms.FirstOrDefault(x => x.AtomId.Value == index.Value);

        public List<Atom> ToList()
        {
            return _atoms;
        }
    
    }
}