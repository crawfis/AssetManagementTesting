using CrawfisSoftware.AssetManagement;
using System.Collections;
using UnityEngine;

namespace CrawfisSoftware.Testing
{
    [RequireComponent(typeof(SpriteRenderer))]
    internal class SpriteSpawnerTest : MonoBehaviour
    {
        [SerializeField] private ScriptableAssetProviderBase<Sprite> _assetManager;
        [SerializeField] private string _assetName;
        [SerializeField] private float _spawnTimeDelay = 0.5f;
        [SerializeField] private float _autoDestructTimeDelay = 6f;

        private SpriteRenderer _spriteRenderer;

        private async void Awake()
        {
            await _assetManager.Initialize();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
            Sprite asset = await _assetManager.GetAsync(_assetName);
            if (asset == null) return;
            _spriteRenderer.sprite = asset;
        }
    }
}
