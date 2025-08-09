using TestDigiTech.Domain;

namespace TestDigiTech.Presentation
{
    public class MultimeterModel
    {
        private readonly IMultimeterCalculator _calculator;
        private MultimeterMode _currentMode;

        public MultimeterModel(IMultimeterCalculator calculator)
        {
            _calculator = calculator;
            _currentMode = MultimeterMode.Neutral;
        }

        public void SetMode(MultimeterMode mode)
        {
            _currentMode = mode;
        }

        public MultimeterReadout GetReadout()
        {
            return _calculator.GetReadout(_currentMode);
        }

        public MultimeterMode GetMode() => _currentMode;
    }
}


