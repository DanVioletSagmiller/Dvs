using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SimpleBehaviourTests
{
    [Test]
    public void Construct_OnAddingComponent_StartAndFirstFrameAreFalse()
    {
        // Arrance
        var go = new GameObject();

        // Act
        SimpleBehavior subject = go.AddComponent<SimpleBehavior>();

        // Assert
        Assert.IsFalse(subject.StartHappened,
            "Object was just constructed, the SimpleBehaviour should have StartHappened set to false.");

        Assert.IsFalse(subject.FirstFrameHappened,
            "Object was just constructed, the SimpleBehaviour should have FirstFrameHappened set to false.");
    }

    [UnityTest]
    public IEnumerator Object_OnFirstFrame_SetsStartAndFirstFrameHappenedToTrue()
    {
        // Arrance
        var go = new GameObject();
        SimpleBehavior subject = go.AddComponent<SimpleBehavior>();

        // Act
        // Already awake
        yield return null; // Now First Frame Happened. 

        // Assert
        Assert.IsTrue(subject.StartHappened,
            "One Frame passed, the SimpleBehaviour should have StartHappened set to true.");

        Assert.IsTrue(subject.FirstFrameHappened, 
            "One Frame Update was allowed to trigger, the SimpleBehaviour should have set FirstFrameHappened to true.");
    }
}
