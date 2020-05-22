using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game_02.Scripts
{
    [CreateAssetMenu(menuName = "GameConnection/Game02/G02_Config")]
    public class G02_Config: ScriptableObject
    {
        [SerializeField] private ElementAssetsHolder[] _elementAssets;

        public Dictionary<Element, ElementAssetsHolder> MapElementAssets()
        {
            var dict = _elementAssets.ToDictionary(x => x.Element, x => x);
            return dict;
        }
    }
}