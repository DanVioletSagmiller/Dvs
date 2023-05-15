using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SimpleFunctionTests
{
    [Test]
    public void Value_OnInitialization_Is0()
    {
        // Arrance
        // Act
        SimpleFunction subject = new SimpleFunction();

        // Assert
        Assert.AreEqual(expected: 0, subject.Value, 
            message: "It was expected that the value would be 0 on initialization, but it was not.");
    }

    [Test]
    public void WriteValue_GivenANumber_SetsValueToThatNumber()
    {
        // Arrange
        SimpleFunction subject = new SimpleFunction();
        int expected = 8;
        
        // Act
        subject.WriteValue(expected);

        // Assert
        Assert.AreEqual(expected, subject.Value, 
            message: "The value should have matched what was passed into the function, but it did not.");

    }
}
