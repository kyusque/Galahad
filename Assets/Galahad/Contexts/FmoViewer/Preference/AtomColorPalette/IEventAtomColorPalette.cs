using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Event;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Preference
{
    public interface IEventAtomColorPalette
    {
        Dictionary<Events, Material> EventMaterials { get; }
    }
}