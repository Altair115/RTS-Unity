using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private GameObject selectionCircle;
        
        private bool _hovered = false;

        private void OnMouseEnter()
        {
            _hovered = true;
        }

        private void OnMouseExit()
        {
            _hovered = false;
        }

        private void Update()
        {
            if (_hovered && Input.GetMouseButtonDown(0))
                Select(true);
        }

        public void Select() { Select(false); }
        public void Select(bool clearSelection)
        {
            if (Globals.SELECTED_UNITS.Contains(this)) return;
            if (clearSelection)
            {
                List<UnitManager> selectedUnits = new List<UnitManager>(Globals.SELECTED_UNITS);
                foreach (UnitManager um in selectedUnits)
                    um.Deselect();
            }
            Globals.SELECTED_UNITS.Add(this);
            selectionCircle.SetActive(true);
        }

        public void Deselect()
        {
            if (!Globals.SELECTED_UNITS.Contains(this)) return;
            Globals.SELECTED_UNITS.Remove(this);
            selectionCircle.SetActive(false);
        }
    }
}