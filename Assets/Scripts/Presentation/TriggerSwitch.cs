using UnityEngine;

namespace TestDigiTech.Presentation
{
    public class TriggerSwitch : MonoBehaviour
    {
        [SerializeField] private Transform _knobTransform;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _highlightColor = Color.yellow;
        [SerializeField] private float _scrollSensitivity = 200f;

        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private MaterialPropertyBlock _mpb;
        private Color _baseColor;

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();

            if (_renderer != null)
            {
                _renderer.GetPropertyBlock(_mpb);
                _baseColor = _renderer.sharedMaterial != null
                    ? _renderer.sharedMaterial.HasProperty(ColorId)
                        ? _renderer.sharedMaterial.color
                        : Color.white
                    : Color.white;

                _mpb.SetColor(ColorId, _baseColor);
                _renderer.SetPropertyBlock(_mpb);
            }

            if (_knobTransform == null)
                Debug.LogError("[TriggerSwitch] Knob Transform is not assigned.");
        }

        public float ZAngle => _knobTransform != null ? _knobTransform.localEulerAngles.z : 0f;

        public void ApplyScroll(float scrollDelta)
        {
            if (_knobTransform == null) return;

            var e = _knobTransform.localEulerAngles;
            e.z = Mathf.Repeat(e.z + (-scrollDelta * _scrollSensitivity), 360f);
            _knobTransform.localRotation = Quaternion.Euler(e);
        }

        public void SetHighlight(bool on)
        {
            if (_renderer == null) 
                return;

            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(ColorId, on ? _highlightColor : _baseColor);
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}