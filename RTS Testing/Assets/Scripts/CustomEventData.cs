using UnityEngine.Events;

public class CustomEventData
{
    private UnitData unitData;

    public CustomEventData(UnitData unitData)
    {
        this.unitData = unitData;
    }
}

[System.Serializable]
public class CustomEvent : UnityEvent<CustomEventData> {}