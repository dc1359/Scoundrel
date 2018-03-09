using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Patroller
/// Agent behivour for patrolling guards
/// </summary>
public class Patroller : MonoBehaviour {
    [SerializeField] private float speed = 2;
    [SerializeField] private Vector2[] patrolPoints;

    /// <summary>
    /// List representing the commands an agent should preform, in order
    /// </summary>
    [HideInInspector] public List<int> commands;
    /// <summary>
    /// Parameters of each command./n
    /// AI_WAIT - ms to hold position
    /// AI_TURN_TO_POINT - index of point to face
    /// AI_FORWARD - index of point to approach
    /// </summary>
    [HideInInspector] public List<int> commandParameters;

    /// <summary>
    /// Command position
    /// </summary>
    private int commandIndex;
    /// <summary>
    /// ms since delay command
    /// </summary>
    private float curWait = 0;
    /// <summary>
    /// Flag for determining if last command is completed.  False will execute command initialization
    /// </summary>
    private bool commandProcessing = false;

    /// <summary>
    /// Attached Rigidbody component, moving and turning
    /// </summary>
    private Rigidbody r;
    /// <summary>
    ///  Field of view script component, attached cone of vision
    /// </summary>
    private FieldOfView fov;

    /// <summary>
    /// Move to next point
    /// </summary>
    public const int AI_FORWARD = 2;
    /// <summary>
    /// Turn to face point
    /// </summary>
    public const int AI_TURN_TO_POINT = 1;
    /// <summary>
    /// Stand still for parameter seconds
    /// </summary>
    public const int AI_WAIT = 0;

    // Use this for initialization
    void Start() {
        r = this.GetComponent<Rigidbody>();
        commandIndex = 0;

        fov = this.GetComponentInChildren<FieldOfView>(true);
    }

    // Update is called once per frame
    void Update() {
        if (CheckSight()) {
            fov.ViewAreaColor = FieldOfView.HOSTILE_VIEWCONE;
        } else {
            fov.ViewAreaColor = FieldOfView.NEUTRAL_VIEWCONE;
        }

        CheckCurrentCommand(AiState);
    }

    /// <summary>
    /// Using field of view, checks if any target of interest is within LoS
    /// </summary>
    /// <returns>True if spotted target of interest</returns>
    bool CheckSight() {
        return (fov.visibleTargets.Count >= 1);
    }

    /// <summary>
    /// Preforms a command from the agent's script
    /// </summary>
    /// <param name="aiState">Index of command to preform</param>
    void CheckCurrentCommand(int aiState)
    {
        switch (aiState)
        {
            case AI_WAIT:
                Wait((float)(AiParamaters / 1000));
                break;
            case AI_TURN_TO_POINT:
                FacePoint(CurrentPatrolPoint);
                break;
            case AI_FORWARD:
                MoveForward(CurrentPatrolPoint);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Agent action, wait in place
    /// </summary>
    /// <param name="ms">Time to wait in milliseconds</param>
    private void Wait(float ms) {
        if (commandProcessing) {
            curWait += Time.deltaTime;
            if (curWait >= ms)
            {
                CommandIndex++;
                commandProcessing = false;
            }
        } else {
            curWait = 0;
            commandProcessing = true;
        }
    }

    // Note, will replace parameter with float angle.
    // TODO: Smooth out turning
    /// <summary>
    /// Agent action, Turn to face point
    /// </summary>
    /// <param name="point">Point to face</param>
    private void FacePoint(Vector3 point) {
        r.transform.LookAt(point);
        //r.transform.rotation = Quaternion.Euler(0, Vector3.Angle(r.position, point), 0);
        CommandIndex++;
    }

    /// <summary>
    /// Agent action, Approaches position
    /// </summary>
    /// <param name="point">Point to approach</param>
    void MoveForward(Vector3 point) {
        if (commandProcessing) {
            r.transform.LookAt(point);
            //r.transform.rotation = Quaternion.Euler(0, Vector3.Angle(r.position, point), 0);
            // Odd behaivour, would slow down when approaching point.
            //r.AddRelativeForce(Vector3.forward * speed);
            r.velocity = ((point - r.position).normalized * speed);
            
            if (Vector3.Distance(r.position, point) <= 0.1) {
                r.velocity = Vector3.zero;
                CommandIndex++;
                commandProcessing = false;
            }
        } else {
            commandProcessing = true;
            r.transform.LookAt(point);
        }
    }

    /// <summary>
    /// Position of agent's script
    /// </summary>
    public int CommandIndex
    {
        get {
            return commandIndex;
        }

        private set {
            commandIndex = value % commands.Count;
        }
    }

    /// <summary>
    /// Current state of agent
    /// </summary>
    public int AiState {
        get {
            return commands[CommandIndex];
        }
    }

    /// <summary>
    /// Parameters of current command
    /// </summary>
    public int AiParamaters {
        get {
            return commandParameters[CommandIndex];
        }
    }

    /// <summary>
    /// Current position agent's command is 
    /// </summary>
    Vector3 CurrentPatrolPoint
    {
        get
        {
            if (AiState == AI_WAIT) return Vector3.zero;

            return new Vector3(patrolPoints[AiParamaters].x, r.position.y, patrolPoints[AiParamaters].y);
        }
    }

}
