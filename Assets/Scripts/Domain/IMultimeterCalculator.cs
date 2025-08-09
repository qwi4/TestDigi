namespace TestDigiTech.Domain
{
    public interface IMultimeterCalculator
    {
        double Resistance { get; }
        double Power { get; }
        double Current { get; }
        double VoltageDC { get; }
        double VoltageAC { get; }

        MultimeterReadout GetReadout(MultimeterMode mode);
    }
}