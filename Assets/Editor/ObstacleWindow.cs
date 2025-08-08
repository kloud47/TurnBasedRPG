using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleWindow : EditorWindow
{
    private const int GridSize = 10;
    public GridStateData gridState;
    public bool[,] gridStates = new bool[GridSize, GridSize];
    private GridSystem gridSystem;

    [MenuItem("My Tools/UI Toolkit/ObstacleWindow")]
    public static void ShowWindow()
    {
        ObstacleWindow wnd = GetWindow<ObstacleWindow>();
        wnd.titleContent = new GUIContent("Obstacle Grid");
        wnd.Initialize();
    }

    private void Initialize()
    {
        // gridSystem = FindFirstObjectByType<GridSystem>();
        if (gridState == null) {
            gridState = ScriptableObject.CreateInstance<GridStateData>();
        }
        gridStates = gridState.gridStates;
        // for (int row = 0; row < GridSize; row++)
        // {
        //     for (int col = 0; col < GridSize; col++)
        //     {
        //         Vector2Int coord = new Vector2Int(row, col);
        //         gridStates[row, col] = false;
        //     }
        // }
    }

    public void CreateGUI()
    {
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Column;
        rootVisualElement.Add(container);
        
        // Create grid
        for (int row = 0; row < GridSize; row++)
        {
            var rowContainer = new VisualElement();
            rowContainer.style.flexDirection = FlexDirection.Row;
            container.Add(rowContainer);

            for (int col = 0; col < GridSize; col++)
            {
                CreateGridButton(rowContainer, row, col);
            }
        }
    }

    void CreateGridButton(VisualElement Btn, int row, int col)
    {
        Button button = new Button();
        button.name = $"{row},{col}";
        button.text = $"{row},{col}";
        button.style.width = 30;
        button.style.height = 30;
    
        button.clicked += () => 
        {
            gridStates[row, col] = !gridStates[row, col];
            UpdateButtonAppearance(button, gridStates[row, col]);
        };
    
        Btn.Add(button);
    }
    
    private void UpdateButtonAppearance(Button button, bool isChecked)
    {
        if (isChecked)
        {
            button.AddToClassList("checked-button");
            button.style.backgroundColor = new Color(0.8f, 0.2f, 0.2f); // Red when checked
        }
        else
        {
            button.RemoveFromClassList("checked-button");
            button.style.backgroundColor = new StyleColor(StyleKeyword.Null); // Reset to default
        }
    }
}
