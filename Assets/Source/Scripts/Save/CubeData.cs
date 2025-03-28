using System;

[Serializable]
public class CubeData
{
    public Vector2Data Vector2Data;
    public int ColorID;

    public CubeData(Vector2Data data, int colorID)
    {
        Vector2Data = data;
        ColorID = colorID;
    }
}