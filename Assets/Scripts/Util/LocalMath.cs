using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalMath
{
    public static float map(float x, float xmin, float xmax, float ymin, float ymax)
    {
        return (((ymax-ymin)*(x-xmin))/(xmax-xmin)) + ymin;
    }
}