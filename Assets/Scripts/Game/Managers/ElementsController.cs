using System.Collections.Generic;
using System.Linq;
using Template.UI;
using Template.UI.Overlays;
using Template.UI.Windows;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Template.Managers
{
    [System.Serializable]
    public class ElementsController
    {
        [SerializeField] private List<UIControllerElement> elements = new List<UIControllerElement>(20);
        public struct Raycaster
        {
            public GraphicRaycaster raycaster;
            public Canvas canvas;
        }
        [SerializeField] private List<Raycaster> raycasters = new List<Raycaster>(20);
        List<RaycastResult> results = new List<RaycastResult>(20);

        public void Init(UIController controller)
        {
            foreach (var el in elements)
            {
                if (el)
                {
                    el.Init(controller);
                }
            }

            var rays = controller.GetComponentsInChildren<GraphicRaycaster>();
            for (int i = 0; i < rays.Length; i++)
            {
                raycasters.Add(new Raycaster()
                {
                    raycaster = rays[i],
                    canvas = rays[i].GetComponent<Canvas>()
                });
            }
        }
        public bool IsOverElements()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            for (int i = 0; i < raycasters.Count; i++)
            {
                if (raycasters[i].canvas.enabled)
                {
                    results.Clear();
                    raycasters[i].raycaster.Raycast(eventData, results);
                    if (results.Count != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}