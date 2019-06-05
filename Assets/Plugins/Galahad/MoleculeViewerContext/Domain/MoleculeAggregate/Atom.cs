using System;
using System.Collections.Generic;
using Galahad.MoleculeViewerContext.Domain.ValueObject;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace Galahad.MoleculeViewerContext.Domain.MoleculeAggregate
{
    [Serializable]
    public class Atom: ISerializationCallbackReceiver
    {
        [SerializeField] private int atomIndex;
        [SerializeField] private AtomicNumber atomicNumber;
        [SerializeField] private Vector3 position;
        
        private AtomIndex _atomIndex;
        private AtomicNumber _atomicNumber;
        private Position _position;

        public Atom(AtomIndex id, AtomicNumber atomicNumber)
        {
            _atomIndex = id;
            _atomicNumber = atomicNumber;
        }

        public Position Position => _position;


        public void OnBeforeSerialize()
        {
            atomIndex = _atomIndex?.Value ?? 0;
            atomicNumber = _atomicNumber;
            position = _position?.Value ?? Vector3.zero;
        }

        public void OnAfterDeserialize()
        {
            _atomIndex = new AtomIndex(atomIndex);
            _atomicNumber = atomicNumber;
            _position = new Position(position);
        }
    }

    public class Atoms
    {
        private List<Atom> _atoms;

        public Atoms(List<Atom> atoms)
        {
            _atoms = atoms;
        }

        public List<Atom> ToList() => _atoms;
    }
}
