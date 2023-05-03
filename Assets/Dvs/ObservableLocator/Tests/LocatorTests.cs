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
    public void Locator_HasReferenceWithNoSetup_ReturnsFalse()
    {
        // Arrange
        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsFalse(actual, "Locator was never setup with the type requested, HasReference should have returned false");
    }

    [Test]
    public void Locator_HasReferenceWithInstanceSet_ReturnsTrue()
    {
        // Arrange
        Locator.Set<string>("string instance");

        // Act
        var actual = Locator.HasReference<string>();

        // Assert
        Assert.IsTrue(actual, "Locator was setup with the type requested, HasReference should have returned true");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator LocatorWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
