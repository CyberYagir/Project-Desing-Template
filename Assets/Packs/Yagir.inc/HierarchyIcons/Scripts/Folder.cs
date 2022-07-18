using UnityEditor;
using UnityEngine;

namespace Packs.Yagir.inc.HierarchyIcons.Scripts
{
    [ExecuteAlways]
    public class Folder : MonoBehaviour
    {
    
#if UNITY_EDITOR

        [SerializeField] private bool haveZeroPos;
        [MenuItem("GameObject/Create Folder", false, -1)]
        public static void CreateFolder()   
        {
            GameObject folder = new GameObject("Folder");
            if (Selection.activeObject != null)
            {
                if (Selection.activeObject is GameObject)
                {
                    folder.transform.parent = (Selection.activeObject as GameObject)?.transform;
                }
            }
            else
            {
                var prefab = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
                if (prefab != null)
                {
                    folder.transform.parent = prefab.prefabContentsRoot.transform;
                }
            }

            if (folder.GetComponentInParent<Canvas>())
            {
                folder.AddComponent<RectTransform>();
            }

            folder.transform.gameObject.AddComponent<Folder>();
            folder.transform.localPosition = Vector3.zero;

            Selection.activeObject = folder;
        }
        private void LateUpdate()
        {
            if (!Application.isPlaying && haveZeroPos)
            {
                transform.localPosition = Vector3.zero;
            }
        }


        private void OnDrawGizmos()
        {
            var filters = GetComponentsInChildren<Renderer>();
            if (filters.Length != 0)
            {
                float count = 0;
                Vector3 center = new Vector3();
                for (int i = 0; i < filters.Length; i++)
                {
                    center += (filters[i].bounds.center);
                    count++;
                }
                Gizmos.DrawIcon(center/count, "Folder Icon");
            }
            else
            {
                Gizmos.DrawIcon(transform.position, "Folder Icon");
            }
        }
#endif
    }
}
