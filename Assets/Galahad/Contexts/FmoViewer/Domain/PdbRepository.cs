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

    public void save(Pdb pdb,string templeteajf)
    {
//        System.Diagnostics.Process.Start(@"C:","/select,");
////        System.Diagnostics.Process.Start()
//        using (var myProcess = new Process())
//        {
//            EditorUtility.SaveFilePanel("","","","")
//        }

        var path = EditorUtility.SaveFilePanel("save ajf", "", "", "ajf");
        if (path.Length!=0)
        {
            
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