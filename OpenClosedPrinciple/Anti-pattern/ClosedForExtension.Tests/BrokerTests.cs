using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace ClosedForExtension.Tests
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

            private bool Equals(Message other)
            {
                return other != null && Value == other.Value;
            }

            private string Value { get; }
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