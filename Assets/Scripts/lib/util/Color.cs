using UnityEngine;

class ColorUtil{
    public static Color GetRandomColor(){
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}