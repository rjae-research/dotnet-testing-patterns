namespace OpenForExtension
{
    public interface IMessageProducer
    {
        void Produce(int partition, string data);
    }
}