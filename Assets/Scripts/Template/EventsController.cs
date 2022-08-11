using System;
using UnityEngine;

namespace Template
{
    public class EventsController : Singleton<EventsController>
    {

        public Action OnStart { get; set; } = delegate { };
        public Action OnUpdate { get; set; } = delegate { };
        public Action OnFixedUpdate { get; set; } = delegate { };
        public Action OnLateFixedUpdate { get; set; } = delegate { };
        public Action OnLateUpdate { get; set; } = delegate { };

        public void Init()
        {
            SingletonSet(this);
        }

        private void Start()
        {
            InvokeTry(OnStart);
        }

        private void Update()
        {
            InvokeTry(OnUpdate);
            InvokeTry(OnLateUpdate);
        }

        private void FixedUpdate()
        {
            
            InvokeTry(OnFixedUpdate);
            InvokeTry(OnLateFixedUpdate);
        }

        public void InvokeTry(Action action)
        {
            foreach (var actionEl in action.GetInvocationList())
            {
                try
                {
                    actionEl.DynamicInvoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

}
