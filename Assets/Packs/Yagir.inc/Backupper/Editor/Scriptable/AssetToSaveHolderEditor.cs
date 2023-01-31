using System;
using UnityEditor;
using UnityEngine;

namespace YagirDev
{
    [CustomEditor(typeof(AssetsToSaveHolder))]
    public class AssetToSaveHolderEditor : Editor
    {
        private void OnDisable()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
