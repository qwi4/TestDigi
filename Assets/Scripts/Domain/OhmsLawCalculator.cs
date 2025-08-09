using System;

namespace TestDigiTech.Domain
{
    public class OhmsLawCalculator : IMultimeterCalculator
    {
        private readonly double _voltageAcFixed;

        public double Resistance { get; private set; }
        public double Power { get; private set; }

        public double Current
        {
            get
            {
                if (Resistance <= 0 || Power < 0) return 0;
                return Math.Sqrt(Power / Resistance);
            }
        }

        public double VoltageDC
        {
            get
            {
                if (Resistance <= 0) return 0;
                return Current * Resistance;
            }
        }

        public double VoltageAC => _voltageAcFixed;

        public OhmsLawCalculator(double resistance, double power, double acFixed)
        {
            Resistance = resistance;
            Power = power;
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
                MultimeterMode.Resistance => Resistance,
                _ => 0.0
            };

            return new MultimeterReadout(display, VoltageDC, VoltageAC, Current, Resistance);
        }
    }
}