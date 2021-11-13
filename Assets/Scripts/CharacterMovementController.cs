using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed=1;

    private float gravityModifier=-9.81f;

    //Private Data
    private CharacterInputController characterInputController;
    private Rigidbody rb;
  
    private bool isRunning;
    private bool reachedGoalLine;
    private Transform endTraget;
   // private Transform lookAtEndTraget;

    private int trainBoxOffSet = 1;
    private Rigidbody curntConnectedBody;
    private int curntTrainBoxIndex = 0;
    [SerializeField]
    private GameObject[] trainBoxList;
    private int reLength = 0;
    int score = 0;

    //Public Data
    public GameObject trainBox;
    public static CharacterMovementController CharacterInst;
    public bool isReInstantiate = false;


    private void Awake()
    {
        CharacterInst = this;
        characterInputController = this.GetComponent<CharacterInputController>();
        CameraFollow.instance.setTarget(this.transform);
    }
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        curntConnectedBody = rb;
        TrainInstantiate();
    }

    void Update()
    {
        if (canPlay())
        {
            move();
        }  
        //Train Boxes Regenerate after separate Boxes
        if (isReInstantiate)
        {
            TrainReInstantiate();
        }
    }
    private void move()
    {
        if (!reachedGoalLine && isRunning)
        {
          //  print("Is Moving");
            Vector3 newPos = this.rb.position + characterInputController.getNewPos();
            this.rb.MovePosition(Vector3.MoveTowards(this.rb.position, Vector3.Slerp(this.rb.position, newPos, .7f), 8f));
        }else if (reachedGoalLine)
        {
            Vector3 directionToEndTraget = endTraget.position - this.transform.position;
            directionToEndTraget *= Time.deltaTime * 1f;
            this.transform.position += directionToEndTraget;
            float distanceToEnd = Vector3.Distance(this.transform.position, endTraget.position);
           
            this.rb.velocity = Vector3.zero;

        }

    }
    void FixedUpdate()
    {
        if (isRunning && canPlay() && !reachedGoalLine)
            rb.velocity = (Vector3.forward * speed + Vector3.up * gravityModifier);
        else
        {
          //  print("Stoping Player");
            rb.velocity = Vector3.zero;

        }

    }
    
    public void updateMovementState(bool currentState)
    {
        isRunning = currentState;
       
    }

    //Active Train Boxes 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("TrainBox"))
        {
            collision.gameObject.SetActive(false);
            //Sore Point
            score += 10;
            GameManager.instance.scoreText.text = score.ToString();
           if(trainBoxList[curntTrainBoxIndex] != null)
           {
                TrainBoxMeshChange(trainBoxList[curntTrainBoxIndex],collision.gameObject);
                TrainBoxMaterialChange(trainBoxList[curntTrainBoxIndex], collision.gameObject);
               // trainBoxList[curntTrainBoxIndex].GetComponent<Collider>().enabled = true;
                trainBoxList[curntTrainBoxIndex].GetComponent<MeshRenderer>().enabled = true;
               curntTrainBoxIndex++;
           }
          
        }

        //Level Complete
        if (collision.collider.CompareTag("Win"))
        {
            GameManager.instance.LevelComplete();
        }

        //Level Failed
        if (collision.collider.CompareTag("Lose"))
        {
            GameManager.instance.GameLose();
        }

    }

    //Instantiate Train Boxes
    void TrainInstantiate()
    {
       int length = GameObject.FindGameObjectsWithTag("TrainBox").Length;
       trainBoxList = new GameObject[length + reLength];

        for (int i = 0; i < length + reLength ; i++)
        {
            GameObject gm = Instantiate(trainBox, transform.position + new Vector3(0, 0, -1.2f * trainBoxOffSet), Quaternion.identity);
            gm.transform.parent = transform;
            gm.GetComponent<ConfigurableJoint>().connectedBody = curntConnectedBody;
            gm.GetComponent<MeshRenderer>().enabled = false;
            trainBoxOffSet++;
            curntConnectedBody = gm.GetComponent<Rigidbody>();
            trainBoxList[i] = gm;
        }
        reLength = 0;
    }

    public  void TrainReInstantiate()
    {
        int totalBoxes = gameObject.transform.childCount;
        reLength = totalBoxes;
        if (totalBoxes > 0)
        {
            int length = GameObject.FindGameObjectsWithTag("TrainBox").Length;        
            trainBoxList = new GameObject[length + totalBoxes];
            for (int i = 0; i < totalBoxes; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            //Add Train Boxes After some Boxes Cut
            curntConnectedBody = rb;
            trainBoxOffSet = 1;
            curntTrainBoxIndex = totalBoxes;
            TrainInstantiate();
            for (int i = 0; i < totalBoxes; i++)
            {
                trainBoxList[i].GetComponent<MeshRenderer>().enabled = true;
            }

        }
        else
        {
            //If All Train Boxes Cut
            curntConnectedBody = rb;
            trainBoxOffSet = 1;
            curntTrainBoxIndex = 0;
            TrainInstantiate();
        }

        isReInstantiate = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            print("Rechead finish line");
            this.rb.velocity = Vector3.zero;
           
            endTraget = other.GetComponent<FinishController>().getFinishTarget();
            characterInputController.enabled = false;
            reachedGoalLine = true;

            //Win Panel Active
            StartCoroutine(LvLComplete());
            
        }
       
    }
    private bool canPlay()
    {
        return GameManager.instance.currentGameState == GameManager.GameState.play;
    }

    IEnumerator LvLComplete()
    {
        yield return new WaitForSeconds(8f);
        GameManager.instance.LevelComplete();
    }

    private void TrainBoxMeshChange(GameObject boxToChange,GameObject collectedBox)
    {
        boxToChange.GetComponent<MeshFilter>().mesh = collectedBox.GetComponent<MeshFilter>().mesh;
    }

    private void TrainBoxMaterialChange(GameObject boxToChange, GameObject collectedBox)
    {
        boxToChange.GetComponent<MeshRenderer>().material = collectedBox.GetComponent<MeshRenderer>().material;
    }
}
