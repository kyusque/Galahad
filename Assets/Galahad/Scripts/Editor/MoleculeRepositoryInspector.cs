using System.Collections;
using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Galahad.Scripts.Editor
{
    [CustomEditor(typeof(MoleculeRepositoryInspector))]
    public class MoleculeRepositoryInspector : UnityEditor.Editor
    {
        private static MoleculeRepository _moleculeRepository;
        private static IEnumerator _iEnumerator;
        private string _json;
        private string _url = "http://0.0.0.0:8080";

        private void OnEnable()
        {
            _moleculeRepository = target as MoleculeRepository;
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (_iEnumerator == null) return;
            while (_iEnumerator.MoveNext())
            {
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _json = EditorGUILayout.TextField(_json);
            if (GUILayout.Button("Add Json Molecule"))
                _moleculeRepository.AddMolecule(JsonUtility.FromJson<Molecule>(_json));

            _url = EditorGUILayout.TextField(_url);
            if (GUILayout.Button("Add Molecule From HTML")) _iEnumerator = GetMoleculeJsonFromHttp(_url);
        }

        public IEnumerator GetMoleculeJsonFromHttp(string uri)
        {
            var request = UnityWebRequest.Get(uri);
            yield return request.SendWebRequest();

            while (!request.isDone) yield return 0;
            Debug.Log(request.responseCode);
            Debug.Log(request.downloadHandler.text);

            _moleculeRepository.AddMolecule(JsonUtility.FromJson<Molecule>(request.downloadHandler.text));
            _iEnumerator = null;
        }
    }
}