namespace WeatherApp.Strategies
{
    public class ProgramContext
    {
        private Strategy strategy;
        public ProgramContext(Strategy strategy)
        {
            this.strategy = strategy;
        }
        public void SetStrategy(Strategy strategy)
        {
            this.strategy = strategy;
        }
        public void ExecuteStrategy()
        {
            strategy.Execute();
        }
    }
}