using System;
using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class Fragment:ISerializationCallbackReceiver
    {
    [SerializeField] private List<FragmentAtom> fragmentAtoms;
    [SerializeField] private List<FragmentHetatm> fragmentHetatms;
    [SerializeField] private List<FragmentBond> fragmentBonds;
    public Fragment()
    {
    }

    public Fragment(FragmentAtoms fragmentAtoms, FragmentHetatms fragmentHetatms,FragmentBonds fragmentBonds) : this()
    {
        FragmentHetatms = fragmentHetatms;
        FragmentAtoms = fragmentAtoms;
        FragmentBonds = fragmentBonds;
    }

    public FragmentAtoms FragmentAtoms { get; private set; }
    public FragmentHetatms FragmentHetatms { get; private set; }
    public FragmentBonds FragmentBonds { get; private set; }

    public State State { get; set; }

    public int TotalCharge()
    {
        var n = 0;
        fragmentAtoms.ForEach(x => { n += x.Atoms.TotalCharge(); });
        fragmentHetatms.ForEach(x => { n += x.Hetatms.TotalCharge(); });
        return n;
    }

    public int NumFrag() => fragmentAtoms.Count + fragmentBonds.Count;
    public void OnBeforeSerialize()
    {
        fragmentAtoms = FragmentAtoms?.ToList()??new List<FragmentAtom>();
        fragmentHetatms = FragmentHetatms?.ToList() ?? new List<FragmentHetatm>();
        fragmentBonds = FragmentBonds?.ToList() ?? new List<FragmentBond>();

    }
    public void OnAfterDeserialize()
    {
        FragmentAtoms=new FragmentAtoms(fragmentAtoms);
        FragmentHetatms=new FragmentHetatms(fragmentHetatms);
        FragmentBonds=new FragmentBonds(fragmentBonds);
    }
    }
}