using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Quad")
        {
            AstroField af = coll.gameObject.GetComponent<AstroField>();

            if(!af.populated)
            {
                af.Populate();
            } else if(af.turnedOff)
            {
                af.TurnOn();
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Quad")
        {
            AstroField af = coll.gameObject.GetComponent<AstroField>();

            if (!af.turnedOff)
            {
                af.TurnOff();
            }
        }
    }
}
