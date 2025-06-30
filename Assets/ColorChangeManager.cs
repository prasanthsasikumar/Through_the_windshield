using PG;
using UnityEngine;

public class ColorChangeManager : MonoBehaviour
{
    public Material carMat;

    public void ChangeToRandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        carMat.color = randomColor;
    }
}
