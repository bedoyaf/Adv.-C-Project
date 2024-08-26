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
    private static HashSet<Vector2Int> GetBiggerCorner(Vector2Int position)
    {
        HashSet<Vector2Int> corner = new HashSet<Vector2Int>();
        foreach (var dir in RandomDirectionGenerator.diagonalDirections)
        {
            corner.Add(new Vector2Int(position.x + dir.x, position.y + dir.y));
        }
        foreach (var dir in RandomDirectionGenerator.directions)
        {
            corner.Add(new Vector2Int(position.x + dir.x, position.y + dir.y));
        }
        return corner;
    }
    /// <summary>
    /// Finds closest room to the selected one from the unconnected
    /// </summary>
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
