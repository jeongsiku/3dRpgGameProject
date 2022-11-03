using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinimapHelper
{
    private static float worldWidth;
    private static float worldDepth;
    private static float uiMapWidth;
    private static float uiMapHeight;
    private static float distanceZ;
    private static float distanceX;

    public static void Setting (float width, float depth, float minimapWidth, float minimapHeight,
        float distX ,float distZ)
    {
        worldWidth = width;
        worldDepth = depth;
        uiMapWidth = minimapWidth;
        uiMapHeight = minimapHeight;
        distanceX = distX;
        distanceZ = distZ;
    }

    private static Vector2 WorldPosToMapPos(Vector3 worldPos, 
        float worldWidth, float worldDepth, float uiMapWidth, float uiMapHeight)
    {
        Vector3 result = Vector3.zero;
        result.x = ((worldPos.x + distanceX) * uiMapWidth) / worldWidth;
        result.y = ((worldPos.z - distanceZ) * uiMapHeight) / worldDepth ;
        return result;
    }

    private static Vector2 MapPosToWorldPos(Vector3 mapPos, float worldWidth, float worldDepth, float uiMapWidth, float uiMapHeight)
    {
        Vector3 result = Vector3.zero;
        result.x = (mapPos.x * worldWidth) / uiMapWidth;
        result.z = (mapPos.y * worldDepth ) / uiMapHeight;
        return result;
    }

    public static void LookAt(Transform worldPlayer, Transform uiPlayer)
    {
        float angleZ = Mathf.Atan2(worldPlayer.forward.z, worldPlayer.forward.x) * Mathf.Rad2Deg;
        uiPlayer.eulerAngles = new Vector3(0, 0, angleZ - 270);
    }

    public static void MarkOnTheMap(Transform worldPlayer, Transform minimap)
    {
        minimap.localPosition = WorldPosToMapPos(worldPlayer.position) * -1;
    }

    public static Vector2 WorldPosToMapPos(Vector3 worldPos)
    {
        return WorldPosToMapPos(worldPos, worldWidth, worldDepth, uiMapWidth, uiMapHeight);
    }

    public static Vector2 MapPosToWorldPos(Vector3 mapPos)
    {
        return MapPosToWorldPos(mapPos, worldWidth, worldDepth, uiMapWidth, uiMapHeight);
    }
}
