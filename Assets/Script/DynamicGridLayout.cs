using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridLayout : MonoBehaviour
{
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform rectTransform;
    private float screenWidth = Screen.width;
    private float screenHeight = Screen.height;

    [SerializeField] private int portraitRowCount = 3; 
    [SerializeField] private int landscapeRowCount = 2;

    private void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        UpdateGridLayout();
    }

    private void Update()
    {
        UpdateGridLayout();
    }

    private void UpdateGridLayout()
    {
        if (screenHeight > screenWidth) // Portrait
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = portraitRowCount;
        }
        else // Landscape
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = landscapeRowCount;
        }
    }
}
