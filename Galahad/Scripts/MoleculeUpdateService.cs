using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Galahad.Scripts
{
    public class MoleculeUpdateService : MonoBehaviour
    {
        private MoleculeRepository _moleculeRepository;
        [SerializeField] private string currentJson;
        [SerializeField] private float interval;
        [SerializeField] private string url = "http://0.0.0.0:8080";

        private void Start()
        {
            StartCoroutine(GetMoleculeJsonFromHttp(url, interval));
            this.ObserveEveryValueChanged(_ => currentJson)
                .Subscribe(x =>
                {
                    _moleculeRepository.UpdateRuntimeMoleculeFromJson(currentJson);
                    Debug.Log(currentJson);
                }).AddTo(this);
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