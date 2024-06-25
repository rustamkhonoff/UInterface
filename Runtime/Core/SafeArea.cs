using UnityEngine;

namespace UInterface.Core
{
    internal class SafeArea : MonoBehaviour
    {
        private Rect m_lastSafeArea;
        private RectTransform m_parentRectTransform;

        private void Start()
        {
            m_parentRectTransform = this.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (m_lastSafeArea != Screen.safeArea)
            {
                ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            Rect safeAreaRect = Screen.safeArea;

            float scaleRatio = m_parentRectTransform.rect.width / Screen.width;

            float left = safeAreaRect.xMin * scaleRatio;
            float right = -(Screen.width - safeAreaRect.xMax) * scaleRatio;
            float top = -(Screen.height - safeAreaRect.yMax) * scaleRatio;
            float bottom = safeAreaRect.yMin * scaleRatio;

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(left, bottom);
            rectTransform.offsetMax = new Vector2(right, top);

            m_lastSafeArea = Screen.safeArea;
        }
    }
}