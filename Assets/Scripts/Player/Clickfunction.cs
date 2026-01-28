using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickfunction : MonoBehaviour
{
    public Camera Cam;

    Ray _ray;
    RaycastHit _hit;

    private Enemy selectedTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = Cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, 1000f))
            {
                Debug.Log("Clicked on object: " + _hit.transform.name);

                
                if (_hit.transform.CompareTag("SelectableTarget"))
                {
                    if(selectedTarget != null)
                    {
                        selectedTarget.TargetRing.getSelect(false);
                    }

                    selectedTarget = _hit.transform.GetComponent<Enemy>();
                    selectedTarget.TargetRing.getSelect(true);
                }
                
            }
        }
    }
}
