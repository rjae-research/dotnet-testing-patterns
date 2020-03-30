using System;
using System.Text.Json;

namespace ClosedForExtension
{
    public class Broker
    {
        public Broker(IMessageProducer messageProducer)
        {
            MessageProducer = messageProducer;
        }

        public void Publish<T>(T value)
        {
            int partition = GetPartition(value);
            Publish(partition, JsonSerializer.Serialize(value));
        }

        /*
         * Anti-pattern: Open Closed Principle violation
         *
         * Requires Broker to be modified to effect change in GetPartition behavior.
         * Static methods are cumbersome to mock.
         *
         * Bug: GetHashCode is NOT appropriate for object unique id.
         */
        private static int GetPartition<T>(T data)
        {
            return Math.Abs(data.GetHashCode());
        }

        private IMessageProducer MessageProducer { get; }

        private void Publish(int partition, string data)
        {
            MessageProducer.Produce(partition, data);
        }
    }
}