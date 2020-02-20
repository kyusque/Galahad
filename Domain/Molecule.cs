using System;
using System.Collections.Generic;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;
using Galahad.Domain.ValueObject;
using UnityEngine;

namespace Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate
{
    [Serializable]
    public class Molecule
    {
        [SerializeField] private Offset offset = new Offset();
        [SerializeField] private List<Atom> atoms = new List<Atom>();
        [SerializeField] private List<Bond> bonds = new List<Bond>();
        [SerializeField] private string identifier;
        [SerializeField] private string title;

        public Molecule(Atoms atoms, Bonds bonds, Position offsetPosition, string title)
        {
            Atoms = atoms;
            Bonds = bonds;
            Identifier = Guid.NewGuid().ToString();
            Title = title;
        }

        public Position OffsetPosition { get; private set; }

        public string Identifier { get; private set; }
        public string Title { get; private set; }
        public Atoms Atoms { get; private set; }
        public Bonds Bonds { get; private set; }

    }
}