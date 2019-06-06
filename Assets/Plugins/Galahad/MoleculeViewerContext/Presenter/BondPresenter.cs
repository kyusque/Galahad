using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;
using UniRx;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Presenter
{
    public class BondPresenter : MonoBehaviour
    {
        public Bond model;
        public Atom BeginAtom { get; set; }
        public Atom EndAtom { get; set; }

        public void Init()
        {
            SubscribeForBondOrderChange();
            this.ObserveEveryValueChanged(_ => BeginAtom.Position.Value)
                .Subscribe(_ => UpdateAtomPositions()).AddTo(this);
            this.ObserveEveryValueChanged(_ => EndAtom.Position.Value)
                .Subscribe(_ => UpdateAtomPositions()).AddTo(this);
        }

        private void UpdateAtomPositions()
        {
            transform.localPosition = (BeginAtom.Position.Value + EndAtom.Position.Value) * 0.5f;
            transform.localRotation =
                Quaternion.LookRotation(EndAtom.Position.Value - BeginAtom.Position.Value);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                (EndAtom.Position.Value - BeginAtom.Position.Value).magnitude);
        }

        private void SubscribeForBondOrderChange()
        {
            this.ObserveEveryValueChanged(_ => model.BondOrder.Value)
                .Subscribe(bondOrder =>
                {
                    for (var i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);

                    switch (bondOrder)
                    {
                        case 0:
                            Destroy(gameObject);
                            break;
                        case 1:
                            GenerateCenterBinding();
                            break;
                        case 2:
                            GenerateTwoBindings();
                            break;
                        case 3:
                            GenerateCenterBinding();
                            GenerateTwoBindings();
                            break;
                    }
                })
                .AddTo(this);
        }

        private void GenerateTwoBindings()
        {
            var left = GameObject.CreatePrimitive(PrimitiveType.Cube);
            left.transform.parent = transform;
            left.transform.localPosition = new Vector3(0.15f, 0f, 0f);
            left.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            left.transform.localRotation = Quaternion.identity;

            var right = GameObject.CreatePrimitive(PrimitiveType.Cube);
            right.transform.parent = transform;
            right.transform.localPosition = new Vector3(-0.15f, 0f, 0f);
            right.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            right.transform.localRotation = Quaternion.identity;
        }

        private void GenerateCenterBinding()
        {
            var center = GameObject.CreatePrimitive(PrimitiveType.Cube);
            center.transform.parent = transform;
            center.transform.localPosition = Vector3.zero;
            center.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            center.transform.localRotation = Quaternion.identity;
        }
    }
}