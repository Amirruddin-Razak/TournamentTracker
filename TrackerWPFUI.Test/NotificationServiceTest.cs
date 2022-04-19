using Moq;
using NUnit.Framework;
using System;
using TrackerWPFUI.Services;

namespace TrackerWPFUI.Test
{
    public class NotificationServiceTest
    {
        private readonly NotificationService _notifyService = new NotificationService();
        public interface IFakeSubscriber { public void CallMethod(object message); };
        private class FakeMessage
        {
            private readonly string _message;
            public FakeMessage(string message)
            {
                _message = message;
            }
        }
        private class FakeMessageTwo
        {
            private readonly string _message;
            public FakeMessageTwo(string message)
            {
                _message = message;
            }
        }


        [Test]
        public void Notify_NotifySubscriber_SingleSubscriber()
        {
            //Arrange
            FakeMessage fakeMessage = new FakeMessage("This is a fake message");
            Mock<IFakeSubscriber> fakeSubscriber1 = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessage>(fakeSubscriber1.Object, fakeSubscriber1.Object.CallMethod);

            //Act
            _notifyService.Notify(fakeMessage);

            //Assert
            fakeSubscriber1.Verify(x => x.CallMethod(fakeMessage), times: Times.Once);
        }

        [Test]
        public void Notify_DoNotNotifySubscriber_SubscriberUnsubscribe()
        {
            //Arrange
            FakeMessage fakeMessage = new FakeMessage("This is a fake message");
            Mock<IFakeSubscriber> fakeSubscriber1 = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessage>(fakeSubscriber1.Object, fakeSubscriber1.Object.CallMethod);

            //Act
            _notifyService.Unsubscribe<FakeMessage>(fakeSubscriber1.Object);
            _notifyService.Notify(fakeMessage);

            //Assert
            fakeSubscriber1.Verify(x => x.CallMethod(fakeMessage), times: Times.Never);
        }

        [Test]
        public void Notify_NotifyAllSubscriber_MultipleSubscriber()
        {
            //Arrange
            FakeMessage fakeMessage = new FakeMessage("This is a fake message");

            Mock<IFakeSubscriber> fakeSubscriber1 = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessage>(fakeSubscriber1.Object, fakeSubscriber1.Object.CallMethod);

            Mock<IFakeSubscriber> fakeSubscriber2 = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessage>(fakeSubscriber2.Object, fakeSubscriber2.Object.CallMethod);

            Mock<IFakeSubscriber> fakeSubscriber3 = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessage>(fakeSubscriber3.Object, fakeSubscriber3.Object.CallMethod);

            //Act
            _notifyService.Notify(fakeMessage);

            //Assert
            fakeSubscriber1.Verify(x => x.CallMethod(fakeMessage), times: Times.Once);
            fakeSubscriber2.Verify(x => x.CallMethod(fakeMessage), times: Times.Once);
            fakeSubscriber3.Verify(x => x.CallMethod(fakeMessage), times: Times.Once);
        }

        [Test]
        public void Notify_NotifyOnlySubscriberOfSpecificType_MultipleSubscriberAndType()
        {
            //Arrange
            FakeMessage fakeMessage = new FakeMessage("This is fake message one");
            FakeMessageTwo fakeMessageTwo = new FakeMessageTwo("This is fake message two");

            Mock<IFakeSubscriber> fakeSubscriber = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessage>(fakeSubscriber.Object, fakeSubscriber.Object.CallMethod);

            Mock<IFakeSubscriber> fakeSubscriberTwo = new Mock<IFakeSubscriber>();
            _notifyService.Subscribe<FakeMessageTwo>(fakeSubscriberTwo.Object, fakeSubscriberTwo.Object.CallMethod);

            //Act
            _notifyService.Notify(fakeMessage);

            //Assert
            fakeSubscriber.Verify(x => x.CallMethod(fakeMessage), times: Times.Once);
            fakeSubscriberTwo.Verify(x => x.CallMethod(It.IsAny<object>()), times: Times.Never);
        }

        [Test]
        public void Notify_ThrowException_MessageIsNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => _notifyService.Notify((FakeMessage)null));
        }

        [Test]
        public void Notify_DoNotThrowException_NoSubscriberForSpecificType()
        {
            //Arrange
            FakeMessage fakeMessage = new FakeMessage("This is fake message one");

            //Assert
            Assert.DoesNotThrow(() => _notifyService.Notify(fakeMessage));
        }

        [Test]
        public void Unsubscribe_DoNotThrowException_NoSubscriptionForSpecificTypeExist()
        {
            //Arrange
            FakeMessage fakeMessage = new FakeMessage("This is fake message one");
            Mock<IFakeSubscriber> fakeSubscriber = new Mock<IFakeSubscriber>();

            //Assert
            Assert.DoesNotThrow(() => _notifyService.Unsubscribe<FakeMessage>(fakeSubscriber.Object));
        }
    }
}