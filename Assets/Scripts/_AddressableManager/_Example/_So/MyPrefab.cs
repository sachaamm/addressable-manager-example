using UnityEngine;

namespace _So
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "SO/MyPrefab", order = 0)]
    public class MyPrefab : ScriptableObject
    {
        public GameObject myPrefab;
    }
}