using System;
using System.Collections.Generic;
using UnityEngine;

public static class CalculateDistance
{
    public static float CalculateTotalDistance(Vector3 spawn, IList<Vector3> chiefsChests, IList<Vector3> guardians, IList<Vector3> villages, Vector3 boat)
    {
        float shortestDistance = float.MaxValue;

        foreach (Vector3 chiefsChest in chiefsChests)
        {
            foreach (Vector3 village in villages)
            {
                List<Vector3> travelPoints = new()
                    {
                        spawn,
                        chiefsChest,
                        guardians[0], guardians[1], guardians[2], guardians[3], guardians[4],
                        village,
                        boat
                    };

                shortestDistance = Math.Min(shortestDistance, CalculateShortestDistance(travelPoints, 2, 6));
            }
        }

        return shortestDistance;
    }

    // First is the first index of list to permute and last is the last index of list to permute
    private static float CalculateShortestDistance(IList<Vector3> travelPoints, int first, int last)
    {
        float shortestDistance = float.MaxValue;
        CalculateShortestDistance(travelPoints, first, last, ref shortestDistance);
        return shortestDistance;
    }

    // Calculates the shortest distance by testing all possible permuations
    private static void CalculateShortestDistance(IList<Vector3> travelPoints, int first, int last, ref float shortestDistance)
    {
        if (first == last)
        {
            shortestDistance = Math.Min(shortestDistance, CalculateMultipleDistances(travelPoints));
            return;
        }

        for (int i = first; i <= last; i++)
        {
            (travelPoints[first], travelPoints[i]) = (travelPoints[i], travelPoints[first]);
            CalculateShortestDistance(travelPoints, first + 1, last, ref shortestDistance);
            (travelPoints[first], travelPoints[i]) = (travelPoints[i], travelPoints[first]);
        }
    }

    private static float CalculateMultipleDistances(IList<Vector3> points)
    {
        float totalDistance = 0;

        for (int i = 0; i < points.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(points[i], points[i + 1]);
        }

        return totalDistance;
    }
}
