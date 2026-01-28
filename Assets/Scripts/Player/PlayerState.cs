using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public Slider playerSlider;

    public int health;

    // Start is called before the first frame update
    void Start()
    {
        playerSlider.maxValue = health;
    }

    // Update is called once per frame
    void Update()
    {
        playerSlider.value = health;
    }
}
