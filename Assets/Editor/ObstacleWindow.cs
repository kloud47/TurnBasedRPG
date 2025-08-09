using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleWindow : EditorWindow
{
    private const int GridSize = 10;
    private bool[,] states;
    private GridStateData gridState;
    
    [MenuItem("My Tools/UI Toolkit/ObstacleWindow")]
    public static void ShowWindow()
    {
        ObstacleWindow wnd = GetWindow<ObstacleWindow>();
        wnd.titleContent = new GUIContent("Obstacle Grid");
        wnd.minSize = new Vector2(350, 400);
    }

    private void OnEnable()
    {
        InitializeGridData();
    }

    private void InitializeGridData()
    {
        states = new bool[GridSize, GridSize];
        
        // Try to find existing GridStateData asset
        string[] guids = AssetDatabase.FindAssets("t:GridStateData");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            gridState = AssetDatabase.LoadAssetAtPath<GridStateData>(path);
        }
        else
        {
            // Create new asset if none exists
            gridState = ScriptableObject.CreateInstance<GridStateData>();
            AssetDatabase.CreateAsset(gridState, "Assets/GridStateData.asset");
            AssetDatabase.SaveAssets();
        }

        // Initialize grid states from ScriptableObject
        if (gridState != null && gridState.grid != null)
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    Vector2Int coord = new Vector2Int(x, y);
                    if (gridState.grid.ContainsKey(coord))
                    {
                        states[x, y] = !gridState.grid[coord].traversable;
                    }
                }
            }
        }
    }

    public void CreateGUI()
    {
        if (gridState == null)
        {
            rootVisualElement.Add(new Label("Failed to load grid data"));
            return;
        }

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

    private void CreateGridButton(VisualElement parent, int row, int col)
    {
        Button button = new Button();
        button.name = $"{row},{col}";
        button.text = states[row, col] ? "X" : $"{row},{col}";
        button.style.width = 30;
        button.style.height = 30;
    
        button.clicked += () => 
        {
            states[row, col] = !states[row, col];
            Vector2Int coord = new Vector2Int(row, col);
            
            // Update ScriptableObject data
            if (gridState.grid.ContainsKey(coord))
            {
                // This is the main code for the Obstacle generate Functionality:
                gridState.grid[coord].traversable = !states[row, col];
                EditorUtility.SetDirty(gridState); // Mark as changed
            }
            
            UpdateButtonAppearance(button, states[row, col], row, col);
        };
    
        parent.Add(button);
    }
    
    private void UpdateButtonAppearance(Button button, bool isObstacle, int row, int col)
    {
        button.text = isObstacle ? "X" : $"{row},{col}";
        button.style.backgroundColor = isObstacle ? 
            new Color(0.8f, 0.2f, 0.2f) : 
            new StyleColor(StyleKeyword.Null);
    }
}