using System;
using Template.Managers;
using Template.UI.Windows;
using UnityEngine;

namespace Template
{
    /// <summary>
    /// Стандартный скрипт игрока
    /// </summary>

    public class MonoCustom : MonoBehaviour
    {

        #region Events
        [Flags]
        public enum Methods
        {
            Start = 1,
            Update = 2,
            FixedUpdate = 4,
            LateUpdate = 8  
        }

        [SerializeField] private Methods methods;

        protected virtual void Awake()
        {
            if (methods.HasFlag(Methods.Start))
            {
                EventsController.Instance.OnStart += OnStart;
            }
        }

        private void OnEnable()
        {
            InitEvents();
        }

        private void InitEvents()
        {
            if (methods.HasFlag(Methods.Update))
            {
                EventsController.Instance.OnUpdate += OnUpdate;
            }

            if (methods.HasFlag(Methods.LateUpdate))
            {
                EventsController.Instance.OnLateUpdate += OnLateUpdate;
            }

            if (methods.HasFlag(Methods.FixedUpdate))
            {
                EventsController.Instance.OnFixedUpdate += OnFixedUpdate;
            }
        }

        private void OnDisable()
        {
            if (methods.HasFlag(Methods.Update))
            {
                EventsController.Instance.OnUpdate -= OnUpdate;
            }

            if (methods.HasFlag(Methods.LateUpdate))
            {
                EventsController.Instance.OnLateUpdate -= OnLateUpdate;
            }

            if (methods.HasFlag(Methods.FixedUpdate))
            {
                EventsController.Instance.OnFixedUpdate -=OnFixedUpdate;
            }
        }

        #endregion

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnLateUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }

    }
    
}