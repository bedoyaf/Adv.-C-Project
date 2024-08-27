using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;

/// <summary>
/// Algorithms that calculate the rooms
/// </summary>
public static class BinarySpacePartitioningAlgorithms
{

    /// <summary>
    /// This algorithm splits the room randomly into two parts repeatedly, until the rooms can not be split anymore then we have created a list of quadrilateral rooms
    /// it randomly chooses which axis it should cut, thats why there is a bit of code repetition to avoid any kind of axis bias
    /// </summary>
    /// <param name="space">Bounds of the whole are where the dungeon will be generated</param>
    /// <param name="minHeight">Minimal height of a Room</param>
    /// <param name="minWidth">Minimal Width of a Room</param>
    /// <returns>resturns a List of BoundsInt, each element represents a room bounds</returns>
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt space, int minHeight, int minWidth)
    {
        Queue<BoundsInt> roomqueue = new Queue<BoundsInt>();
        List<BoundsInt> finalrooms = new List<BoundsInt>();
        roomqueue.Enqueue(space);

   
        while (roomqueue.Count>0 )
        {
            var currentRoom = roomqueue.Dequeue();

            if(currentRoom.size.x >= minWidth && currentRoom.size.y >= minHeight)
            {
                if(UnityEngine.Random.value < 0.5f)
                {
                    if (currentRoom.size.x >= minWidth * 2)
                    {
                        SplitRoomVertically(roomqueue, currentRoom,  minHeight);
                    }
                    else if (currentRoom.size.y >= minHeight * 2)
                    {
                        SplitRoomHorizontaly(roomqueue, currentRoom, minWidth);
                    }
                    else
                    {
                        finalrooms.Add(currentRoom);
                    }
                }
                else
                {
                    if (currentRoom.size.y >= minHeight * 2)
                    {
                        SplitRoomHorizontaly(roomqueue, currentRoom, minWidth);
                    }
                    else if (currentRoom.size.x >= minWidth * 2)
                    {
                        SplitRoomVertically(roomqueue, currentRoom, minHeight);
                    }
                    else
                    {
                        finalrooms.Add(currentRoom);
                    }
                }
            }
        }
        return finalrooms;
    }
    /// <summary>
    /// Splits the one room into two rooms on an axis
    /// </summary>
    /// <param name="roomqueue">Queue from the binary partitioning algorithm</param>
    /// <param name="room">the room that is being split</param>
    /// <param name="minWidth">the minimal Width of a room</param>
    /// <returns>resturns a nothing but the reference of the queue will have two new rooms</returns>
    private static void SplitRoomHorizontaly(Queue<BoundsInt> roomqueue, BoundsInt room, int minWidth)
    {
        var splitY = UnityEngine.Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, splitY, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y +splitY, room.min.z)
            , new Vector3Int(room.size.x, room.size.y-splitY, room.size.z));
        roomqueue.Enqueue(room1);
        roomqueue.Enqueue(room2);
    }
    /// <param name="roomqueue">Queue from the binary partitioning algorithm</param>
    /// <param name="room">the room that is being split</param>
    /// <param name="minWidth">the minimal Height of a room</param>
    /// <returns>resturns a nothing but the reference of the queue will have two new rooms</returns>
    private static void SplitRoomVertically(Queue<BoundsInt> roomqueue, BoundsInt room, int minHeight)
    {
        var splitX = UnityEngine.Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(splitX, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + splitX, room.min.y, room.min.z)
            , new Vector3Int(room.size.x - splitX, room.size.y, room.size.z));
        roomqueue.Enqueue(room1);
        roomqueue.Enqueue(room2);
    }
}
