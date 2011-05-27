namespace Common.RTMObserver
{
    public interface IObserver  // observer interface
    {
        void Update(DataGatherer t);
        string GetName();
    }
}
