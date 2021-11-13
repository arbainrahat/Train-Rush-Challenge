using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform camTarget;
    private Vector3 offset;
    private bool playerReachedGoalLine;
    public Transform[] camEndTargets;
    private Transform camEndLookTarget;
    private int currentEndTragetReachedIndex = 0;
    //Reference
    public static CameraFollow instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        camTarget = GameObject.FindWithTag("Player").transform;
        offset = camTarget.position - this.transform.position;
    }
    void Update()
    {
        if (!playerReachedGoalLine && canplay())
            followTarget();
        else if (playerReachedGoalLine)
            finishCamPos();
    }
    private void followTarget()
    {
        Vector3 newCamPos = (camTarget.position - offset);
        this.transform.position = Vector3.MoveTowards(this.transform.position, Vector3.Lerp(this.transform.position, newCamPos, .9f), 1f);
    }

    public void setCamEndTarget(Transform[] newCamTarget, Transform endLookTraget)
    {
        playerReachedGoalLine = true;
        camEndLookTarget = endLookTraget;
        camEndTargets = newCamTarget;
    }

    private void finishCamPos()
    {
        //  Vector3 newCamPos = camTarget.position - this.transform.position;

        this.transform.LookAt(camEndLookTarget.position);
        Vector3 newCamPos = camEndTargets[currentEndTragetReachedIndex].position;
        if (Vector3.Distance(newCamPos, this.transform.position) < 1f)
        {
            switchEndTarget();
        }
        newCamPos = camEndTargets[currentEndTragetReachedIndex].position;
        // newCamPos =this.transform.position+(newCamPos - transform.position)*Time.deltaTime*10;
        this.transform.position = Vector3.MoveTowards(this.transform.position, Vector3.Slerp(this.transform.position, newCamPos, 1f), 0.5f);
    }
    
    private void switchEndTarget()
    {
        if (currentEndTragetReachedIndex < camEndTargets.Length - 1)
            currentEndTragetReachedIndex++;

    }
    public void setTarget(Transform nTarget)
    {
        camTarget = nTarget;
        offset = camTarget.position - this.transform.position;

    }
    private bool canplay()
    {
        return GameManager.instance.currentGameState == GameManager.GameState.play;
    }
}
