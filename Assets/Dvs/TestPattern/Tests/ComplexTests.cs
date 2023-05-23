using Dvs;
using Moq;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

internal class ComplexTests
{
    [TearDown]
    public void Cleanup()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Locator.Clear();
#pragma warning restore CS0618 // Intended only for test
    }

    [UnityTest, Ignore("FAILS ON PURPOSE")]
    public IEnumerator _1_ComplexSystem_OnPlay_HasVideoPlaying_FAILS()
    {
        // Arrange
        var go = new GameObject();
        ComplexSystem subject = go.AddComponent<ComplexSystem>();

        // Act
        yield return null;
        // Fails here, we are missing its video system. 
        // In order to work with this object, we must set up is other
        // objects.

        // Assert
        Assert.IsTrue(subject.Video.IsPlaying, "Failed On Purpose");
    }

    [UnityTest, Ignore("FAILS ON PURPOSE")]
    public IEnumerator _2_ComplexSystem_OnPlay_HasVideoPlaying_FAILS()
    {
        // Arrange
        var go = new GameObject();
        ComplexSystem subject = go.AddComponent<ComplexSystem>();
        subject.Video = go.AddComponent<VideoSystem>(); 

        // Act
        yield return null;

        // Assert
        // Race condition, Video will 'START' after ComplexSystem
        // So video never initializes. 
        Assert.IsTrue(subject.Video.IsPlaying, "Failed On Purpose");
    }

    [UnityTest]
    public IEnumerator _3_ComplexSystem_OnPlay_HasVideoPlaying()
    {
        // Arrange
        GameObject go = new GameObject();
        VideoSystem video = go.AddComponent<VideoSystem>();
        ComplexSystem subject = go.AddComponent<ComplexSystem>();
        subject.Video = video;

        // Act
        yield return null;

        // Assert
        // works, but senstive. InObvious sequencing requirement
        // once setup, test validation would never capture this 
        // sensitivity
        Assert.IsTrue(subject.Video.IsPlaying, "Should not fail, Integrity Check");
    }

    public class _4_VideoSystem : IVideoSystem
    {
        public bool IsPlaying { get; private set; }
        public void Play() => IsPlaying = true;
        public void Stop() => IsPlaying = false;
    }

    [UnityTest]
    [Description("First abstracted test")]
    public IEnumerator _4_ComplexSystem_OnPlay_HasVideoPlaying()
    {
        // Arrange
        var go = new GameObject();
        ComplexSystem2 subject = go.AddComponent<ComplexSystem2>();
        subject.Video = new _4_VideoSystem();

        // Act
        yield return null;

        // Assert
        Assert.IsTrue(subject.Video.IsPlaying, "ComplexSystem2.Start should have tried to play the video.");
    }

    [UnityTest]
    [Description("Field cleanup initialization test")]
    public IEnumerator _5_ComplexSystem_OnPlay_HasVideoPlaying()
    {
        // Arrange
        var go = new GameObject();
        ComplexSystem3 subject = go.AddComponent<ComplexSystem3>();
        subject.Video.Value = new _4_VideoSystem();

        // Act
        yield return null;

        // Assert
        Assert.IsTrue(subject.Video.Value.IsPlaying, "Should not fail, Test Integrity Check");
    }

    [UnityTest]
    [Description("Locator solution to remove test dependencies")]
    public IEnumerator _6_ComplexSystem_OnPlay_HasVideoPlaying()
    {
        // Arrange
        var video = new _4_VideoSystem();
        Locator.Set<IVideoSystem>(video);
        var go = new GameObject();
        var subject = go.AddComponent<ComplexSystem4>();

        // Act
        yield return null;

        // Assert
        Assert.IsTrue(video.IsPlaying, "Should not fail, Test Integrity Check");
    }

    [UnityTest]
    [Description("Mocked system")]
    public IEnumerator _7_ComplexSystem_OnStart_CallsVideoSystemPlay()
    {
        // Arrange
        Mock<IVideoSystem> videoSystemMock = new Mock<IVideoSystem>();
        int expected = 1;
        int actual = 0;
        videoSystemMock.Setup(x => x.Play())
            .Callback(() => actual++);

        Locator.Set<IVideoSystem>(videoSystemMock.Object);
        var go = new GameObject();
        var subject = go.AddComponent<ComplexSystem4>();

        // Act
        yield return null;

        // Assert
        Assert.AreEqual(expected, actual, "This should have been called one time, but it was not.");
    }
}
