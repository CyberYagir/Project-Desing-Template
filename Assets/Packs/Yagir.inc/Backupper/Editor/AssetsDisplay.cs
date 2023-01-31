using UnityEngine;

namespace YagirDev
{
    public class AssetsDisplay
    {
        public string filter;
        public GUIContent icon;

        public AssetsDisplay(string filter, GUIContent icon)
        {
            this.filter = filter;
            this.icon = icon;
        }
    }
}