using System;
using Eflatun.SceneReference;
using UnityEngine;

namespace GameJamKit.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneLoadData", menuName = "GameJamKit/SceneLoadData")]
    public class SceneLoadData : ScriptableObject
    {
        [SerializeField] private SceneReference? scene;
        public SceneReference Scene => scene ?? throw new NullReferenceException($"{nameof(SceneLoadData)}: {nameof(scene)} is null");

        [SerializeField] private AudioClip? sceneMusic;
        public AudioClip? SceneMusic => sceneMusic;
    }
}
