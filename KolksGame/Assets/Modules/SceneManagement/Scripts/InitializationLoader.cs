using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace SceneManagement
{
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _managersScene = default;

        private void Start() 
        {
            _managersScene.sceneAsset.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;   
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            Debug.Log("LoadEventChannel()");
        }
    }
}