using CrawfisSofware.AssetManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CrawfisSoftware.Testing
{
    public class AddressableSpawner : MonoBehaviour
    {
        [SerializeField] private string _assetProviderName;
        [SerializeField] private string _assetName;
        [SerializeField] private float _spawnTimeDelay = 0.5f;
        [SerializeField] private float _autoDestructTimeDelay = 6f;

        private IAssetManagerAsync<GameObject> _assetManager;

        private async void Awake()
        {
            var handle = Addressables.LoadAssetAsync<IAssetManagerAsync<GameObject>>(_assetProviderName);
            await handle.Task;
            //_assetManager = handle.Result.GetComponent<IAssetManagerAsync<GameObject>>();
            _assetManager = handle.Result;
            await _assetManager.Initialize();
        }

        void Start()
        {
            StartCoroutine(SpawnWithAddedScript());
        }

        private IEnumerator SpawnWithAddedScript()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnTimeDelay);
                Vector3 position = 20 * new Vector3(UnityEngine.Random.value, 1, UnityEngine.Random.value);
                SpawnAtAsync(position);
            }
        }

        private async void SpawnAtAsync(Vector3 position)
        {
            GameObject asset = await _assetManager.GetAsync(_assetName);
            if(asset == null) return;
            asset.transform.position = position;
            var autoReleaseScript = asset.AddComponent<AutoRelease>();
            autoReleaseScript.SetAssetManager(_assetManager);
            var autoDetroy = asset.AddComponent<SelfDestruct>();
            autoDetroy.DestructAfter(_autoDestructTimeDelay);
        }
    }
}