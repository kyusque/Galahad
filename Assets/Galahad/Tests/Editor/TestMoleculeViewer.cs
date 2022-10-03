using System.Collections.Generic;
using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;
using NUnit.Framework;
using UnityEngine;

namespace Galahad.Tests.Editor
{
    public class TestMoleculeViewer
    {
        [Test]
        public void TestMoleculeViewerSimplePasses()
        {
            Debug.Log("Test Start");
            var atoms = new List<Atom>
            {
                new(0, 1, new Vector3(0, 0, 1), 0),
                new(1, 1, new Vector3(0, 0, 0), 0)
            };
            var bonds = new List<Bond>
            {
                new(0, 1, 1)
            };
            var molecule = new Molecule(atoms, bonds, Vector3.zero, "test");
            Assert.That(molecule.Title == "test");
        }
    }
}