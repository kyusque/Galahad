using System.Collections.Generic;
using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;

namespace Galahad.Contexts.MoleculeViewer.Domain
{
    public interface IMoleculeRepository
    {
        List<Molecule> ToList();

        string GetMoleculeJson(string identifier);
        string GetMoleculeJson(int index);

        Molecule GetMolecule(int index);
        Molecule GetMolecule(string identifier);

        void SetMolecules(List<Molecule> molecules);
    }
}