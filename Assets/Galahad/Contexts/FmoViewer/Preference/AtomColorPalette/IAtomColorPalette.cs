using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Preference
{
    public interface  IAtomColorPalette
    {
        Dictionary<ElementSymbol, Material> AtomMaterials { get;}
    }
}