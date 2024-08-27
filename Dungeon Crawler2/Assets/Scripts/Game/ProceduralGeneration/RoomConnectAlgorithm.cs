using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Algorithms for creating coords of corridors between rooms
/// </summary>
public static class RoomConnectAlgorithm 
{
    /// <summary>
    /// Randomly selects rooms from the connected part and then finds the closes path to an unconnected room and then repeats
    /// </summary>
    /// <param name="actualRoomCenters">list of centers of rooms</param>
    /// <returns>hashset of positions of the tiles from the corridors</returns>
    public static HashSet<Vector2Int> ConnectRooms(List<Vector2Int> actualRoomCenters)
    {
        List<Vector2Int> roomCenters = actualRoomCenters.ToList();
        List<Vector2Int> connectedPart = new List<Vector2Int>();
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (roomCenters.Count == 0)
        {
            return floor;
        }

        int rand = UnityEngine.Random.Range(0, roomCenters.Count());

        connectedPart.Add(roomCenters[rand]);
        roomCenters.RemoveAt(rand);
        while(roomCenters.Count > 0)
        {
            rand = UnityEngine.Random.Range(0, connectedPart.Count());
            Vector2Int newconnection = FindClosestPoint(roomCenters, connectedPart[rand]);
            floor.UnionWith(CreateCorridorBetweenPoints(newconnection, connectedPart[rand]));
            connectedPart.Add(newconnection);
            roomCenters.Remove(newconnection);
        }
        return floor;
    }
    /// <summary>
    /// Just adds the path between those points by randomly chosing an axis and then following it and then switching, creating straight lines
    /// </summary>
    /// <param name="point1">One center of a room</param>
    /// <param name="point2">One center of a room</param>
    /// <returns>hashset of positions of the tiles from the corridor</returns>
    private static HashSet<Vector2Int> CreateCorridorBetweenPoints(Vector2Int point1, Vector2Int point2)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        int direction = 1;
        if (UnityEngine.Random.value < 0.5f)
        {
            if (point1.x > point2.x) direction = -1;

            for (int x = point1.x; x != point2.x; x += direction)
            {
                corridor.Add(new Vector2Int(x, point2.y - 1));
                corridor.Add(new Vector2Int(x, point2.y));
                corridor.Add(new Vector2Int(x, point2.y + 1));
            }
            direction = 1;
            if (point1.y > point2.y) direction = -1;
            for (int y = point1.y; y != point2.y; y += direction)
            {
                corridor.Add(new Vector2Int(point1.x - 1, y));
                corridor.Add(new Vector2Int(point1.x, y));
                corridor.Add(new Vector2Int(point1.x + 1, y));
            }
        }
        else
        {
            if (point1.y > point2.y) direction = -1;

            for (int y = point1.y; y != point2.y; y += direction)
            {
                corridor.Add(new Vector2Int(point1.x - 1, y));
                corridor.Add(new Vector2Int(point1.x, y));
                corridor.Add(new Vector2Int(point1.x+1, y));
            }
            corridor.UnionWith(GetBiggerCorner(new Vector2Int(point1.x, point2.y)));
            direction = 1;
            if (point1.x > point2.x) direction = -1;

            for (int x = point1.x; x != point2.x; x += direction)
            {
                corridor.Add(new Vector2Int(x, point2.y - 1));
                corridor.Add(new Vector2Int(x, point2.y));
                corridor.Add(new Vector2Int(x, point2.y+1));
            }
        }
        return corridor;
    }
    /// <summary>
    /// Fills up the area in the corner of a corridor, when it changes axis, just adds all the neighbouring areas to itself
    /// </summary>
    private static HashSet<Vector2Int> GetBiggerCorner(Vector2Int position)
    {
        HashSet<Vector2Int> corner = new HashSet<Vector2Int>();
        foreach (var dir in DirectionManager.diagonalDirections)
        {
            corner.Add(new Vector2Int(position.x + dir.x, position.y + dir.y));
        }
        foreach (var dir in DirectionManager.directions)
        {
            corner.Add(new Vector2Int(position.x + dir.x, position.y + dir.y));
        }
        return corner;
    }
    /// <summary>
    /// Finds closest room to the selected one from the unconnected
    /// </summary>
    /// <param name="points">all of the rooms centers</param>
    /// <param name="point">current point which closes we are searching for</param>
    /// <returns>coordinates of the closest room center to the point</returns>
    private static Vector2Int FindClosestPoint(List<Vector2Int> points, Vector2Int point)
    {
        Vector2Int best = points[0];
        float bestDistance= Vector2Int.Distance(point, best);
        foreach(var p in points)
        {
            float distance = Vector2Int.Distance(p, point);
            if(distance < bestDistance)
            {
                best = p;
                bestDistance = distance;
            }
        }
        return best;
    }
    
}
