using UnityEngine;

namespace _So
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "SO/MyColor", order = 0)]
    public class MyColor : ScriptableObject
    {
        public Color color;
    }
}
