using Dvs.EventsManager;
using NUnit.Framework;
using System;

public class EventsManagerTests
{
    [Test]
    public void Trigger_WithListener_CallsListener()
    {
        // Arrange
        int expected = 1;
        int actual = 0;
        Events.ListenTo<int>(() => actual++);

        // Act
        Events.Trigger<int>();

        // Assert
        Assert.AreEqual(expected, actual,
            message: "The listener should have been called on time when trigger was called.");
    }

    [Test]
    public void Trigger_CalledTwice_CallsListenerTwice()
    {
        // Arrange
        int expected = 2;
        int actual = 0;
        Events.ListenTo<int>(() => actual++);

        // Act
        Events.Trigger<int>();
        Events.Trigger<int>();

        // Assert
        Assert.AreEqual(expected, actual,
            message: "The listener should have been called twice, since the trigger was called twice.");
    }

    [Test]
    public void Trigger_WithListenerStopped_DoesNotCallListener()
    {
        // Arrange
        int expected = 0;
        int actual = 0;
        Action a = () => actual++;
        Events.ListenTo<int>(a);
        Events.StopListeningTo<int>(a);

        // Act
        Events.Trigger<int>();

        // Assert
        Assert.AreEqual(expected, actual,
            message: "The Listener was stopped, it should not have been called from triggering the event.");
    }

    [Test]
    public void Trigger_WithMultipleListeners_CallsAllListeners()
    {
        // Arrange
        int expected = 1;
        int actual1 = 0;
        int actual2 = 0;
        
        Events.ListenTo<int>(() => actual1++);
        Events.ListenTo<int>(() => actual2++);

        // Act
        Events.Trigger<int>();

        // Assert
        Assert.AreEqual(expected, actual1,
            message: "The first listener attached was not called. It should have been.");
        Assert.AreEqual(expected, actual2,
            message: "The second listener attached was not called. It should have been.");
    }

    [Test]
    public void Trigger_WithListenersButOneStopped_StillCallsRemainingListener()
    {
        // Arrange
        int expected = 1;
        int actual = 0;
        Events.ListenTo<int>(() => actual++);
        Action a = () => actual++;
        Events.ListenTo<int>(a);
        Events.StopListeningTo<int>(a);

        // Act
        Events.Trigger<int>();

        // Assert
        Assert.AreEqual(expected, actual,
            message: "One of two listeners were stopped, but the other one should still have been called when it was triggered.");
    }

    [Test]
    public void Trigger_WithNoListener_DoesNotError()
    {
        // Arrange
        // Act
        Events.Trigger<int>();
        // Assert

    }

    [Test]
    public void StopListeningTo_GivenTheSameThingTwice_DoesNotError()
    {
        // Arrange
        Action a = () => { };
        Events.ListenTo<int>(a);
        Events.StopListeningTo<int>(a);

        // Act
        Events.StopListeningTo<int>(a);

        // Assert
    }

    [Test]
    public void StopListeningTo_WhenListenerNeverRequested_DoesNotError()
    {
        // Arrange
        Action a = () => { };

        // Act
        Events.StopListeningTo<int>(a);

        // Assert
    }
}
