using System.Collections;
using System.Collections.Generic;
using Galahad.Contexts.FmoViewer.Domain;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FmoTargetRepository))]
public class FmoRepositoryTargetEditor :Editor
{
    public FmoTargetRepository FmoTargetRepository;
    [SerializeField]private string cpf;
    [SerializeField] private Object _cpf;
    private void OnEnable()
    {
        FmoTargetRepository = (FmoTargetRepository) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _cpf = EditorGUILayout.ObjectField("cpffile", _cpf,typeof(Object));
        if (_cpf != null)
        {
            cpf = AssetDatabase.GetAssetPath(_cpf);
        }
        cpf = EditorGUILayout.TextField("cpffile", cpf);
        if (GUILayout.Button("readcpf"))
        {
            FmoTargetRepository.readcpf(cpf);
        }
    }
}
