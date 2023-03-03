using System.Collections.Generic;
using UnityEngine;

namespace _So
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class AddressableKeysToLoad : ScriptableObject
    {
        public List<string> keys;
    }
    
    
}