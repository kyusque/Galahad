using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;
using UnityEngine;
using TMPro;

namespace Galahad.Contexts.MoleculeViewer.Presenter
{
    public class AtomPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshPro chargeInfo;
        public Atom model;

        public void Init()
        {
            InitChargeInfo();
            transform.localPosition = model.Position.Value;
            chargeInfo.text = model.FormalCharge.Value == 0 ? "" : model.FormalCharge.Value.ToString();
        }

        private void Update()
        {
            if (transform.localPosition != model.Position.Value) transform.localPosition = model.Position.Value;

            if (chargeInfo.text != model.FormalCharge.Value.ToString())
                chargeInfo.text = model.FormalCharge.Value == 0 ? "" : model.FormalCharge.Value.ToString();
        }

        private void InitChargeInfo()
        {
            var obj = new GameObject
                {name = "charge", transform = {parent = transform, localPosition = new Vector3(-0.4f, 0.6f, -0.6f)}};
            chargeInfo = obj.AddComponent<TextMeshPro>();
            chargeInfo.text = "";
            chargeInfo.autoSizeTextContainer = true;
            chargeInfo.enableWordWrapping = false;
            chargeInfo.fontSizeMin = 0.5f;
            chargeInfo.fontSize = 3f;
        }
    }
}