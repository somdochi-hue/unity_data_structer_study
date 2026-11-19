using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button number, value;

    
    public void Divide(int dividen, int divisor, out int q, out int r)
    {
        q = dividen / divisor;
        r = dividen % divisor;
    }

    public void Start()
    {
        Divide(17, 5, out int q, out _);
    }
}
