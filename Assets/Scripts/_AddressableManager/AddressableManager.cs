using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _AddressableManager
{
    public abstract class AddressableManager : MonoBehaviour
    {
        #region Fields
    
        [Header("Loading Progress")] [SerializeField]
        private Slider loadingProgressSlider;

        [SerializeField] protected TMP_Text loadingProgressText;
        [SerializeField] protected TMP_Text loadingPercentageText;
        [SerializeField] protected GameObject loadingPanel;

        [SerializeField] protected TMP_Text loadingErrorsText;

        #region AsyncOperations
    
        protected AsyncOperationHandle currentAsyncOperation;

        private Dictionary<string, AsyncOperationHandle> asyncOperations = new Dictionary<string, AsyncOperationHandle>();
    
        #endregion

        string REMOTE_CHECKER_KEY = "REMOTE_CHECKER";

        bool _hasStartedLoading = false;

        bool _remoteReachable = false;
    
        #endregion
    
        #region MonoBehaviour

        private void Start()
        {
            asyncOperations.Clear();

            loadingProgressText.text = "Loading Remote Check";
        
            // Check if remote is reachable with the first loading
            var firstAssetLoad = Addressables.LoadAssetAsync<AddressableManagerRemoteChecker>(REMOTE_CHECKER_KEY);
            currentAsyncOperation = firstAssetLoad;
            firstAssetLoad.Completed += delegate(AsyncOperationHandle<AddressableManagerRemoteChecker> handle)
            {
                _remoteReachable = handle.Status == AsyncOperationStatus.Succeeded;
                Addressables.InternalIdTransformFunc = FixResourceLocation;

                OnAddressableReady();
            };
        
            asyncOperations.Add(REMOTE_CHECKER_KEY, firstAssetLoad);
        
        }

        void Update()
        {
            if (_hasStartedLoading)
            {
                if (currentAsyncOperation.IsValid())
                {
                    loadingProgressSlider.value = currentAsyncOperation.PercentComplete;
                    loadingPercentageText.text = (currentAsyncOperation.PercentComplete * 100).ToString("P0");
                }
            }
        }

        private void OnDisable()
        {
            foreach (var kvp in asyncOperations)
            {
                ReleaseAttempt(kvp.Value);   
            }
        }
    
        #endregion
    
        #region Events
    
        // At this point, we know that the remote is reachable or not.
        void OnAddressableReady()
        {
            _hasStartedLoading = true;
            loadingPanel.SetActive(true);
        
            StartCoroutine(AfterInitCoroutine());
        }
    
        IEnumerator AfterInitCoroutine()
        {
            yield return LoadAddressableCoroutine();
            _hasStartedLoading = false;
            loadingPanel.SetActive(false);
        }
    
        protected void OnAllAssetsLoaded()
        {
            loadingPanel.SetActive(false);
        }
    
        #endregion
    
        #region Loading Process
    
        protected abstract IEnumerator LoadAddressableCoroutine();
    
        #endregion
    
        #region Addressable Utils
    
        protected AsyncOperationHandle<T> LoadAssetOperation<T>(string key, Action<AsyncOperationHandle<T>> callback) where T : UnityEngine.Object
        {
            loadingProgressText.text = "Loading " + key + "...";
            AsyncOperationHandle<T> loading = Addressables.LoadAssetAsync<T>(key);
            loading.Completed += callback;
            loading.Completed += delegate(AsyncOperationHandle<T> handle)
            {
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    loadingErrorsText.text += handle.OperationException.Message + "\n";
                }
            };
            asyncOperations.Add(key, loading);
            return loading;
        }

        protected AsyncOperationHandle<SceneInstance> LoadAdditiveSceneOperation(string key, Action<AsyncOperationHandle<SceneInstance>> callback)
        {
            var loading = Addressables.LoadSceneAsync(key, LoadSceneMode.Additive);
            loading.Completed += callback;
            asyncOperations.Add(key, loading);
            return loading;
        }
    
        void ReleaseAttempt(AsyncOperationHandle t)
        {
            if(t.IsValid()) Addressables.Release(t);
        }   
    
        #endregion

        #region FixResourceLocation
        // Implement a method to transform the internal ids of locations
        string FixResourceLocation(IResourceLocation location)
        {
            if (_remoteReachable)
                return location.InternalId; // The remote is reachable, we don't have to transform the internal id.

            // The remote is not reachable, we have to transform the internal id to point local data.
            if (location.InternalId.StartsWith("http", System.StringComparison.Ordinal))
            {
                string loc = location.InternalId;
                int indexOfLastSlash = loc.LastIndexOf("/", StringComparison.Ordinal);

                string afterUrl = location.InternalId.Substring(indexOfLastSlash + 1, loc.Length - 1 - indexOfLastSlash);

                return "ServerData/StandaloneWindows64/" + afterUrl;
            }

            return location.InternalId;
        }

        #endregion
    
        protected AsyncOperationHandle GetAsyncOperation(string key)
        {
            if (asyncOperations.ContainsKey(key))
            {
                return asyncOperations[key];
            }
    
            Debug.LogError($"Key {key} missing ");
            return default;
        }
    
    }
}


