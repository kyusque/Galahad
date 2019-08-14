using System;
using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    public class ListElementAttribute:PropertyAttribute
    {
        public readonly string Name;

        public ListElementAttribute(AtomName name)
        {
            this.Name = name.ToString();
        }
    }
}