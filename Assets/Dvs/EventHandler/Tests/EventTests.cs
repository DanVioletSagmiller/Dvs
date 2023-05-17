using Dvs;
using NUnit.Framework;
using System;

public class EventsTests
{
    [Test]
    public void Trigger_WithListener_CallsListener()
    {
        // Arrange
        int expected = 1;
        int actual = 0;
        Events.ListenTo<int>((int i) => actual++);

        // Act
        Events.Trigger<int>(0);

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
        Events.ListenTo<int>((int value) => actual++);

        // Act
        Events.Trigger<int>(value: 0);
        Events.Trigger<int>(value: 0);

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
        Action<int> a = (int i) => actual++;
        Events.ListenTo<int>(a);
        Events.StopListeningTo<int>(a);

        // Act
        Events.Trigger<int>(value: 0);

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
        
        Events.ListenTo<int>((int i) => actual1++);
        Events.ListenTo<int>((int i) => actual2++);

        // Act
        Events.Trigger<int>(value: 0);

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
        Events.ListenTo<int>((int i) => actual++);
        Action<int> a = (int i) => actual++;
        Events.ListenTo<int>(a);
        Events.StopListeningTo<int>(a);

        // Act
        Events.Trigger<int>(value: 0);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "One of two listeners were stopped, but the other one should still have been called when it was triggered.");
    }

    [Test]
    public void Trigger_WithNoListener_DoesNotError()
    {
        // Arrange
        // Act
        Events.Trigger<int>(value: 0);
        // Assert

    }

    [Test]
    public void StopListeningTo_GivenTheSameThingTwice_DoesNotError()
    {
        // Arrange
        Action<int> a = (int i) => { };
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
        Action<int> a = (int i) => { };

        // Act
        Events.StopListeningTo<int>(a);

        // Assert
    }

    [Test]
    public void Trigger_WithValue_PassesValue()
    {
        // Arrange
        int expected = 1;
        int actual = 0;
        Events.ListenTo<int>((int i) => actual = i);

        // Act
        Events.Trigger<int>(expected);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "The listener should have passed the same value that was passed in.");
    }

    [Test]
    public void Trigger_WithNullValue_PassesValue()
    {
        // Arrange
        string expected = null;
        string actual = "";
        Events.ListenTo<string>((string i) => actual = i);

        // Act
        Events.Trigger<string>(expected);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "The listener should have recieved a null value when trigger was called with null.");
    }
}
