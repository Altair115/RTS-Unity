using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private GameObject selectionCircle;
        
        private bool _hovered = false;
        private Transform _canvas;
        private GameObject _healthbar;
        
        private void Awake()
        {
            _canvas = GameObject.Find("Canvas").transform;
        }

        private void _SelectUtil()
        {
            Globals.SELECTED_UNITS.Add(this);
            selectionCircle.SetActive(true);
            if (_healthbar == null)
            {
                _healthbar = GameObject.Instantiate(Resources.Load("Prefabs/UI/Healthbar")) as GameObject;
                _healthbar.transform.SetParent(_canvas);
                Healthbar h = _healthbar.GetComponent<Healthbar>();
                h.Initialize(transform);
                h.SetPosition();
            }
        }


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
            Globals.SELECTED_UNITS.Remove(this);
            selectionCircle.SetActive(false);
            Destroy(_healthbar);
            _healthbar = null;
        }
    }
}