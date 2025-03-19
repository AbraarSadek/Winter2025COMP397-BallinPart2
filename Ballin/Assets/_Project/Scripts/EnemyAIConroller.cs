/*
 * 
 * Script Name: EnemyAIConroller
 * Created By: Abraar Sadek
 * Date Created: 02/20/2025
 * Last Modified: 02/23/2025
 * 
 * Script Purpose: To control the enemy character's AI...
 * 
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//EnemyAIController Class
public class EnemyAIController : MonoBehaviour {

    //Public Component Reference Variables
    [Header("Component References: ")]
    public Transform player; //Transform variable to store the player's transform
    public NavMeshAgent navMeshAgent; //NavMeshAgent variable to store the AI's NavMeshAgent

    //Public Script References
    [Header("Script References: ")]
    public PlayerHealthController PlayerHealthController; //Script Reference to the PlayerHealthController script

    //Public Variables
    [Header("Player Distance Variables: ")]
    public float distanceFromPlayer; //Float variable to that will store the distance between the enemy and the players

    //Public Variables
    [Header("Patrolling & Waypoint Variables: ")]
    [SerializeField] private List<Transform> waypoints; //List of Transform variables that will store the waypoints
    [SerializeField] private float waypointArrivalThreshold = 5.2f; //Float variable that will store the threshold for detecting arrival arrival at a waypoint
    private int currentWaypointIndex = 0;
    [SerializeField] private float patrolSpeed = 8f;
    private bool isPatrolling = false;

    //Private Variables
    [Header("Enemy Chasing Variables: ")]
    [SerializeField] private float chaseRange = 20f;
    [SerializeField] private float chaseSpeed = 10f;
    [SerializeField] private float stopChaseRange = 25f;
    private bool isChasing = false;

    //Private Variables For Enemy Attacking
    [Header("Enemy Attacking Variables: ")]
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackForce = 10f;

    [SerializeField] private float dashSpeedMultiplier = 2f;
    [SerializeField] private float dashDuration = 1f;

    [SerializeField] private float stunDuration = 0.5f;

    [SerializeField] private float slowMotionTimeScale = 0.5f;
    [SerializeField] private float slowMotionDuration = 0.3f;
    
    private bool isAttacking = false;
    private bool isStunned = false;

    //Awake Method - That Will Be Called When Tthe Script Is Loaded
    private void Awake()  {

        navMeshAgent = GetComponent<NavMeshAgent>();

        PlayerHealthController = player.GetComponent<PlayerHealthController>();

        //If-Statement That Will Check If 
        if (waypoints.Count > 0) {
            isPatrolling = true; //Set the 'isPatrolling' variable to true
            navMeshAgent.destination = waypoints[currentWaypointIndex].position; //Set the NavMeshAgent's destination to the waypoint at the current index in the list
            navMeshAgent.speed = patrolSpeed; //
        } //End of If-Statement

    } //End of Awake Method

    //Update Method - 
    void Update() {

        //If-Statement - That Will Check If The Enemy Has No Waypoints, Is Not On The NavMesh, Or Is Stunned
        if (waypoints.Count == 0 || !navMeshAgent.isOnNavMesh || isStunned) {
            return;
        } //End of If-Statement

        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position); //Get and store the distance between the enemy and the player into the 'distanceFromPlayer' variable

        //If-Statement - That Will Check If The Enemy Is Attacking The Player
        if (isAttacking) { return; } //End of If-Statement

        //If-Else Statement - That Will Check If The Enemy Is Chasing The Player
        if (isChasing) {

            //Nest Else-If Statement - That Will Check If The Player Position Is Outside of The Stop Chasing Range
            if (distanceFromPlayer > stopChaseRange) {
                isChasing = false; //Set the 'isChasing' variable to false
                navMeshAgent.speed = patrolSpeed; //Set the NavMeshAgent's speed to the value of the 'patrolSpeed' variable
                MoveEnemyToNextWaypoint(); //Call the 'MoveEnemyToNextWaypoint' method
            } else if (distanceFromPlayer <= attackRange) /*Check If The Player Position Is Inside of The Attack Range*/ { 
                StartCoroutine(MakeEnemyAttackPlayer()); //Call the 'MakeEnemyAttackPlayer' method
            } else {
                navMeshAgent.SetDestination(player.position); //Set the NavMeshAgent's destination to the player's position
            } //End of Nest Else-If Statement

        } else {

            //Nested If-Else Statement - That Will Check If The Player Position Is Inside of The Chase Range
            if (distanceFromPlayer <= chaseRange) {
                MakeEnemyChasePlayer(); //Call the 'MakeEnemyChasePlayer' method
            } else {
                MakeEnemyPatrolWaypoints(); //Call the 'MakeEnemyPatrolWaypoints' method
            } //End of Nested If-Else Statement

        }//End of If-Else Statement

    } //End of Update Method

    //MakeEnemyPatrolWaypoints Method - That Will Make The Enemy Patrol Waypoints
    void MakeEnemyPatrolWaypoints() {

        //If-Statement - That Will 
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= waypointArrivalThreshold && navMeshAgent.velocity.sqrMagnitude < 0.1f) {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; //Increment the current waypoint index
            Debug.Log($"Moving to waypoint {currentWaypointIndex}: {waypoints[currentWaypointIndex].position}");
            MoveEnemyToNextWaypoint(); //Call the 'MoveEnemyToNextWaypoint' method
        } //End of If-Statement

    } //End of MakeEnemyPatrolWaypoints Method

    //MoveToNextWaypoint Method - That Will Move The Enemy To The Next Waypoint In The List
    private void MoveEnemyToNextWaypoint() {

        //If-Statement - That Will 
        if (waypoints.Count == 0) {
            return;
        } //End of If-Statement

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position); //Set the NavMeshAgent's destination to the waypoint at the current index in the list
        navMeshAgent.isStopped = false; // Ensure the agent is not stopped

    } //End of MoveToNextWaypoint Method

    //MakeEnemyChasePlayer Method - That Will Make The Enemy Chase The Player When They Get Close
    void MakeEnemyChasePlayer() {
        isChasing = true; //Set the 'isChasing' variable to true
        navMeshAgent.speed = chaseSpeed; //Set the NavMeshAgent's speed to the value of the 'chaseSpeed' variable
    } //End of MakeEnemyChasePlayer Method

    //MakeEnemyAttackPlayer Method - That Will Make The Enemy Attack The Player When They Get Close
    private IEnumerator MakeEnemyAttackPlayer() {

        //If-Statement - That Will 
        if (isAttacking || isStunned) {
            yield break;
        } //End of If-Statement

        isAttacking = true; //Set the 'isAttacking' variable to true
        navMeshAgent.isStopped = true; //Stop the NavMeshAgent from moving

        //Set the NavMeshAgent's velocity to the direction of the player
        Vector3 attackDirection = (player.position - transform.position).normalized; //Vector3 variable that will get the direction of the player from the enemy

        //Temporarily Increase The Enemys Speed While Preforming Attack/Tackle
        float enemyOriginalSpeed = navMeshAgent.speed;
        navMeshAgent.speed *= dashSpeedMultiplier;

        //Set The Nav Mesh Agent Destination To The Players Position
        navMeshAgent.SetDestination(player.position);

        yield return new WaitForSeconds(dashDuration);

        //Reset The Enemys Speed And Stun
        navMeshAgent.speed = enemyOriginalSpeed;
        navMeshAgent.isStopped = true;

        //End The Enemys Attack
        yield return new WaitForSeconds(0.5f); //Wait for 0.5 seconds before continuing
        isAttacking = false; //Set the 'isAttacking' variable to false
        StartCoroutine(StunEnemyAfterAttack()); //Call the 'StunEnemy' method

    } //End of MakeEnemyAttackPlayer Method

    //StunEnemyAfterAttack Method - That Will 'Stun' The Enemy After Attacking The Player So The Player Has Time To Get Away
    private IEnumerator StunEnemyAfterAttack() {

        navMeshAgent.stoppingDistance = 0.2f; //Set the 'navMeshAgent's' 'stoppingDistance' to 0.2f
        isStunned = true; //Set the 'isStunned' variable to true
        navMeshAgent.isStopped = true; //Stop the NavMeshAgent from moving

        yield return new WaitForSeconds(stunDuration); //Wait for the stun duration

        navMeshAgent.stoppingDistance = 5f; //Set the 'navMeshAgent's' 'stoppingDistance' to 5f
        navMeshAgent.isStopped = false; //Resume the NavMeshAgent's movement
        isStunned = false; //Set the 'isStunned' variable to false

    } //End of StunEnemyAfterAttack Method

    //OnTriggerEnter Method - Will Handle The Enemys Collision-based Attack Detection
    private void OnTriggerEnter(Collider other) {

        //If-Statement - That Will Check If The Enemey Is Attacking
        if (!isAttacking) { return; }

        //If-Statement - That Will Check If The Object The Enemy Colided With Has The "Player" Tag
        if (other.CompareTag("Player")) {

            PlayerHealthController.DamageTaked(1); //Call the 'DamageTaked' method from the 'PlayerHealthController' script and pass it the value of 1

            StartCoroutine(KnockbackPlayer(other.transform)); //Call the '' method

            StartCoroutine(SlowMotionEffect()); //Call the 'SlowMotionEffect' method

        } //End of If-Statement 

    } //End of OnTriggerEnter Method

    //StartCoroutine Method - That Will Knock The Player Back When They Are Damaged
    private IEnumerator KnockbackPlayer(Transform playerTransform) {

        //Calculate The Knock Back Force
        Vector3 knockbackDirection = (playerTransform.position - transform.position).normalized; //Calculate the direction of the knockback 
        knockbackDirection += Vector3.up * 0.5f; //Add an upward force to the knockback
        knockbackDirection += Vector3.down * 0.5f; //Add an upward force to the knockback
        knockbackDirection.Normalize(); //Normalize 'knockbackDirection' variable once more to maintain consistent force

        Vector3 targetPosition = playerTransform.position + (knockbackDirection * (attackForce * 2)); //Calculate the end point of the knockback

        float elapsedTime = 0f; //Float variable that will hold how much time has passed
        float knockbackDuration = 1; //Float variable that will hold the duration of the knockback

        //While Loop - That Will Run As Long As The Value of The 'elapsedTime' Variable Is Less Then The Value of The 'knockbackDuration' Variable
        while (elapsedTime < knockbackDuration) {

            playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;

        } //End of While Loop

        playerTransform.position = targetPosition;

    } //End of KnockbackPlayer Method

    //SlowMotionEffect Method - That Will Apply A Slow Motion Effect After The Player Lands From The Knockback
    private IEnumerator SlowMotionEffect() {

        Time.timeScale = slowMotionTimeScale;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;

    } //End of SlowMotionEffect Method

    //OnDrawGizmos Method - That Will Draw Gizmos In The Scene
    private void OnDrawGizmosSelected() {

        //If-Statement - That Will Check 
        if (player != null) {

            //Draw A Green Sphere At The Enemy's Position With A Radius Of The 'chaseRange' Variable
            Gizmos.color = Color.green; //Set the Gizmo's color to green
            Gizmos.DrawWireSphere(transform.position, chaseRange); //Draw a green sphere at the enemy's position with a radius of the 'chaseRange' variable

            //Draw A Yellow Sphere At The Enemy's Position With A Radius Of The 'stopChaseRange' Variable
            Gizmos.color = Color.yellow; //Set the Gizmo's color to yellow
            Gizmos.DrawWireSphere(transform.position, stopChaseRange); //Draw a yellow sphere at the enemy's position with a radius of the 'stopChaseRange' variable

            //Draw A Red Sphere At The Enemy's Position With A Radius Of The 'attackRange' Variable
            Gizmos.color = Color.red; //Set the Gizmo's color to red
            Gizmos.DrawWireSphere(transform.position, attackRange); //Draw a red sphere at the enemy's position with a radius of the 'attackRange' variable

        } //End of If-Statement

    } //End of OnDrawGizmos Method

} //End of EnemyAIController Class


/*
 * 
 * public NavMeshAgent agent; //NavMeshAgent Variable to store the AI's NavMeshAgent
    public Transform player; //Transform Variable to store the player's transform
    public LayerMask whatIsGround, whatIsPlayer; //LayerMasks Variables to store the layers that the AI can see on

    //Variables For Enemy Patrolling 
    public Vector3 walkPoint; //Vector3 Variable to store the AI's walk point
    bool walkPointSet; //Bool Variable to check if the AI has a walk point set
    public float walkPointRange; //Float Variable to store the AI's walk point range

    //Variables For Enemy Patrolling
    public float timeBetweenAttacks; //Float Variable to store the AI's time between attacks
    bool alreadyAttacked; //Bool Variable to check if the AI has already attacked

    //Variables For Enemy States
    public float sightRange, attackRange; //Float Variables to store the AI's sight and attack ranges
    public bool playerInSightRange, playerInAttackRange; //Bool Variables to check if the player is in the AI's sight and attack ranges

    //Private Variables

    //Awake Method - Is Called Before The Start Method
    private void Awake() {
        player = GameObject.Find("Player").transform; //Find the player's transform
        agent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component
    } //End of Awake Method

    //Start Method - Is Called Once Before The First Execution of Update After The MonoBehaviour Is Created
    void Start() {

    } //End of Start Method

    //Update Method - Is Called Once Per Frame
    void Update() {

        //Check If The Player Is In The AI's Sight And Attack Range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //If-Statement - That Will Check If The Player Is Not In The AI's Sight And Attack Range
        if (!playerInSightRange && !playerInAttackRange) {
            //Make Emeny Patrol
            Patroling(); //Call the 'Patrolling' Method
        } //End of If-Statement

        //If-Statement - That Will Check If The Player Is In The AI's Sight But Not In There Attack Range
        if (playerInSightRange && !playerInAttackRange) {
            //Make Emeny Chase Player
            ChasePlayer(); //Call the 'ChasePlayer' Method
        } //End of If-Statement

        //If-Statement - That Will Check If The Player Is In The AI's Attack And Sight Range
        if (playerInAttackRange && playerInSightRange) {
            //Make Enemy Attack Player
            AttackPlayer(); //Call the 'AttackPlayer' Method
        } //End of If-Statement

    } //End of Update Method

    //Patrolling Method - That Will Make The Enemy Patrol The Map
    private void Patroling() {

        //If-Statement - That Will Check If The 'walkPointSet' variable is false
        if (!walkPointSet) { 
            SearchWalkPoint(); //Call the 'SearchWalkPoint' Method
        } //End of If-Statement

        //If-Statement - That Will Check If The 'walkPointSet' Variable Is Set To 'true'
        if (walkPointSet) {
            agent.SetDestination(walkPoint); //Set the AI's destination to the calculated walk point
        } //End of If-Statement

        //Create  A New Vector3 Variable To Store The Distance Between The AI And The Walk Point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //If-Statement - That Will Check If The Value Of The 'distanceToWalkPoint' Variable Is Less Than 1 (Enemy Has Arrived At The Walk Point)
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false; //Set the 'walkPointSet' variable to 'false'
        } //End of If-Statement

    } //End of Patrolling Method

    //SearchWalkPoint Method - That Will Calculate The Enemys Walk Point
    private void SearchWalkPoint() {
        
        //Calculate The AI's Random Walk Point That Is In Range
        float randomZ = Random.Range(-walkPointRange, walkPointRange); //Float variable to store the AI's random Z-aixs walk point
        float randomX = Random.Range(-walkPointRange, walkPointRange); //Float variable to store the AI's random X-aixs walk point

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //Create a new Vector3 variable to store the AI's walk point

        //If-Statement - That Will Check If The Calculated Walk Point Is Valid (Within The Map)
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true; //Set the 'walkPointSet' variable to true
        } //End of If-Statement

    } //End of SearchWalkPoint Method

    //ChasePlayer Method - That Will Make The Enemy Chase The Player
    private void ChasePlayer() {
        agent.SetDestination(player.position); //Set the Enemy's destination to the player
    } //End of ChasePlayer Method

    //AttackPlayer Method - 
    private void AttackPlayer() {

        transform.LookAt(player); //Make the enemy look at the player

        //If-Statement - That Will Check If The Enemy Has Already Attacked The Player
        if (!alreadyAttacked) {

            //Put Attack Code here:

            alreadyAttacked = true; //Set the 'alreadyAttacked' variable to 'true'
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //Invoke the 'ResetAttack' Method
        } //End of If-Statement

    } //End of AttackPlayer Method

    //ResetAttack Method - That Will Reset The Enemy's Attack 
    private void ResetAttack() {
        playerInAttackRange = false; //Set the 'playerInAttackRange' variable to 'false'
    } //End of ResetAttack Method

    //OnDrawGizmosSelected Method - That Will Visually Represent The Enemy's Attack And Sight Range On The Editor
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red; //Set the color of the gizmos to red
        Gizmos.DrawWireSphere(transform.position, attackRange); //Draw a wire sphere at the enemy's position with a radius of the attack range
        Gizmos.color = Color.yellow; //Set the color of the gizmos to yellow
        Gizmos.DrawWireSphere(transform.position, sightRange); //Draw a wire sphere at the enemy's position with a radius of the sight range
    } //End of OnDrawGizmosSelected Method
 * 
 * 
 */

/*
 * //Private Variables
    private NavMeshAgent agent; //NavMeshAgent Variable to store the AI's NavMeshAgent

    [SerializeField] private List<Transform> waypoints; //List Variable to store the enemy's waypoints
    private int currentWaypointIndex; //Int Variable to store the current waypoint index

    [SerializeField] private Transform player; //Transform Variable to store the player's transform

    [SerializeField] private float distanceThreshold; //Float Variable to store the enemy's distance threshold
    [SerializeField] private float chaseRange = 5f; //Float Variable to store the enemy's chase range
    private bool isChasing = false; //Bool Variable to check if the enemy is chasing the player
    [SerializeField] private float losePlayerDistance = 8f; //Float Variable to store the enemy's lose player distance

    [SerializeField] private LayerMask obstacleMask; //LayerMask Variable to store the obstacle layer

    //Awake Method - Called When The Scipt Is Loaded
    private void Awake() {

        agent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component attached to the enemy

        //If-Statement - That Will Check If The Waypoints List Is Not Empty
        if (waypoints.Count > 0) {
            agent.SetDestination(waypoints[currentWaypointIndex].position); //Set the NavMeshAgent's destination to the first waypoint
        } //End of If-Statement

    } //End of Awake Method

    //Update Method - Called Once Per Frame
    private void Update() {

        //If-Statement - That Will Check If The Enemy Is Not On The NavMesh
        if (!agent.isOnNavMesh) {
            return;
        } //End of If-Statement

        float distanceToPlayer = Vector3.Distance(transform.position, player.position); //Float Variable to store the distance between the enemy and the player

        //Else-If Statement - That Will Check If The Player Is Within The Enemy's Chase Range And Has Line Of Sight
        if (distanceToPlayer <= chaseRange && HasLineOfSight()) {
            StartChasing(); //Call the 'StartChasing' Method
        } else if (isChasing && distanceToPlayer > losePlayerDistance) {
            StopChasing(); //Call the 'StopChasing' Method
        }//End of Else-If Statement

        //If-Else Statement - That Will Check If The Enemy Is Not Chasing
        if (isChasing) {
            agent.SetDestination(player.position); //Set the NavMeshAgent's destination to the player's position
        }
        else {
            Patrol(); //Call the 'Patrol' Method
        }//End of If-Else Statement

    } //End of Update Method

    //Patrol Method - That Will Make The Enemy Patrol
    private void Patrol(){

        //If-Statement - That Will Check If The Waypoints List Is Not Empty
        if (waypoints.Count > 0 || !agent.isOnNavMesh) {
            return; //Return value
        } //End of If-Statement

        //If-Statement - That Will Check If The Remaining Distance Is Less Than Or Equal To The Distance Threshold
        if (agent.remainingDistance <= distanceThreshold) {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; //Increment the current waypoint index by 1 and then modulus it by the number of waypoints
            agent.SetDestination(waypoints[currentWaypointIndex].position); //Set the NavMeshAgent's destination to the current waypoint
        } //End of If-Statement

    } //End of Patrol Method

    //StartChasing Method - That Will Make The Enemy Chase The Player
    private void StartChasing() {
        isChasing = true; //Set the 'isChasing' variable to true
        agent.SetDestination(player.position); //Set the NavMeshAgent's destination to the player
    } //End of StartChasing Method

    //StopChasing Method - That Will Make The Enemy Stop Chasing The Player
    private void StopChasing() { 
        isChasing = false; //Set the 'isChasing' variable to false
        currentWaypointIndex = GetClosestWaypointIndex(); //Call the 'GetClosestWaypointIndex' Method
        agent.SetDestination(waypoints[currentWaypointIndex].position); //Set the NavMeshAgent's destination to the current waypoint
    } //End of StopChasing Method

    //HasLineOfSight Method - That Will Check To See If The Enemy Has A Clear Line of Sight To The Player
    private bool HasLineOfSight() { 

        Vector3 directionToPlayer = (player.position - transform.position).normalized; //Vector3 Variable to store the direction from the enemy to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); //Float Variable to store the distance between the enemy and the player

        //If-Statement - That Will Check If The Enemy Has A Clear Line of Sight To The Player
        if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask)) {
            return true; //Return value (No obstacles detected, enemy has line of sight)
        } //End of If-Statement

        return false; //Return value (Obstacles detected, enemy does not have line of sight)

    } //End of HasLineOfSight Method

    //GetClosestWaypointIndex Method - That Will Return The Index Of The Closest Waypoint To The Enemy
    private int GetClosestWaypointIndex() {

        int closestIndex = 0; //Int Variable to store the index of the closest waypoint
        float minDistance = Mathf.Infinity; //Float Variable to store the minimum distance between the enemy and the waypoints

        //For Loop - That Will Loop Through The Waypoints List
        for (int i = 0; i < waypoints.Count; i++) {
            float distance = Vector3.Distance(transform.position, waypoints[i].position); //Float Variable to store the distance between the enemy and the waypoint
            //Nested If-Statement - That Will Check If The Current Distance Is Less Than The Minimum Distance
            if (distance < minDistance) {
                minDistance = distance; //Set the minimum distance to the current distance
                closestIndex = i; //Set the closest index to the current index
            } //End of Nested If-Statement
        } //End of For Loop
        return closestIndex; //Return the index of the closest waypoint
    } //End of 

    //OnDrawGizmos Method - That Will Draw Gizmos In The Unity Editor
    private void OnDrawGizmos() {

        //If-Statement - That Will Check If The Waypoints List Is Not Empty
        if (waypoints.Count > 0 || !agent.isOnNavMesh) {
            return; //Return value
        } //End of If-Statement

        Gizmos.color = Color.red; //Set the Gizmo's color to red
        //For Loop - That Will Loop Through The Waypoints List
        for (int i = 0; i < waypoints.Count; i++) {

            Gizmos.DrawSphere(waypoints[i].position, 0.2f); //Draw a sphere at the position of the waypoint

            //If-Statement - That Will Check
            if (i > 0) {
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position); //Draw a line between the previous waypoint and the current waypoint
            } //End of If-Statement

        } //End of For Loop

        //Draw Chase Range
        Gizmos.color = Color.blue; //Set the Gizmo's color to blue
        Gizmos.DrawWireSphere(transform.position, chaseRange); //Draw a wire sphere at the position of the enemy

        //Draw For Lose PLayer Range
        Gizmos.color = Color.green; //Set the Gizmo's color to green
        Gizmos.DrawWireSphere(transform.position, losePlayerDistance); //Draw a wire sphere at the position of the enemy

        //If-Statement - That Will Check If The Player Transform Is Not Null
        if (player != null) {
            Gizmos.color = Color.yellow; //Set the Gizmo's color to yellow
            Gizmos.DrawLine(transform.position, player.position); //Draw a line from the enemy to the player
        } //End of If-Statement

    } //End of OnDrawGizmos Method
*/

/*
 * 

    //Public Variables
    public NavMeshAgent navMeshAgent; //NavMeshAgent Variable to store the enemy's 'NavMeshAgent' component
   
    public float enemyStartWaitTime; //Float Variable to store the enemy's 
    public float enemyTimeToRotate; //Float Variable to store the enemy's 
   
    public float enemySpeedWalk; //Float Variable to store the enemy's
    public float enemySpeedRun; //Float Variable to store the enemy's

    public float enemyViewRadius; //Float Variable to store the enemy's
    public float enemyViewAngle; //Float Variable to store the enemy's

    public LayerMask playerMask; //LayerMask Variable to store the players mask
    public LayerMask obstacleMask; //LayerMask Variable to store the obstacles mask

    public float meshResolution; //Float Variable to store
                                 //
    public float edgeIterations; //Float Variable to store
    public float edgeDistance; //Float Variable to store

    [SerializeField] private List<Transform> waypoints; //List Variable to store the waypoints for the enemy to move between
    private int m_CurrentWaypointIndex = 0; //Int Variable to store the current waypoint index

    Vector3 playerLastPosition; //Vector3 Variable to store the player's last position
    Vector3 m_PlayerPosition; //Vectore3 Variable to store the player's position

    float m_WaitTime; //Float Variable to store the wait time
    float m_TimeToRotate; //Float Variable to store the time to rotate

    bool m_PlayerInRange; //Float Variable to store the player in range
    bool m_PlayerNear; //Float Variable to store the player near 

    bool m_IsPatrolling; //Bool Variable to store if the enemy is patrolling
    bool m_HasCaughtThePlayer; //Bool Variable to store  if the player has been caught

    //Start Method - Is Called Once Before The First Execution of Update After The MonoBehaviour Is Created
    void Start() {

        m_PlayerPosition = Vector3.zero; //Set the player position to zero
        
        m_IsPatrolling = true; //Set the is patrolling to true
        m_HasCaughtThePlayer = false; //Set the has caught the player to false
        m_PlayerInRange = false; //Set the player in range to false

        m_WaitTime = enemyStartWaitTime; //Set the wait time to the enemy start wait time
        m_TimeToRotate = enemyTimeToRotate; //Set the time to rotate to the enemy time to rotate

        m_CurrentWaypointIndex = 0; //Set the current waypoint index to 0
        
        navMeshAgent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component
        navMeshAgent.isStopped = false; //Set the 'NavMeshAgent.isStopped' to false
        navMeshAgent.speed = enemySpeedWalk; //Set the 'NavMeshAgent.speed' to the enemy speed walk
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); //Set the 'NavMeshAgent.destination' to the current waypoint

    } //End of Start Method

    //Update Method - Is Called Once Per Frame
    void Update() {

        EnvironmentView(); //Call the 'EnvironmentView' Method

        //If-Else Statement - That Will 
        if (!m_IsPatrolling) {
            Chasing(); //Call the 'Chasing' Method
        } else {
            Patroling(); //Call the 'Patroling' Method
        } //End of If-Statement

    } //End of Update Method

    //Patroling Method - That Will Make The Enemy Patrol Between Set Waypoints
    private void Patroling() {

        //If-Statement - That Will Check Is The Enemy Is Near The Player
        if (m_PlayerNear) {
            //Nested If-Else Statement 1 - That Will Check 
            if (m_TimeToRotate <= 0) {
                MoveEnemy(enemySpeedWalk); //Call the 'MoveEnemy' Method
                LookingAtPlayer(playerLastPosition); //Call the 'LookingAtPlayer' Method
            } else {
                StopEnemy(); //Call the 'StopEnemy' Method
                m_TimeToRotate -= Time.deltaTime; //Decrement the time to rotate
            } //End of Nested If-Else Statement 1
        } else {  
            m_PlayerNear = false; //Set the player near to false
            playerLastPosition = Vector3.zero; //Set the player last position to zero
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); //Set the 'NavMeshAgent.destination' to the current waypoint

            //Nested If Statement 2 - That Will Check
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                //Nested If-Else Statement 3 - That Will Check
                if (m_WaitTime <= 0) {
                    MoveToNextWaypoitn(); //Call the 'MoveToNextWaypoitn' Method
                    MoveEnemy(enemySpeedWalk); //Call the 'MoveEnemy' Method
                    m_WaitTime = enemyStartWaitTime; //Set the wait time to the enemy start wait time
                } else {
                    StopEnemy(); //Call the 'StopEnemy' Method
                    m_WaitTime -= Time.deltaTime; //Decrement the wait time
                }//End of Nested If-Else Statement 3
            } //End of Nested If Statement 2

        }//End of If-Else Statement

    } //End of Patroling Method

    //Chasing Method - That Will Make The Enemy Chase The Player When They Get To Close 
    private void Chasing() {

        m_PlayerNear = false; //Set the player near to false
        playerLastPosition = Vector3.zero; //Set the player last position to zero
        
        //If-Statement - That Will 
        if (!m_HasCaughtThePlayer) {
            MoveEnemy(enemySpeedRun); //Call the 'MoveEnemy' Method
            navMeshAgent.SetDestination(m_PlayerPosition); //Set the 'NavMeshAgent.destination' to the player position
        } //End of If-Statement

        //If Statement - That Will 
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {

            //Nested If-Else Statement 1 - That Will
            if (m_WaitTime <= 0 && !m_HasCaughtThePlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)  {
                m_IsPatrolling = true; //Set the is patrolling to true
                m_PlayerNear = false; //Set the player near to false    
                MoveEnemy(enemySpeedWalk); //Call the 'MoveEnemy' Method
                m_TimeToRotate = enemyTimeToRotate; //Set the time to rotate to the enemy time to rotate
                m_WaitTime = enemyStartWaitTime; //Set the wait time to the enemy start wait time
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); //Set the 'NavMeshAgent.destination' to the current waypoint
            } else {
                //Nested If-Statement 2 - That Will
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) {
                    StopEnemy(); //Call the 'StopEnemy' Method
                    m_WaitTime -= Time.deltaTime; //Decrement the wait time
                } //End of Nested If-Statement 2
            } //End of Nested If-Else Statement

        } //End of If-Statement

    } //End of Chasing Method


    //MoveEnemy Method - That Will 
    void MoveEnemy(float speed) {
        navMeshAgent.isStopped = false; //Set the 'NavMeshAgent.isStopped' to false
        navMeshAgent.speed = speed; //Set the 'NavMeshAgent.speed' to the speed
    } //End of MoveEnemy Method

    //StopEnemy Method - That Will 
    void StopEnemy() { 
        navMeshAgent.isStopped = true; //Set the 'NavMeshAgent.isStopped' to true
        navMeshAgent.speed = 0; //Set the 'NavMeshAgent.speed' to zero
    } //End of StopEnemy Method

    //MoveToNextWaypoitn Method - That Will Move The Enemy To The Next Waiypoint
    public void MoveToNextWaypoitn() {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Count; //Set the current waypoint index to the next waypoint index
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); //Set the 'NavMeshAgent.destination' to the current waypoint
    } //End of MoveToNextWaypoitn Method

    //PlayerHasBeenCaught Method - Is Called When The Player Has Been Caught
    void PlayerHasBeenCaught() {
        m_HasCaughtThePlayer = true; //Set the has caught the player to true
    } //End of PlayerHasBeenCaught Method

    //LookingAtPlayer Method - 
    void LookingAtPlayer(Vector3 player) { 

        navMeshAgent.SetDestination(player); //Set the 'NavMeshAgent.destination' to the player

        //If-Statement - That Will 
        if (Vector3.Distance(transform.position, player) <= 0.3) {
            //Nested If-Else Satement - That Will 
            if (m_WaitTime <= 0) {
                m_PlayerNear = false; //Set the player near to false
                MoveEnemy(enemySpeedRun); //Call the 'MoveEnemy' Method
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); //Set the 'NavMeshAgent.destination' to the current waypoint
                m_WaitTime = enemyStartWaitTime; //Set the wait time to the enemy start wait time
                m_TimeToRotate = enemyTimeToRotate; //Set the time to rotate to the enemy time to rotate
            } else {
                StopEnemy(); //Call the 'StopEnemy' Method
                m_WaitTime -= Time.deltaTime; //Decrement the wait time
            } //End of Nested If-Else Satement 

        } //End of If-Statement

    } //End of LookingAtPlayer Method

    //EnvironmentView Method - That Will 
    void EnvironmentView() { 

        Collider[] playerInRange = Physics.OverlapSphere(transform.position, enemyViewRadius, playerMask); //Get the player in range

        //For Loop - That Will 
        for (int i = 0; i < playerInRange.Length; i++) { 

            Transform player = playerInRange[i].transform; //Transform Variable to store the player's transform
            Vector3 directionToPlayer = (player.position - transform.position).normalized; //Vector3 Variable to store the direction to the player

            //Nested If-Statement - That Will 
            if (Vector3.Angle(transform.forward, directionToPlayer) < enemyViewAngle / 2) {

                float distanceToPlayer = Vector3.Distance(player.position, transform.position); //Float Variable to store the distance to the player

                //Nested If-Else Statement 2 - That Will 
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask)) {
                    m_PlayerInRange = true; //Set the player in range to true
                    m_IsPatrolling = false; //Set the is patrolling to false
                } else {
                    m_PlayerInRange = false; //Set the player in range to false
                }//End of Nested If-Else Statement 2

            } //End of Nested If-Statement 1

            //Nested If-Else Statement 3 - That Will 
            if (Vector3.Distance(transform.position, player.position) > enemyViewRadius) {
                m_PlayerInRange = false; //Set the player near to true
            } //End of Nested If-Else Statement 3

            //If-Statement - That Will 
            if (m_PlayerInRange) {
                m_PlayerPosition = player.transform.position; //Set the player position to the player's transform
            } //End of If-Statement

        } //End of For Loop

    } //End of EnvironmentView Method
 */

/*
 * using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//enemyAIConroller Class
public class enemyAIConroller : MonoBehaviour {

    private NavMeshAgent agent; //NavMeshAgent Variable to store the Enemy's NavMeshAgent

    //Variables For Moving Between Waypoints
    [SerializeField] private List<Transform> waypoints; //List Variable to store the waypoints for the enemy to move between
    [SerializeField] private float distanceThreshold = 0.5f; //Float Variable to store the distance threshold for the enemy to move between waypoints
    private int currentWaypointIndex = 0; //Int Variable to store the current waypoint index

    //Variables For Chasing The Player
    [SerializeField] private Transform player; //Transform Variable to store the player's transform
    [SerializeField] private float startChaseRange = 5f; //Float Variable to store the range at which the enemy will start to chase the player
    [SerializeField] private float chaseSpeed = 2f; //Float Variable to store the speed at which the enemy will chase the player
    [SerializeField] private bool isChasing = false; //Bool Variable to store if the enemy is currently chasing the player
    [SerializeField] private float stopChaseRange = 10f; //Float Variable to store the range at which the enemy will stop chasing the player


    //Awake Method - Is Called Before The Start Method
    private void Awake() {

        agent = GetComponent<NavMeshAgent>(); //Get the 'NavMeshAgent' component attached to the enemy

        //If-Statement - That Will Check If The Waypoints List Is Not Empty
        if (waypoints.Count > 0) {
            agent.destination = waypoints[currentWaypointIndex].position; //Set the 'agent.destination' variable to the value of the 'waypoints[currentWaypointIndex].position' variable
        } //End of If-Statement

    } //End of Awake Method

    //Update Method - Is Called Once Per Frame
    private void Update() {

        //If-Statement - That Will Check If The Waypoints List Is Empty
        if (waypoints.Count == 0 || !agent.isOnNavMesh) {
            return;
        } //End of If-Statement

        float playerDistance = Vector3.Distance(transform.position, player.position); //Float variable to store the distance between the enemy and the player

        //If-Statement - That Will 
        if (isChasing) {

            //Nested If-Else Statemnt - That Will
            if (playerDistance > stopChaseRange)
            {
                isChasing = false; //Set the 'isChasing' variable to false
                MoveToNextWaypoint();
            } else {
                agent.SetDestination(player.position); //Set the 'agent.destination' variable to the value of the 'player.position' variable
            }

        } else
        {
            Patrol(); //Call the 'Patrol' Method
        }



    } //End of Update Method

    //Patrol Method - That Will Make The Enemy Patrol The Waypoints
    private void Patrol() {

        if (waypoints.Count == 0 || !agent.isOnNavMesh) {
            return;
        }

        //If-Statement - That Will Check If The 'agent.remainingDistance' variable is less than or equal to the 'distanceThreshold' variable
        if (!agent.pathPending && agent.remainingDistance <= distanceThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;  // Loop to the first waypoint after the last
            MoveToNextWaypoint(); //Call the 'MoveToNextWaypoint' MethodWaypoint
        } //End of If-Statement 

    } //End of Patrol Method

    //MoveToNextWaypoint Method - That Will
    private void MoveToNextWaypoint() {
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    } //End of MoveToNextWaypoint Method

    //OnDrawGizmos Method - Is Called Before The Start Method
    private void OnDrawGizmos()
    {

        //If-Statement - That Will Check If The Waypoints List Is Empty
        if (waypoints.Count == 0)
        {
            return;
        } //End of If-Statement

        Gizmos.color = Color.red; //Set the 'Gizmos.color' variable to red

        //For-Loop -  That Will Loop Through The 'waypoints' List
        for (int i = 0; i < waypoints.Count; i++) {
            Gizmos.DrawSphere(waypoints[i].position, 0.2f); //Draw a sphere at the 'waypoints[i].position' with a radius of 0.32
            //Nested If-Statement - That Will Check If The 'i' variable is greater than 0
            if (i > 0) {
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position); //Draw a line between the 'waypoints[i - 1].position' and the 'waypoints[i].position'
            } //End of Nested If-Statement
        } //End of For-Loop

        //If-Statement - That Will Check 
        if (player != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, startChaseRange); // Chase range
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stopChaseRange); // Return-to-patrol range
        }

    } //End of OnDrawGizmos Method

} //End of enemyAIConroller Class
 */