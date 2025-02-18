using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KMUtils
{
    [RequireComponent(typeof(GridLayoutGroup))]

    public class GridLayoutGroupSize : MonoBehaviour
    {
        public float offsetX;
        public int space;
        [SerializeField] private int num = 1;

        
        private void Awake()
        {
            RefreshSize();
        }

        private void RefreshSize()
        {
            Vector2 size = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta;
            GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
            Vector2 cellsize = grid.cellSize;
            cellsize.x = ((size.x - offsetX) - space * (num - 1)) / num;
            grid.cellSize = cellsize;
            Vector2 spacing = grid.spacing;
            spacing.x = space;
            grid.spacing = spacing;
        }
    }
}