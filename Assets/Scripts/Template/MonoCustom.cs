using System;
using Template.Managers;
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

        private void Awake()
        {
            if (methods.HasFlag(Methods.Start))
            {
                GameManager.Instance.OnStart.AddListener(OnStart);
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
                GameManager.Instance.OnUpdate += OnUpdate;
            }

            if (methods.HasFlag(Methods.LateUpdate))
            {
                GameManager.Instance.OnLateUpdate += OnLateUpdate;
            }

            if (methods.HasFlag(Methods.FixedUpdate))
            {
                GameManager.Instance.OnFixedUpdate += OnFixedUpdate;
            }
        }

        private void OnDisable()
        {
            if (methods.HasFlag(Methods.Update))
            {
                GameManager.Instance.OnUpdate -= OnUpdate;
            }

            if (methods.HasFlag(Methods.LateUpdate))
            {
                GameManager.Instance.OnLateUpdate -= OnLateUpdate;
            }

            if (methods.HasFlag(Methods.FixedUpdate))
            {
                GameManager.Instance.OnFixedUpdate -=OnFixedUpdate;
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