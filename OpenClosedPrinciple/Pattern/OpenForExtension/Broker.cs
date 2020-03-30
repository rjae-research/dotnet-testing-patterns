using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace OpenForExtension
{
    public class Broker : BrokerBase
    {
        public Broker(IMessageProducer messageProducer) : base(messageProducer)
        {
        }

        protected override int GetPartition<T>(T data)
        {
            return Math.Abs(data.GetHashCode());
        }
    }

    public class Broker2 : Broker
    {
        public Broker2(IMessageProducer messageProducer) : base(messageProducer)
        {
        }

        protected virtual byte[] GetBytes(object data)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
        }

        protected override int GetPartition<T>(T data)
        {
            return Math.Abs(GetBytes(data).GetHashCode());
        }
    }

    public abstract class BrokerBase
    {
        protected BrokerBase(IMessageProducer messageProducer)
        {
            MessageProducer = messageProducer;
        }

        public virtual void Publish<T>(T value)
        {
            int partition = GetPartition(value);
            Publish(partition, JsonSerializer.Serialize(value));
        }

        /*
         * Pattern: Open Closed Principle
         *
         * GetPartition is open to extension and closed to modification.
         * Note that a get-partition strategy could be dependency injected, but eventually
         * the algorithm would be implemented somewhere and that thing would be open to extension.
         */
        protected abstract int GetPartition<T>(T data);

        protected virtual void Publish(int partition, string data)
        {
            MessageProducer.Produce(partition, data);
        }

        private IMessageProducer MessageProducer { get; }
    }
}