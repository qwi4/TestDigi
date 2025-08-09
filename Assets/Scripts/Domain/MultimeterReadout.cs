using System.Globalization;

namespace TestDigiTech.Domain
{
    public readonly struct MultimeterReadout
    {
        public double DisplayValue { get; }
        public double VoltageDC { get; }
        public double VoltageAC { get; }
        public double Current { get; }
        public double Resistance { get; }

        public MultimeterReadout(double display, double vdc, double vac, double current, double resistance)
        {
            DisplayValue = display;
            VoltageDC = vdc;
            VoltageAC = vac;
            Current = current;
            Resistance = resistance;
        }

        public string ToFixed2() => DisplayValue.ToString("F2", CultureInfo.InvariantCulture);
    }
}