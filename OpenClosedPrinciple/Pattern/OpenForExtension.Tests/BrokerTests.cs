using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace OpenForExtension.Tests
{
    public class BrokerTests
    {
        [Fact]
        public void PublishMustCallProduceWithDifferentPartitionWhenDataIsNotSame()
        {
            MockMessageProducer producer = new MockMessageProducer();
            new Broker(producer).Publish("TestOne");
            new Broker(producer).Publish("TestTwo");
            Assert.NotEqual(producer.Messages[0].Item1, producer.Messages[1].Item1);
        }

        [Fact]
        public void PublishMustCallProduceWithDifferentPartitionWhenDataIsNotSameRegardlessOfHashCode()
        {
            MockMessageProducer producer = new MockMessageProducer();
            new Broker(producer).Publish(new Message("TestOne"));
            new Broker(producer).Publish(new Message("TestTwo"));
            Assert.Throws<NotEqualException>(() => Assert.NotEqual(producer.Messages[0].Item1, producer.Messages[1].Item1));
            /*
             * Note how easy it was to release improved/fixed behavior in Broker.GetPartition due to OCP design.
             * Broker2 works for all types of T since its GetPartition behavior does not depend on GetHashCode.
             */
            Assert.NotEqual(new Message("TestOne").Value, new Message("TestTwo").Value);
            new Broker2(producer).Publish(new Message("TestOne"));
            new Broker2(producer).Publish(new Message("TestTwo"));
            Assert.NotEqual(producer.Messages[2].Item1, producer.Messages[3].Item1);
        }

        [Fact]
        public void PublishMustCallProduceWithSamePartitionWhenDataIsSame()
        {
            MockMessageProducer producer = new MockMessageProducer();
            new Broker(producer).Publish("Test");
            new Broker(producer).Publish("Test");
            Assert.Equal(producer.Messages[0].Item1, producer.Messages[1].Item1);
        }

        private class Message
        {
            public Message(string value)
            {
                Value = value;
            }

            public override bool Equals(object other)
            {
                return Equals(other as Message);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public string Value { get; }

            private bool Equals(Message other)
            {
                return other != null && Value == other.Value;
            }
        }

        private class MockMessageProducer : IMessageProducer
        {
            public List<ValueTuple<int, string>> Messages = new List<ValueTuple<int, string>>();

            public void Produce(int partition, string data)
            {
                Messages.Add((partition, data));
            }
        }
    }
}