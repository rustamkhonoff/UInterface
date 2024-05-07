using System;
using UnityEngine;

namespace UInterface.Extras
{
    public class AnimationElementEventHandler_Individual : ElementEventHandler
    {
        [SerializeField] private AnimationClip _showAnimation, _hideAnimation;
        [SerializeField] private float _animationSpeed = 1f;

        private Animation m_animation;
        private float m_showAnimationDuration, m_hideAnimationDuration;

        public override void Initialize()
        {
            m_showAnimationDuration = _showAnimation.length * _animationSpeed;
            m_hideAnimationDuration = _hideAnimation.length * _animationSpeed;

            if (gameObject.TryGetComponent(out Animation anim))
                m_animation = anim;
            else if (m_animation == null)
                m_animation = gameObject.AddComponent<Animation>();

            m_animation.AddClip(_showAnimation, _showAnimation.name);
            m_animation.AddClip(_hideAnimation, _hideAnimation.name);
            m_animation.playAutomatically = false;
        }

        protected override void OnHandleShow(Action showAction)
        {
            StopAllCoroutines();
            m_animation.clip = _showAnimation;
            m_animation.Play();
            m_animation[_showAnimation.name].speed = 0f;
            this.InvokeDelayed(() => m_animation[_showAnimation.name].speed = _animationSpeed, _showDelay);
            this.InvokeDelayed(showAction, m_showAnimationDuration + _showDelay);
        }

        protected override void OnHandleHide(Action hideAction)
        {
            StopAllCoroutines();
            m_animation.clip = _hideAnimation;
            m_animation.Play();
            m_animation[_hideAnimation.name].speed = 0f;
            this.InvokeDelayed(() => m_animation[_hideAnimation.name].speed = _animationSpeed, _hideDelay);
            this.InvokeDelayed(hideAction, m_hideAnimationDuration + _hideDelay);
        }

        protected override float HideCost => m_hideAnimationDuration;
        protected override float ShowCost => m_showAnimationDuration;
    }
}