using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBoxCutter : MonoBehaviour
{
    //Train Boxes Separate After Hit Cutter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hurdle"))
        {
            if (GetComponent<MeshRenderer>().isVisible)
            {
                this.GetComponent<ConfigurableJoint>().connectedBody = null;
                GameObject player =  transform.parent.gameObject;
                CharacterMovementController.CharacterInst.isReInstantiate = true;
                for (int i= transform.GetSiblingIndex(); i < player.transform.childCount; i++)
                {
                    Destroy(player.transform.GetChild(i).gameObject);
                }
            }
            else
            {
                this.GetComponent<Collider>().isTrigger = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hurdle"))
        {
            this.GetComponent<Collider>().isTrigger = false;
        }
        
    }
}
