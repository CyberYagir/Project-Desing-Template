using System;
using Template.Tweaks;

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
            OnStart.Invoke();
        }

        private void Update()
        {
            OnUpdate.Invoke();
            OnLateUpdate.Invoke();
        }
        private void FixedUpdate()
        {
            OnFixedUpdate.Invoke();
            OnLateFixedUpdate.Invoke();
        }
    }

}
