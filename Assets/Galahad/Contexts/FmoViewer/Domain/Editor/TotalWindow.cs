using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Object = UnityEngine.Object;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    public class TotalWindow:EditorWindow
    {
        private Object _pdb;
        private string pdb;
        private PdbRepository _pdbRepository;
        [MenuItem("Editor/totalwindow")]
        public static void Init()
        {
            var window =  (TotalWindow) EditorWindow.GetWindow(typeof(TotalWindow));
            window.titleContent=new GUIContent("TotalEditor");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("CreateRepository"))
            {
                var name = EditorUtility.SaveFilePanel("New Repository",
                    "/Assets/Galahad/FmoViewer/Domain/PdbRepository", "", "asset");
                Debug.Log(Path.GetDirectoryName(name));
                var n=name.LastIndexOf("Assets", StringComparison.Ordinal);
                Debug.Log(name.Substring(0,n));
                Debug.Log(name.Substring(n,name.Length-n));
                Debug.Log(name);
                if (name.Length>0)
                {
                    var repository = CreateInstance<PdbRepository>();
                    _pdbRepository = repository;
                    AssetDatabase.CreateAsset(repository,name.Substring(n,name.Length-n));
                    AssetDatabase.Refresh();
                }
                
            }
            if (GUILayout.Button("PDBFile Download"))
            {
                var path = EditorUtility.OpenFilePanel("Download", "", "pdb");
                if (path.Length>0)
                {
                    _pdbRepository.ReadPdb(path);
                }
            }
            _pdb = EditorGUILayout.ObjectField("pdbfile", _pdb,typeof(Object));
            if (_pdb != null)
            {
                pdb = AssetDatabase.GetAssetPath(_pdb);
            }
            pdb = EditorGUILayout.TextField("pdbfile", pdb);
            _pdbRepository = (PdbRepository) EditorGUILayout.ObjectField("PdbRepository", _pdbRepository, typeof(PdbRepository));
            if (_pdbRepository)
            {
                if (GUILayout.Button("readpdb"))
                {
                    _pdbRepository.ReadPdb(pdb);
                }
                if (GUILayout.Button("cut"))
                {
                    _pdbRepository.AutoCut();
                }
                if (GUILayout.Button("residuecut"))
                {
                    if (_pdbRepository.Fragment.State.ResidueCut)
                    {
                        return;
                    }
                    _pdbRepository.NewAutoResidueCut();
                }
                if (GUILayout.Button("HETATM CUT"))
                {
                    
                }
                if (GUILayout.Button("AtomCut"))
                {
                    
                }
            }
        }
    }
}