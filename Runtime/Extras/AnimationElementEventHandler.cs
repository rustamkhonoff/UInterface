using System;
using UnityEngine;

namespace UInterface.Extras
{
    public class AnimationElementEventHandler : ElementEventHandler
    {
        [SerializeField] private AnimationClip _animation;
        [SerializeField] private float _animationSpeed = 1f;

        private Animation m_animation;
        private float m_animationDuration;

        public override void Initialize()
        {
            m_animationDuration = _animation.length * _animationSpeed;

            if (gameObject.TryGetComponent(out Animation anim))
                m_animation = anim;
            else if (m_animation == null)
                m_animation = gameObject.AddComponent<Animation>();

            m_animation.AddClip(_animation, _animation.name);
            m_animation.playAutomatically = false;
            m_animation.clip = _animation;
        }

        protected override void OnHandleShow(Action showAction)
        {
            StopAllCoroutines();
            m_animation.Play();
            m_animation[_animation.name].speed = 0f;
            this.InvokeDelayed(() => m_animation[_animation.name].speed = _animationSpeed, _showDelay);
            this.InvokeDelayed(showAction, m_animationDuration + _showDelay);
        }

        protected override void OnHandleHide(Action hideAction)
        {
            StopAllCoroutines();
            m_animation[_animation.name].time = m_animation[_animation.name].length;
            m_animation.Play();
            m_animation[_animation.name].speed = 0f;
            this.InvokeDelayed(() => m_animation[_animation.name].speed = -_animationSpeed, _hideDelay);
            this.InvokeDelayed(hideAction, m_animationDuration + _hideDelay);
        }

        protected override float HideCost => m_animationDuration;
        protected override float ShowCost => m_animationDuration;
    }
}