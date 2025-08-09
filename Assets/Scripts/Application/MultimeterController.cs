using TestDigiTech.Domain;
using UnityEngine;
using TMPro;

namespace TestDigiTech.Presentation
{
    public class MultimeterController : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _interactableMask;

        [Header("View")] 
        [SerializeField] private MultimeterView _view;
        
        [Space, Header("Settings")]
        [SerializeField] private float _resistance = 1000.0f;
        [SerializeField] private float _power = 400.0f;
        [SerializeField] private float _acFixed = 0.01f;
        
        private TriggerSwitch _activeKnob;
        private bool _hoverActive;
        private MultimeterModel _model;

        private void Awake()
        {
            var calc = new OhmsLawCalculator(_resistance, _power, _acFixed);
            _model = new MultimeterModel(calc);
        }

        private void Update()
        {
            UpdateHover();

            if (_hoverActive && _activeKnob != null)
            {
                var scroll = Input.GetAxis("Mouse ScrollWheel");

                if (Mathf.Abs(scroll) > 0.001f)
                {
                    _activeKnob.ApplyScroll(scroll);
                    UpdateUI();
                }
            }
        }

        private void UpdateHover()
        {
            if (_camera == null) return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            TriggerSwitch knob = null;

            if (Physics.Raycast(ray, out var hit, 100f, _interactableMask))
                hit.collider.GetComponentInParent<TriggerSwitch>()?.TryGetComponent(out knob);

            var newHover = knob != null;

            if (newHover != _hoverActive)
            {
                if (_activeKnob != null) 
                    _activeKnob.SetHighlight(false);
                
                if (knob != null) 
                    knob.SetHighlight(true);
                
                _hoverActive = newHover;
            }

            if (_activeKnob != knob)
            {
                if (_activeKnob != null && !newHover) 
                    _activeKnob.SetHighlight(false);
                
                _activeKnob = knob;
            }
        }

        private void UpdateUI()
        {
            if (_activeKnob == null || _view == null)
                return;

            var mode = GetModeByAngle(_activeKnob.ZAngle);
            _model.SetMode(mode);
            var readout = _model.GetReadout();
            _view.Render(readout, mode);
        }

        // View rendering is delegated to MultimeterView

        private MultimeterMode GetModeByAngle(float zDegrees)
        {
            var z = Normalize(zDegrees);

            if (InRange(z, -60f, -25f))
                return MultimeterMode.VoltageDC;

            if (InRange(z, -150f, -105f))
                return MultimeterMode.VoltageAC;

            if (InRange(z, 110f, 155f))
                return MultimeterMode.Current;

            if (InRange(z, 40f, 80f))
                return MultimeterMode.Resistance;

            return MultimeterMode.Neutral;
        }

        private bool InRange(float val, float min, float max) => val >= min && val <= max;

        private float Normalize(float deg)
        {
            var d = deg % 360f;
            if (d > 180f) d -= 360f;
            if (d < -180f) d += 360f;
            return d;
        }

        // Suffix mapping is handled by MultimeterView
    }
}