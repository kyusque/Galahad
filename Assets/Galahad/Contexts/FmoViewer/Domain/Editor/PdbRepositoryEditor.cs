using System.Collections;
using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PdbRepository))]
public class PdbRepositoryEditor:Editor
{
    public PdbRepository _pdbRepository;
    [SerializeField]private string pdb;
    [SerializeField] private Object _pdb;
    
    
    private void OnEnable()
    {
        _pdbRepository= (PdbRepository) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _pdb = EditorGUILayout.ObjectField("pdbfile", _pdb,typeof(Object));
        if (_pdb != null)
        {
            pdb = AssetDatabase.GetAssetPath(_pdb);
        }
        pdb = EditorGUILayout.TextField("pdbfile", pdb);
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