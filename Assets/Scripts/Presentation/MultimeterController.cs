using System.Globalization;
using TestDigiTech.Application;
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

        [Header("TMP Outputs")]
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _uiVdc;
        [SerializeField] private TMP_Text _uiVac;
        [SerializeField] private TMP_Text _uiI;
        [SerializeField] private TMP_Text _uiR;
        
        [Space, Header("Settings")]
        [SerializeField] private float _resistance = 1000.0f;
        [SerializeField] private float _power = 400.0f;
        [SerializeField] private float _acFixed = 0.01f;
        
        private TriggerSwitch _activeKnob;
        private bool _hoverActive;
        private MultimeterService _service;

        private void Awake()
        {
            var calc = new OhmsLawCalculator(_resistance, _power, _acFixed);
            _service = new MultimeterService(calc);
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
            if (_activeKnob == null) 
                return;
            
            var mode = GetModeByAngle(_activeKnob.ZAngle);
            RenderUI(mode);
        }

        private void RenderUI(MultimeterMode mode)
        {
            var readout = _service.Calculate(mode);

            _resultText.text = readout.DisplayValue.ToString("F2", CultureInfo.InvariantCulture) + GetSuffix(mode);

            var zero = 0.0.ToString("F2", CultureInfo.InvariantCulture);
            _uiVdc.text = mode == MultimeterMode.VoltageDC 
                ? readout.VoltageDC.ToString("F2", CultureInfo.InvariantCulture) : zero;
            _uiVac.text = mode == MultimeterMode.VoltageAC 
                ? readout.VoltageAC.ToString("F2", CultureInfo.InvariantCulture) : zero;
            _uiI.text = mode == MultimeterMode.Current   
                ? readout.Current.ToString("F2",   CultureInfo.InvariantCulture) : zero;
            _uiR.text = mode == MultimeterMode.Resistance
                ? readout.Resistance.ToString("F2",CultureInfo.InvariantCulture) : zero;
        }

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

        private string GetSuffix(MultimeterMode mode) => mode switch
        {
            MultimeterMode.VoltageDC => " V",
            MultimeterMode.VoltageAC => " V~",
            MultimeterMode.Current => " A",
            MultimeterMode.Resistance => " Ω",
            _ => ""
        };
    }
}