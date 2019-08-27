using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

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
                Debug.Log(Path.GetPathRoot(name));
                Debug.Log(Path.GetFullPath(name));
                Debug.Log(Path.GetFileNameWithoutExtension(name));
                Debug.Log(name);
                if (name.Length>0)
                {
                    var repository = CreateInstance<PdbRepository>();
                    AssetDatabase.CreateAsset(repository,name);
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
                _pdbRepository.Pdb.Atoms.ToList().ForEach(atom =>
                {
                
                });
            }
            if (GUILayout.Button("test"))
            {
//            _pdbRepository.save();
            }
            }
        }
    }
}