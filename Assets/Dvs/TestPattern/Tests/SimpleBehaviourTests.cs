using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SimpleBehaviourTests
{
    [UnityTest]
    public IEnumerator Object_OnFirstFrame_SetsFirstFrameHappenedToTrue()
    {
        // Arrance
        var go = new GameObject();
        SimpleBehavior subject = go.AddComponent<SimpleBehavior>();

        // Act
        // Already awake
        yield return null; // Now First Frame Happened. 

        // Assert
        Assert.IsTrue(subject.FirstFrameHappened, 
            "One Frame Update was allowed to trigger, the SimpleBehaaviour should have set FirstFrameHappened to true.");
    }

    [UnityTest]
    public IEnumerator Object_OnFirstFrame_SetsStartHappenedToTrue()
    {
        // Arrance
        var go = new GameObject();
        SimpleBehavior subject = go.AddComponent<SimpleBehavior>();

        // Act
        // Already awake
        yield return null; // 1 Frame In

        // Assert
        Assert.IsTrue(subject.StartHappened,
            "One Frame passed, the SimpleBehaaviour should have StartHappened set to true.");
    }


    [Test]
    public void Object_OnAddingComponent_StartAndFirstFrameAreFalse_AwakeIsTrue()
    {
        // Arrance
        var go = new GameObject();

        // Act
        SimpleBehavior subject = go.AddComponent<SimpleBehavior>();

        // Assert
        Assert.IsFalse(subject.StartHappened,
            "Object was just constructed, the SimpleBehaaviour should have StartHappened set to false.");
        Assert.IsFalse(subject.FirstFrameHappened,
            "Object was just constructed, the SimpleBehaaviour should have FirstFrameHappened set to false.");
        Assert.IsFalse(subject.FirstFrameHappened,
            "Object was just constructed, the SimpleBehaaviour should have FirstFrameHappened set to false.");
    }
}
