using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public TargetRingCreater TargetRing;

    public bool Selected = false;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //TargetRing.getSelect(Selected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
