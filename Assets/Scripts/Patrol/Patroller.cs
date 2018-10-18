﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class Patroller
/// Agent behivour for patrolling guards
/// </summary>
public class Patroller : MonoBehaviour {
    private float speed {
        get {
            return agent.speed;
        }
        set {
            agent.speed = value;
        }
    }
    [SerializeField] private Vector2[] patrolPoints;

    /// <summary>
    /// List representing the commands an agent should preform, in order
    /// </summary>
    [HideInInspector] public List<int> commands;
    /// <summary>
    /// Parameters of each command.
    /// AI_WAIT - ms to hold position
    /// AI_TURN_TO_POINT - index of point to face
    /// AI_FORWARD - index of point to approach
    /// </summary>
    [HideInInspector] public List<int> commandParameters;

    [HideInInspector] private NavMeshAgent agent;

    /// <summary>
    /// If >= 0, interupts current AI flow and preforms command
    /// </summary>
    private int forceCommand = AI_NULL;
    /// <summary>
    /// Parameters to execute during a forced command
    /// </summary>
    private int forceCommandParameters = -1;

    /// <summary>
    /// The last point spotted by the guard.
    /// </summary>
    private Vector2 PointSpotted;

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
    /// <summary>
    /// For forced AI options.  Resets AI to normal state.
    /// </summary>
    public const int AI_NULL = -1;
    /// <summary>
    /// Chase the player
    /// </summary>
    public const int AI_CHASE = 10;
    /// <summary>
    /// 
    /// </summary>
    public const int AI_SPOT = 11;
    public const int AI_GLANCE = 12;

    /// <summary>
    /// How long it takes the AI to notice the player
    /// </summary>
    public const int NOTICE_TIME = 2000;

    public const float GLANCE_ANGLE_MOD = 90;
    public const int GLANCE_CHANGE = 1500;
    public const int GLANCE_GIVEUP = GLANCE_CHANGE * 4;
    private float glanceAngle, glanceFactor;


    // Use this for initialization
    void Start() {
        r = this.GetComponent<Rigidbody>();
        commandIndex = 0;

        fov = this.GetComponentInChildren<FieldOfView>(true);

        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        if (AiState != AI_CHASE) {
            if (CheckSight()) {
                fov.ViewAreaColor = FieldOfView.ALERTED_VIEWCONE;
                if (AiState != AI_SPOT) ForceAI(AI_SPOT, NOTICE_TIME);
            } else {
                fov.ViewAreaColor = FieldOfView.NEUTRAL_VIEWCONE;
            }
        } else {
            fov.ViewAreaColor = FieldOfView.HOSTILE_VIEWCONE;
        }

        r.angularVelocity = Vector3.zero;
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
                FaceAngle(AiParamaters);
                break;
            case AI_FORWARD:
                MoveForward(CurrentPatrolPoint);
                break;
            case AI_CHASE:
                ChasePlayer(PointSpotted);
                break;
            case AI_SPOT:
                Suspicious((float)(AiParamaters / 1000));
                break;
            case AI_GLANCE:
                Glance((float)(AiParamaters / 1000));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Agent loses target, glances around
    /// </summary>
    /// <param name="seconds">seconds to glance around</param>
    private void Glance(float seconds) {
        if (commandProcessing) {
            curWait += Time.deltaTime;

            if (curWait >= seconds) {
                commandProcessing = false;
                ForceAI(AI_NULL);
            } else {
                if (curWait > (GLANCE_CHANGE * glanceFactor) / 1000) {
                    glanceFactor++;
                    glanceAngle += (GLANCE_ANGLE_MOD - Random.value * GLANCE_ANGLE_MOD * 2) % 360;
                }

                float rot = Utility.DesireSmoothAngle(transform.rotation.eulerAngles.y, glanceAngle);

                if (rot < 1) {
                    transform.Rotate(Vector3.up, rot);
                }
            }
            Debug.Log(agent.isStopped + ":  " + (seconds - curWait) + ": factor: " + glanceFactor);
        } else {
            curWait = 0;
            glanceAngle = transform.rotation.eulerAngles.y;
            glanceFactor = 0;
            commandProcessing = true;
        }
    }

    /// <summary>
    /// Agent sees something, becomes suspicious
    /// </summary>
    /// <param name="seconds">Seconds to confirm</param>
    private void Suspicious(float seconds) {
        if (commandProcessing) {
            curWait += Time.deltaTime;
            if (!CheckSight() && curWait >= seconds) {
                commandProcessing = false;
                ForceAI(AI_NULL, 0);
            } else if (curWait >= seconds) {
                commandProcessing = false;
                ForceAI(AI_CHASE, 0);
            }
        } else {
            curWait = 0;
            agent.isStopped = true;
            commandProcessing = true;
        }
    }

    /// <summary>
    /// Chase down the player, or the player's last spotted location.
    /// </summary>
    /// <param name="scoutPoint">Player's last spotted location</param>
    private void ChasePlayer(Vector2 scoutPoint) {
        Vector2 targetPoint;

        if (CheckSight()) {
            targetPoint = new Vector2();
            targetPoint.x = fov.visibleTargets[0].position.x;
            targetPoint.y = fov.visibleTargets[0].position.z;

            // Remember the last position player was seen
            PointSpotted = targetPoint;
        } else {
            targetPoint = scoutPoint;
            if (ReachedDestination(targetPoint)) {
                r.velocity = Vector3.zero;
                
                ForceAI(AI_GLANCE, GLANCE_GIVEUP);
            }
        }

        //r.transform.LookAt(new Vector3(targetPoint.x, r.transform.position.y, targetPoint.y));
        //r.AddRelativeForce(Vector3.forward * Mathf.RoundToInt(speed * 1.5f));
        Debug.Log(agent.isStopped);

        agent.isStopped = false;
        agent.SetDestination(new Vector3(targetPoint.x, r.position.y, targetPoint.y));
    }

    /// <summary>
    /// Agent action, wait in place
    /// </summary>
    /// <param name="seconds">Time to wait in seconds</param>
    private void Wait(float seconds) {
        if (commandProcessing) {
            curWait += Time.deltaTime;
            if (curWait >= seconds)
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
    /// <param name="angle">Angle to face</param>
    private void FaceAngle(float angle) {
        if (commandProcessing) {
            float rot = Utility.DesireSmoothAngle(transform.rotation.eulerAngles.y, angle);

            if (Mathf.Abs(rot) <= 1) {
                CommandIndex++;
                commandProcessing = false;
            } else {
                transform.Rotate(Vector3.up, rot);
            }
        } else {
            commandProcessing = true;
        }
    }

    /// <summary>
    /// Agent action, Approaches position
    /// </summary>
    /// <param name="point">Point to approach</param>
    void MoveForward(Vector3 point) {
        if (commandProcessing) {
            agent.isStopped = false;
            agent.SetDestination(point);
            //r.transform.LookAt(point);
            //r.transform.rotation = Quaternion.Euler(0, Vector3.Angle(r.position, point), 0);
            // Odd behaivour, would slow down when approaching point.
            //r.AddRelativeForce(Vector3.forward * speed);
            //r.velocity = ((point - r.position).normalized * speed);
            
            if (ReachedDestination(point)) {
                r.velocity = Vector3.zero;
                CommandIndex++;
                commandProcessing = false;
                agent.isStopped = true;
            }
        } else {
            commandProcessing = true;
            //r.transform.LookAt(point);
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

    public void ForceAI(int command, int parameters = 0) {
        forceCommand = command;
        forceCommandParameters = parameters;
        commandProcessing = false;
    }

    /// <summary>
    /// Current state of agent
    /// </summary>
    public int AiState {
        get {
            if (forceCommand < 0)
                return commands[CommandIndex];

            return forceCommand;
        }
    }

    /// <summary>
    /// Parameters of current command
    /// </summary>
    public int AiParamaters {
        get {
            if (forceCommand < 0)
                return commandParameters[CommandIndex];

            return forceCommandParameters;
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

    private bool ReachedDestination(Vector3 destination) {
        return ReachedDestination(new Vector2(destination.x, destination.z));
    }

    private bool ReachedDestination(Vector2 destination) {
        Vector2 source = new Vector2(transform.position.x, transform.position.z);
        return (Vector2.Distance(source, destination) < Mathf.Min(agent.stoppingDistance * 2, agent.stoppingDistance + 1));
    }
}
