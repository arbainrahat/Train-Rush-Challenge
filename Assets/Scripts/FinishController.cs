using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishController : MonoBehaviour
{
    
    private Transform finishTarget;
    [SerializeField]
    private Transform[] finishTargets;
    [SerializeField]
    private Transform finishLookAtTarget;
    [SerializeField]
    private Transform[] finishCamPos;
    [SerializeField]
    private Transform endCamLookTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform getFinishTarget()
    {
        return finishTarget;
    }
    public Transform getFinishLookAtTarget()
    {
        return finishLookAtTarget;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // FinishMultiplierHolderController.instance.setUpFinishShowStage();
            CameraFollow.instance.setCamEndTarget(finishCamPos, endCamLookTarget); ;
            finishTarget = finishTargets[other.transform.childCount];
            //Win Score
            int scr = 10 * other.transform.childCount;
            GameManager.instance.winScoreText.text =   scr.ToString();
        }
    }
}
