using System;
using Dvs;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LocatorTests
{
    #region setup/teardown
    [SetUp]
    public void Setup()
    {
#pragma warning disable CS0618 // Clear is intended for test cleanup only. This is the proper use case.
        Locator.Clear();
#pragma warning restore CS0618 
    }

    [TearDown]
    public void TearDown()
    {
#pragma warning disable CS0618 // Clear is intended for test cleanup only. This is the proper use case.
        Locator.Clear();
#pragma warning restore CS0618 
    }
    #endregion // setup/teardown

    [Test]
    public void HasReference_WithNoSetup_ReturnsFalse()
    {
        // Arrange
        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsFalse(actual, "Locator was never setup with the type requested, HasReference should have returned false");
    }

    [Test]
    public void HasReference_WithInstanceSet_ReturnsTrue()
    {
        // Arrange
        Locator.Set<string>("string instance");

        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsTrue(actual, "Locator was setup with the type requested, HasReference should have returned true");
    }

    [Test] 
    public void HasReference_WithConstructorSet_ReturnsTrue()
    {
        // Arrange
        Locator.SetConstructor<string>(() => "Has a value");

        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsTrue(actual, "Locator was setup with a constructor for the type requested, HasReference should have returned true");
    }

    [Test]
    public void HasReference_WithConstructorSet_DoesNotExecuteConstructor()
    {
        // Arrange
        bool actual = true;
        Locator.SetConstructor<string>(() =>
        {
            actual = false;
            return "";
        });

        // Act
        Locator.HasReference<string>();

        // Assert
        Assert.IsTrue(actual, "HasReference was called for a given type. It should not have triggered the constructor.");
    }

    [Test]
    public void HasReference_WithDefaultTypeSet_ReturnsTrue()
    {
        // Arrange
        Locator.Set<string, string>();

        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsTrue(actual, "Locator was setup with a constructor for the type requested, HasReference should have returned true");
    }

    [Test]
    public void HasReference_WhenSetNull_ReturnsFalse()
    {
        // Arrange
        Locator.Set<string>(null);

        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsFalse(actual, "HasReference returned True, when its value was set to null. Null does not count as a value.");
    }

    [Test]
    public void Observe_WhenTypeNotSetup_DoesNothing()
    {
        // Arrange
        var actual = false;
        // Act
        Locator.Observe<string>((s) =>
        {
            actual = true;
        });

        // Assert
        Assert.IsFalse(actual, "The type was never setup, so the observation event should not have been called.");
    }

    [Test]
    public void Observe_WhenSetupWithReference_ReturnsExpectedValue()
    {
        // Arrange
        var actual = nameof(Observe_WhenSetupWithReference_ReturnsExpectedValue);
        Locator.Set<string>(actual);
        var expected = "NotActual";
        // Act
        Locator.Observe<string>((s) =>
        {
            expected = s;
        });

        // Assert
        Assert.AreEqual(expected, actual, "Observe did not return the reference value that was passed into it.");
    }

    [Test]
    public void Observe_WhenSetupWithReference_ReturnsExpectedValueMultipleTimes()
    {
        // Arrange
        var actual = nameof(Observe_WhenSetupWithReference_ReturnsExpectedValue);
        Locator.Set<string>(actual);
        var expected = "NotActual";
        Locator.Observe<string>((s) => { });

        // Act
        Locator.Observe<string>((s) =>
        {
            expected = s;
        });

        // Assert
        Assert.AreEqual(expected, actual, "Observe did not return the reference value that was passed into it, the second time it was called.");
    }

    [Test]
    public void Observe_WhenSetGivenSameValue_IsNotNotified()
    {
        // Arrange
        int observeCount = 0;
        int expectedCount = 1;
        LocatorScriptable obj = ScriptableObject.CreateInstance<LocatorScriptable>();
        Locator.Observe<LocatorScriptable>((s) => observeCount++);
        Locator.Set<LocatorScriptable>(obj);

        // Act
        Locator.Set<LocatorScriptable>(obj);

        // Assert
        Assert.AreEqual(expectedCount, observeCount, 
            "Set was given the same value twice. Since it did not change on Set, it should have only notified observers once.");
    }

    [Test]
    public void Set_WithMultipleObservesSetup_CallsEachObserverOnSet()
    {
        // Arrange
        bool called1 = false;
        bool called2 = false;
        Locator.Observe<int>((int s) => called1 = true);
        Locator.Observe<int>((int s) => called2 = true);

        // Act
        Locator.Set<int>(instance: 100);

        // Assert
        Assert.IsTrue(called1, message: "The first observable did not get called when a value was passed in.");
        Assert.IsTrue(called2, message: "The second observable did not get called when a value was passed in.");
    }

    [Test]
    public void Set_WithAStoppedObserver_WillNotCallThatObserver()
    {
        // Arrange
        int expected = 0;
        int actual = 0;
        Action<int> action = (i) => actual = i;  
        Locator.Observe<int>(action);
        Locator.StopObserving<int>(action);

        // Act
        Locator.Set<int>(instance: 1001);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "An Observer was setup and then stopped. It should never have been called.");
    }

    [Test]
    public void Observe_WhenStopObserviceCalledOnADifferentObserve_WillStillBeCalledOnSet()
    {
        // Arrange
        int expected = 1;
        int actual = 0;
        Action<int> action = (i) => { };
        Locator.Observe<int>((i) => actual++);
        Locator.Observe<int>(action);
        Locator.StopObserving<int>(action);

        // Act
        Locator.Set<int>(1001);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "An Observer was stopped but another observer was still setup. Set with a new value did not trigger the remaining observer.");
    }
    
    [Test]
    public void Set_WhenNewReferenceSet_UpdatesAllTheCalls()
    {
        // Arrange
        var actual = "";
        var expected = nameof(Observe_WhenSetupWithReference_ReturnsExpectedValue);

        Locator.Set<string>("asdf");
        Locator.Observe<string>((s) =>
        {
            actual = s;
        });

        // Act
        Locator.Set<string>(expected);

        // Assert
        Assert.AreEqual(expected, actual, "Setting a value did not call observables that were previously setup.");
    }

    [Test]
    public void Set_WithScriptable_GeneratesAScriptableReferenceForExistingObservers()
    {
        // Arrange
        LocatorScriptable actual = null;
        Locator.Observe<LocatorScriptable>((s) => { actual = s; });

        // Act
        Locator.Set<LocatorScriptable, LocatorScriptable>();

        // Assert
        Assert.NotNull(actual, "Setting the type for locator to a scriptable, should have constructed the scriptable for the existing observer.");
    }

    [Test]
    public void Set_WithBehaviour_GeneratesAReferenceForExistingObserversWithAGameObject()
    {
        // Arrange
        LocatorBehaviour actual = null;
        Locator.Observe<LocatorBehaviour>((s) => { actual = s; });

        // Act
        Locator.Set<LocatorBehaviour, LocatorBehaviour>();

        // Assert
        Assert.NotNull(actual, "Setting the type for locator to a behaviour, should have constructed the behaviour for the existing observer.");
        Assert.NotNull(actual.gameObject, "Generating a Monobehaviour when only the type was set should have constructed a GameObject, but does not appear to have done so.");
        Debug.Log(actual.gameObject.name);
    }

    [Test]
    public void Set_WithNullOnFirstCall_WillNotCallObservers()
    {
        // Arrange
        int expected = 0;
        int actual = 0;
        Locator.Observe<int>((int s) => actual++);

        // Act
        Locator.Set<string>(instance: null);

        // Assert
        Assert.AreEqual(expected, actual, 
            message: "An Observable was called when the value was set to null. "
            + "Specifically, the value was never set before, so null should technically still not be "
            + "setup and should not have been seen as a change in state.");
    }

    [Test]
    public void Set_WithNullAfterSetWithValue_WillCallObservers()
    {
        // Arrange
        int expected = 0;
        int actual = 0;
        Locator.Observe<LocatorBehaviour>((LocatorBehaviour s) => actual++);
        Locator.Set<LocatorBehaviour>(instance: null);

        // Act
        Locator.Set<LocatorBehaviour>(instance: null);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "Setting null a second time should not have triggered a notification event.");
    }

    [Test]
    public void Set_WithNullAfterSetWithNull_WillNotCallObservers()
    {
        // Arrange
        int expected = 2;
        int actual = 0;
        GameObject go = new GameObject();
        LocatorBehaviour lb = go.AddComponent<LocatorBehaviour>();
        Locator.Observe<LocatorBehaviour>((LocatorBehaviour s) => actual++ );
        Locator.Set<LocatorBehaviour>(instance: lb);

        // Act
        Locator.Set<LocatorBehaviour>(instance: null);

        // Assert
        Assert.AreEqual(expected, actual,
            message: "Setting null on a reference that previously had a value should have trigged the observer. "
            + "In this test, that would show up as being called twice, as the first one was for the value being set in the first place.");
    }
}
