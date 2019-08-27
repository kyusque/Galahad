using System;
using System.Collections.Generic;
using System.IO;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Galahad.Contexts.FmoViewer.Domain
{
    [CreateAssetMenu(fileName = "Pdb", menuName = "Installers/PdbRepository")]
    public class PdbRepository : ScriptableObjectInstaller<PdbRepository>
    {
        public Pdb Pdb;
        public Fragment Fragment;
    public Dictionary<int,Bonds>Bonds;

        public void ReadPdb(string pdbfilepath)
        {
            Pdb=new Pdb(new Atoms(),new Hetatms());
            using (var file = new StreamReader(pdbfilepath))
            {
                string line;
                while((line=file.ReadLine())!=null)
                {
                    Debug.Log(line);
                    if (line.Length < 6) continue;
                    if (!Enum.TryParse<RecordName>(line.Substring(0, 6), out var name)) continue;
                    switch (name)
                    {
                        case RecordName.ATOM:
                            Pdb.Atoms.AddAtoms(line);
                            break;
                        case RecordName.HETATM:
                            Pdb.Hetatms.AddHetatms(line);
                            break;
                    }
                }
            }
        }
        public void NewAutoResidueCut(Pdb pdb)
        {
            Fragment = null;
            Bonds=new Dictionary<int, Bonds>();
            Debug.Log("start");
            var fragmentAtoms=new FragmentAtoms();
            var fragmentHetatms = new FragmentHetatms();
            var fragmentBonds=new FragmentBonds();
            var endAtoms = pdb.Atoms.GetEndOxAtoms();
            Fragment=new Fragment(fragmentAtoms,fragmentHetatms,fragmentBonds);
            Fragment.State=new State();
            foreach (AtomName value in Enum.GetValues(typeof(AtomName)))
            {
                switch (value)
                {
                    case AtomName.CA:
                        var ca = pdb.Atoms.Get(value);
                        foreach (var atom in ca.ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                Fragment.FragmentAtoms.AddCa(new FragmentId(ca.IndexOf(atom)+1),atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms.AddCa(new FragmentId(ca.IndexOf(atom)+1),atom).Add(new Bond(ca.IndexOf(atom)+1,atom.ToString(),false));
                                Bonds.Add(ca.IndexOf(atom)+1,new Bonds().Add(new Bond(ca.IndexOf(atom)+1,atom.ToString(),false)));
                                Fragment.FragmentBonds.AddNewCa(new FragmentId(ca.IndexOf(atom)+1),atom);
                                
                            }
                        }
                        break;
                    case AtomName.C:
                        var c=pdb.Atoms.Get(value);
                        foreach (var atom in c.ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value)].Add(atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber( atom.ResidueSequencsNumber.Value+1)]
                                    .Add(atom)
                                    .Add(new Bond(c.IndexOf(atom)+1,atom.ToString(),true));
                                Bonds[c.IndexOf(atom) + 1].Add(new Bond(c.IndexOf(atom) + 1, atom.ToString(), true));
                                Fragment.FragmentBonds[Fragment.FragmentAtoms[atom.ResidueSequencsNumber].FragmentId]
                                    .AddGetAtom(atom);
                            };
                        }
                        break;
                    case AtomName.O:
                        foreach (var atom in pdb.Atoms.Get(value).ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value)].Add(atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber( atom.ResidueSequencsNumber.Value+1)].Add(atom);
                            };
                        }
                        break;
                    default:
                        if (pdb.Atoms.Exists(value))
                        {
                            foreach (var atom in pdb.Atoms.Get(value).ToList())
                            {
                                Fragment.FragmentAtoms[atom.ResidueSequencsNumber].Add(atom);
                            }
                        }
                        break;
                }
            }

            foreach (var hetatm in pdb.Hetatms.ToList())
            {
                if (Fragment.FragmentHetatms.Exists(hetatm.ResidueSequencsNumber)==false)
                {
                    Fragment.FragmentHetatms.Add(hetatm.ResidueSequencsNumber);
                }
                Fragment.FragmentHetatms[hetatm.ResidueSequencsNumber].Add(hetatm);
            }
        }//ちょっと綺麗にしたPDB＝＞fragment

        public void NewAutoResidueCut()
        {
            var n= Fragment.FragmentAtoms.Count();
            for (var i = 0; i < n; i++)
            {
                if (Fragment.FragmentAtoms[new FragmentId(n-i)].ResidueName=="GLY")
                {
                    continue;
                }
                Bonds.Add(Bonds.Count+1,Fragment.FragmentAtoms
                    .AddLastNew()
                    .MoveOther(new FragmentId(n - i))
                    .OneResidueCut(new FragmentId(n - i),Bonds.Count+1));
            }
            Fragment.State.Cut();
        } 
        
        public void Save(string templeteajf)
        {
//        System.Diagnostics.Process.Start(@"C:","/select,");
////        System.Diagnostics.Process.Start()
//        using (var myProcess = new Process())
//        {
//            EditorUtility.SaveFilePanel("","","","")
//        }
            var path = EditorUtility.SaveFilePanel("save ajf", "", "", "ajf");
            if (path.Length <= 0) return;
            var pat = Path.GetFileNameWithoutExtension(path);
            if (File.Exists(path))
            {
                if (EditorUtility.DisplayDialog("overwrite?", pat + ".ajf already exists.Overwrite or Not", "overwrite",
                        "choose other name") == false)
                {
                    return;
                }
                
            }
            else
            {
                File.Create(path);
            }
            WriteAjf(templeteajf,path,pat,pat);
        }

        public void WriteAjf(string readpath,string writepath,string saveName,string basisSet)
        {
            using (var reader=new StreamReader(readpath))
            {
                using (var writer=new StreamWriter(writepath))
                {
                    string line;
                    while ((line=reader.ReadLine())!=null)
                    {
                        Debug.Log(line);
                        if (line.Contains("{{{BASENAME}}}"))
                        {
                            var n=line.LastIndexOf("{{{BASENAME}}}");
                            Debug.Log(
                                line.Substring(0, n) 
                                +saveName
                                +line.Substring(n+14,line.Length - n-14));
                            writer.WriteLine(
                                line.Substring(0, n) 
                                +saveName
                                +line.Substring(n+14,line.Length - n-14));
                        }
                        else if (line.Contains("{{{TOTAL_CHARGE}}}"))
                        {
                            var n = line.IndexOf("{{{TOTAL_CHARGE}}}");
                            Debug.Log(
                                line.Substring(0, n - 1)
                                +Fragment.TotalCharge()
                                +line.Substring(n +18 ,line.Length - n-18));
                            writer.WriteLine(
                                line.Substring(0, n - 1)
                                +Fragment.TotalCharge()
                                +line.Substring(n +18 ,line.Length - n-18));
                        }
                        else if (line.Contains("{{{BASIS_SET}}}"))
                        {
                            var n = line.IndexOf("{{{BASIS_SET}}}");
                            Debug.Log(
                                line.Substring(0, n - 1)
                                +basisSet 
                                +line.Substring(n+ 14,line.Length - n-14));
                            writer.WriteLine(
                                line.Substring(0, n - 1)
                                +basisSet 
                                +line.Substring(n+ 14,line.Length - n-14));
                        }
                        else if (line.Contains("{{{NUM_FRAGS}}}"))
                        {
                            var n = line.IndexOf("{{{NUM_FRAGS}}}");
                            Debug.Log(
                                line.Substring(0, n) 
                                +Fragment.NumFrag()
                                +line.Substring(n + 14,line.Length - n-14));
                            writer.WriteLine(
                                line.Substring(0, n) 
                                + Fragment.NumFrag()
                                +line.Substring(n + 14,line.Length - n-14));
                        }
                        else if (line.Contains("{{{ABINITMP_FRAGMENT}}}"))
                        {
                            writer.WriteLine("&FRAGMENT");
                            writer.Write(WriteFragment());
                            writer.WriteLine("/");
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
        }

        private string WriteFragment()
        {
            var frg = "";
            var write = "";
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (!fragmentAtom.Atoms.Contains()) continue;
                if (frg.Length>80)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentAtom.Atoms.Count().ToString(),8}";
            }
            foreach (var fragmentHetatm in Fragment.FragmentHetatms.ToList())
            {
                if (!fragmentHetatm.Hetatms.Contains()) continue;
                if (frg.Length > 80)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentHetatm.Hetatms.Count().ToString(),8}";
            }
            write += Environment.NewLine + frg;
            frg = "";
            //原子数
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (!fragmentAtom.Atoms.Contains()) continue;
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentAtom.Atoms.TotalCharge(),8}";
            }

            foreach (var fragmentHetatm in Fragment.FragmentHetatms.ToList())
            {
                if (!fragmentHetatm.Hetatms.Contains()) continue;
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentHetatm.Hetatms.TotalCharge(),8}";
            }
            write += Environment.NewLine + frg;
            frg = "";
            //形式電荷
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                if (fragmentAtom.Atoms.Contains())
                {
                    frg += $"{fragmentAtom.Bonds.BondNum(),8}";
                }
            }
//            foreach (var fragmentHetatm in Fragment.FragmentHetatms.ToList())
//            {
//                if (frg.Length>79)
//                {
//                    write += Environment.NewLine + frg;
//                    frg = "";
//                }
//                if (fragmentHetatm.Hetatms.Contains())
//                {
//                    frg += $"{fragmentHetatm.Hetatms.TotalCharge(),8}";
//                }
//            }
            write += Environment.NewLine + frg;
            frg = "";
            //結合本数
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (!fragmentAtom.Atoms.Contains()) continue;
                foreach (var atom in fragmentAtom.Atoms.ToList())
                {
                    if (frg.Length>79)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{atom.AtomSerialNumber.Value,8}";
                }
                write += Environment.NewLine + frg;
                frg = "";
            }
            foreach (var hetems in Fragment.FragmentHetatms.ToList())
            {
                foreach (var hetem in hetems.Hetatms.ToList())
                {
                    if (frg.Length>79)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{hetem.AtomSerialNumber.Value,8}";
                }
            }
            write += Environment.NewLine + frg;
            frg = "";
            //原子の番号
            //
            foreach (var bond in Bonds)
            {
                frg += $"{bond.Value.False().Value,8}" + $"{bond.Value.True().Value,8}";
                write += Environment.NewLine + frg;
                frg = "";
            }
            //結合しているフラグメントの原子番号
            return write;
        }
        
        public override void InstallBindings()
        {
        }
    }
}