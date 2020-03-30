namespace ClosedForExtension
{
    public interface IMessageProducer
    {
        void Produce(int partition, string data);
    }
}