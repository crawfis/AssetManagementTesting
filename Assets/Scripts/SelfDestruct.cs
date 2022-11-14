using CrawfisSoftware.AssetManagement;
using System.Collections;
using UnityEngine;

namespace CrawfisSoftware.Testing
{
    internal class SelfDestruct : MonoBehaviour
    {
        public void DestructAfter(float timeDelay)
        {
            StartCoroutine(AutoDestroy(timeDelay));
        }

        private IEnumerator AutoDestroy(float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            //Destroy(this.gameObject);
            _assetProvider.ReleaseAsync(this.gameObject);
        }
        private IAssetManagerAsync<GameObject> _assetProvider;

        public void SetAssetManager(IAssetManagerAsync<GameObject> assetProvider)
        {
            this._assetProvider = assetProvider;
        }
        private void OnDestroy()
        {
            _assetProvider.ReleaseAsync(this.gameObject);
        }
    }
}