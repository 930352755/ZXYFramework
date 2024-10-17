using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// The adaptation of the fringe screen removes the background and moves down the effective display area.
    /// Modified to suit this UI framework.
    /// </summary>
    public class PanelFitNotchScreen : MonoBehaviour
    {
        private RectTransform panelRectTransform;
        private Vector2 originalAnchorMin;
        private Vector2 originalAnchorMax;

        private ScreenOrientation curOrientation;
        private void Start()
        {
            // Pay attention to UI construction
            panelRectTransform = transform.GetChild(1) as RectTransform;
            originalAnchorMin = panelRectTransform.anchorMin;
            originalAnchorMax = panelRectTransform.anchorMax;
            curOrientation = Screen.orientation;
            FitNotchScreen();
        }

        /// <summary>
        /// This design only supports vertical screen, and left and right horizontal screen
        /// </summary>
        private void FitNotchScreen()
        {
            if (Screen.orientation == ScreenOrientation.Portrait)
            {
                float cd = Screen.height - Screen.safeArea.height;
                if (cd > 120) cd = 120;

                float offset = cd / Screen.height;
                panelRectTransform.anchorMin = originalAnchorMin;
                panelRectTransform.anchorMax = new Vector2(originalAnchorMax.x, originalAnchorMax.y - offset);
            }
        }
        private void Update()
        {
            if (curOrientation != Screen.orientation)
            {
                FitNotchScreen();
                curOrientation = Screen.orientation;
            }
        }
    }
}
