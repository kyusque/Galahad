using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Galahad.Scripts
{
    public class MoleculeUpdateService : MonoBehaviour
    {
        [SerializeField] private MoleculeRepository _moleculeRepository;
        [SerializeField] private string currentJson;
        [SerializeField] private float interval;
        [SerializeField] private string url = "http://0.0.0.0:8080";
        private string tempJson;

        private void Start()
        {
            StartCoroutine(GetMoleculeJsonFromHttp(url, interval));
            tempJson = currentJson;
            _moleculeRepository.UpdateRuntimeMoleculeFromJson(currentJson);
            Debug.Log(currentJson);
        }

        private void Update()
        {
            if (tempJson != currentJson)
            {
                _moleculeRepository.UpdateRuntimeMoleculeFromJson(currentJson);
                Debug.Log(currentJson);
            }
        }


        public IEnumerator GetMoleculeJsonFromHttp(string url, float interval)
        {
            while (true)
            {
                var request = UnityWebRequest.Get(url);
                yield return request.SendWebRequest();
                if (request.responseCode == 200) currentJson = request.downloadHandler.text;

                yield return new WaitForSeconds(interval);
            }
        }
    }
}