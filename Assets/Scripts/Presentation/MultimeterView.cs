using System.Globalization;
using TestDigiTech.Domain;
using TMPro;
using UnityEngine;

namespace TestDigiTech.Presentation
{
    public class MultimeterView : MonoBehaviour
    {
        [Header("TMP Outputs")]
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _uiVdc;
        [SerializeField] private TMP_Text _uiVac;
        [SerializeField] private TMP_Text _uiI;
        [SerializeField] private TMP_Text _uiR;

        public void Render(MultimeterReadout readout, MultimeterMode mode)
        {
            if (_resultText == null || _uiVdc == null || _uiVac == null || _uiI == null || _uiR == null)
                return;

            _resultText.text = readout.DisplayValue.ToString("F2", CultureInfo.InvariantCulture) + GetSuffix(mode);

            var zero = 0.0.ToString("F2", CultureInfo.InvariantCulture);
            _uiVdc.text = mode == MultimeterMode.VoltageDC
                ? readout.VoltageDC.ToString("F2", CultureInfo.InvariantCulture)
                : zero;
            _uiVac.text = mode == MultimeterMode.VoltageAC
                ? readout.VoltageAC.ToString("F2", CultureInfo.InvariantCulture)
                : zero;
            _uiI.text = mode == MultimeterMode.Current
                ? readout.Current.ToString("F2", CultureInfo.InvariantCulture)
                : zero;
            _uiR.text = mode == MultimeterMode.Resistance
                ? readout.Resistance.ToString("F2", CultureInfo.InvariantCulture)
                : zero;
        }

        private string GetSuffix(MultimeterMode mode) => mode switch
        {
            MultimeterMode.VoltageDC => " V",
            MultimeterMode.VoltageAC => " V~",
            MultimeterMode.Current => " A",
            MultimeterMode.Resistance => " Ω",
            _ => string.Empty
        };
    }
}


