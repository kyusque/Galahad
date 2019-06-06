using System.Collections.Generic;
using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;

namespace Galahad.MoleculeViewerContext.Domain
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