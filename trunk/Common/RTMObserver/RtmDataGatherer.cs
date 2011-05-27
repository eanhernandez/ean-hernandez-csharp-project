namespace Common.RTMObserver
{
    // this concrete observer provides the rtm system with a subject that can be 
    // created for use in main methods where there are no subject classes to attach
    // observers to.
    public class RtmDataGatherer : DataGatherer  
    {
        public RtmDataGatherer(string name) : base(name) {}
    }
}
