namespace TestDigiTech.Domain
{
    public interface IMultimeterCalculator
    {
        MultimeterReadout GetReadout(MultimeterMode mode);
    }
}