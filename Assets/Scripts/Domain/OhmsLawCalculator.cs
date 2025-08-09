using System;

namespace TestDigiTech.Domain
{
    public class OhmsLawCalculator : IMultimeterCalculator
    {
        private readonly double _resistanceOhms;
        private readonly double _powerWatts;
        private readonly double _voltageAcFixed;

        private double Current => Math.Sqrt(_powerWatts / _resistanceOhms);
        private double VoltageDC => Current * _resistanceOhms;
        private double VoltageAC => _voltageAcFixed;

        public OhmsLawCalculator(double resistance, double power, double acFixed)
        {
            if (resistance <= 0) throw new ArgumentOutOfRangeException(nameof(resistance), "Resistance must be > 0");
            if (power < 0) throw new ArgumentOutOfRangeException(nameof(power), "Power must be >= 0");
            if (acFixed < 0) throw new ArgumentOutOfRangeException(nameof(acFixed), "AC voltage must be >= 0");

            _resistanceOhms = resistance;
            _powerWatts = power;
            _voltageAcFixed = acFixed;
        }

        public MultimeterReadout GetReadout(MultimeterMode mode)
        {
            var display = mode switch
            {
                MultimeterMode.Neutral => 0.0,
                MultimeterMode.VoltageDC => VoltageDC,
                MultimeterMode.VoltageAC => VoltageAC,
                MultimeterMode.Current => Current,
                MultimeterMode.Resistance => _resistanceOhms,
                _ => 0.0
            };

            return new MultimeterReadout(display, VoltageDC, VoltageAC, Current, _resistanceOhms);
        }
    }
}