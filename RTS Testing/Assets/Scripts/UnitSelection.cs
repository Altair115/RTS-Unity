using System.Collections.Generic;
using Managers;
using Tools;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    [SerializeField] private Color colour = new Color(0.5f, 1f, 0.4f, 0.2f);
    
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDraggingMouseBox = true;
            _dragStartPosition = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonUp(0))
                    _isDraggingMouseBox = false;

        if (_isDraggingMouseBox && _dragStartPosition != Input.mousePosition)
            _SelectUnitsInDraggingBox();
    }
    
    private void _SelectUnitsInDraggingBox()
    {
        Bounds selectionBounds = Utils.GetViewportBounds(Camera.main, _dragStartPosition, Input.mousePosition);
        GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
        bool inBounds;
        foreach (GameObject unit in selectableUnits)
        {
            inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position));
            if (inBounds)
                unit.GetComponent<UnitManager>().Select();
            else
                unit.GetComponent<UnitManager>().Deselect();
        }
    }

    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, colour);
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }

}