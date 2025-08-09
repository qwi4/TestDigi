using TestDigiTech.Domain;

namespace TestDigiTech.Application
{
    public class MultimeterService
    {
        private readonly IMultimeterCalculator _calc;

        public MultimeterService(IMultimeterCalculator calc)
        {
            _calc = calc;
        }

        public MultimeterReadout Calculate(MultimeterMode mode) => _calc.GetReadout(mode);
    }
}