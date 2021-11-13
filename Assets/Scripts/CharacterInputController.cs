using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    //Serialized Data
    [SerializeField]
    private float dragSpeed;
    //Private Data
    private CharacterMovementController characterMovement;
    private Vector3 newPos;
    private Vector3 startTouchPos=Vector3.zero;
    private Vector3 currentTouchPos=Vector3.zero;
    private bool canMove=false;
    private float touchesDifference=0;
    private void Awake()
    {
        characterMovement = this.GetComponent<CharacterMovementController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkForInput();
    }
    Vector3 currentTouchePosTemp;
    private void checkForInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canMove = true;
            characterMovement.updateMovementState(canMove);
            startTouchPos = Input.mousePosition;
        }else if (Input.GetMouseButton(0))
        {
            
            currentTouchPos = Input.mousePosition;
            
            float distanceBetweenTouches = Vector3.Distance(startTouchPos, currentTouchPos);
            touchesDifference = currentTouchePosTemp.x-currentTouchPos.x;
            if (distanceBetweenTouches > 1f) {
                float rotX = Input.GetAxis("Mouse X") * Mathf.Deg2Rad * 2.5f;
                newPos =new Vector3((rotX * dragSpeed), 0, 0);
            }
            currentTouchePosTemp = Input.mousePosition;

        }else if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
            touchesDifference = 0;
            characterMovement.updateMovementState(canMove);
        }
    }
    public Vector3 getNewPos()
    {
        return newPos;
    }
    public bool canPlayerMove()
    {
        return canMove;
    }
    public float getTouchesDiference()
    {
        return touchesDifference;
    }
}
