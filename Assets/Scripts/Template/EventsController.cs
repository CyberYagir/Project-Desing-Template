using System;
using Template.Tweaks;
using Event = Template.Tweaks.Event;

namespace Template
{
    public class EventsController : Singleton<EventsController>
    {
        public Event OnStart = new Event();
        public Event OnUpdate = new Event();
        public Event OnFixedUpdate = new Event();
        public Event OnLateUpdate = new Event();

        public void Init()
        {
            SingletonSet(this);
        }

        private void Start()
        {
            OnStart.Run();
        }

        private void Update()
        {
            OnUpdate.Run();
            OnLateUpdate.Run();
        }
        private void FixedUpdate()
        {
            OnFixedUpdate.Run();
        }
    }

}
