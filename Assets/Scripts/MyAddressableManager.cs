using System.Collections;
using _Extensions;
using _So;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class MyAddressableManager : AddressableManager
{
    protected override IEnumerator LoadAddressableCoroutine()
    {
        currentAsyncOperation = LoadAssetOperation<AudioClip>("SoundA", OnSoundLoadedCompleted);
        yield return currentAsyncOperation;
        
        currentAsyncOperation = LoadAssetOperation<MyColor>("MyColor", OnMyColorLoaded);
        yield return currentAsyncOperation;
        
        currentAsyncOperation = LoadAssetOperation<MyPrefab>("MyPrefab", OnMyPrefabLoaded);
        yield return currentAsyncOperation;
        
        currentAsyncOperation = LoadAdditiveSceneOperation("SceneB", OnSceneLoadedCompleted);
        yield return currentAsyncOperation;
    }
    
    private void OnMyColorLoaded(AsyncOperationHandle<MyColor> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            StaticReferences.MyColor = obj.Result;
            SceneReferences.Instance.cubeRenderer.material.color = obj.Result.color;
        }

        obj.DebugOperationError();
    }
    
    private void OnMyPrefabLoaded(AsyncOperationHandle<MyPrefab> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            StaticReferences.MyPrefab = obj.Result;
        }

        obj.DebugOperationError();
    }

    private void OnSoundLoadedCompleted(AsyncOperationHandle<AudioClip> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            SceneReferences.Instance.audioSource.clip = obj.Result;
            SceneReferences.Instance.audioSource.Play();
        }
    }

    private void OnSceneLoadedCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        OnAllAssetsLoaded();
    }

}