using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using UnityEditor;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(fileName = "Pdb", menuName = "Installers/PdbRepository")]
public class PdbRepository : ScriptableObjectInstaller<PdbRepository>
{
    public Pdb Pdb;
    

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

    public void writer(string templeteajf,string path,string basename)
    {
        using (var writer=new StreamWriter(path))
        {
            using (var reader=new StreamReader(templeteajf))
            {
//                while (reader.ReadLine())
//                {
//                    writer.NewLine=reader
//                }
            }
        }
    }
    
    public override void InstallBindings()
    {
    }
}