using System;
using System.Collections;
using System.Collections.Generic;
using Dvs.ObservableLocator;
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
        Assert.IsTrue(actual, "Locator was setup with a constructor for the type requested, HasReference should have returned true");
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

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public void Observe_WithMultipleObservesSetup_CallsEachObserverOnSet()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public void Observe_OnceStopObservingCalled_CancelsThatObserver()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public void Observe_WhenStopObserviceCalledOnADifferentObserve_WillStillBeCalledOnSet()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
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

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public void Set_WithNullAfterSetWithValue_WillCallObservers()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }

    [Test]
    public void Set_WithNullAfterSetWithNull_WillNotCallObservers()
    {
        // Arrange

        // Act

        // Assert
        throw new NotImplementedException();
    }
}
