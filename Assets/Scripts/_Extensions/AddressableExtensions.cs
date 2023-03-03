using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

namespace _Extensions
{
    public static class AddressableExtensions
    {
        public static void ReleaseAttempt(this AsyncOperationHandle t)
        {
            if(t.IsValid()) Addressables.Release(t);
        }   
        
        public static void DebugOperationError<T>(this AsyncOperationHandle<T> handle)
        {
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                // string dlError = GetDownloadError(handle);
                string dlError = handle.OperationException.Message;
                Debug.LogError(dlError);
            }
        }
        
        static string GetDownloadError(AsyncOperationHandle fromHandle)
        {
            if (fromHandle.Status != AsyncOperationStatus.Failed)
                return null;

            RemoteProviderException remoteException;
            System.Exception e = fromHandle.OperationException;
            while (e != null)
            {
                remoteException = e as RemoteProviderException;
                if (remoteException != null)
                    return remoteException.WebRequestResult.Error;
                e = e.InnerException;
            }

            return null;
        }
    }
}