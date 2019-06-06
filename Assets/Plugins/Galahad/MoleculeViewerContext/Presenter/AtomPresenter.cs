using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;
using TMPro;
using UniRx;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Presenter
{
    public class AtomPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshPro chargeInfo;
        public Atom model;

        public void Init()
        {
            InitChargeInfo();
            this.ObserveEveryValueChanged(_ => model.FormalCharge.Value)
                .Subscribe(x => { chargeInfo.text = x == 0 ? "" : x.ToString(); })
                .AddTo(this);

            this.ObserveEveryValueChanged(_ => model.Position.Value)
                .Subscribe(x => { transform.localPosition = x; }).AddTo(this);
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