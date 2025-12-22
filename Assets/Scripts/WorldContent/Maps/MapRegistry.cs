using System.Linq;

[System.Serializable]
public class MapRegistry
{
    public ShadowmoonRiverSO shadowmoonRiverSO;
    public DriftwoodMarshSO driftwoodMarshSO;
    public ArcaneLakeSO arcaneLakeSO;

    // List of maps
    public MapSO[] AllMaps =>
        new MapSO[]
        {
            shadowmoonRiverSO,
            driftwoodMarshSO,
            arcaneLakeSO
        };

    public void Initialize()
    {
        shadowmoonRiverSO.Initialize();
        driftwoodMarshSO.Initialize();
        arcaneLakeSO.Initialize();
    }

    public MapSO GetByName(string mapName)
    {
        return AllMaps.FirstOrDefault(m => m.mapName == mapName);
    }
}
