using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SceneManagement
{
    [CreateAssetMenu(fileName = "NewGameSceneSO", menuName = "Scene Data/Game Scene")]
    public class GameSceneSO : ScriptableObject
    {
        public AssetReference sceneAsset = default;
    }
}