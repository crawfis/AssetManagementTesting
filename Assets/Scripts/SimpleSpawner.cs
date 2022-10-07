using CrawfisSoftware.AssetManagement;
using CrawfisSofware.AssetManagement;
using System.Collections;
using UnityEngine;

namespace CrawfisSoftware.Testing
{
    public class SimpleSpawner : MonoBehaviour
    {
        [SerializeField] private ScriptableAssetProviderBase _assetManager;
        [SerializeField] private string _assetName;
        [SerializeField] private float _spawnTimeDelay = 0.5f;
        [SerializeField] private float _autoDestructTimeDelay = 6f;

        //private IAssetManagerAsync<GameObject> _assetManager;

        private async void Awake()
        {
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
            //var autoReleaseScript = asset.AddComponent<AutoRelease>();
            //autoReleaseScript.SetAssetManager(_assetManager);
            var autoDestroy = asset.AddComponent<SelfDestruct>();
            autoDestroy.SetAssetManager(_assetManager);
            autoDestroy.DestructAfter(_autoDestructTimeDelay);
        }
    }
}