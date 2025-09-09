using UnityEngine;
using System.Collections.Generic;

public class InnerBoard : MonoBehaviour
{
    public CellButton[] cells; // assigned in the prefab through inspector

    public void Init(int index)
    {
        for (int i = 0; i < 9; i++)
        {
            cells[i].Init(index, i);
        }
    }

    public void HighlightPlayable(bool isPlayable)
    {
        foreach (var cell in cells)
        {
            cell.SetInteractable(isPlayable);
        }
    }

    public void SetAllCells(Color color, bool interactable)
    {
        foreach (var cell in cells)
        {
            cell.SetColor(color);
            cell.SetInteractable(interactable);
        }
    }

    public void SetInactiveVisual(Color color)
    {
        foreach (var cell in cells)
        {
            if (cell.IsEmpty())
            {
                cell.SetColor(color);
                cell.SetInteractable(false);
            }
        }
    }
}