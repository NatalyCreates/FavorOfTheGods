using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalNums {
    public static float LANE_BETWEEN_DISTANCE = 10f;
    public static float TOTAL_JOURNEY_TIME = 120f;
    public static List<List<float>> DISASTER_TIMES = new List<List<float>>
    {
        new List<float> { 10f, 15f },
        new List<float> { 30f, 40f },
        new List<float> { 50f, 66f },
        new List<float> { 80f, 95f },
        new List<float> { 110f, 117f },
    };
}
