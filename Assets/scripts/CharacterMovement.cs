using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseCharacter : MonoBehaviour
{
    public GameObject MissionCompleted;
    bool MissionCompletedTriggered = false;

    public Transform player; // reference to the player's transform
    public Animator animator; // reference to the enemy's Animator component
    public GameObject GameOverWindow;
    public float moveSpeed = 5f; // the enemy's move speed
    public float rotationSpeed = 5f; // the speed at which the enemy rotates
    public float chaseRange = 10f; // the distance at which the enemy starts chasing the player
    public float deathRange = .75f; // the distance at which the enemy kills the player

    private void Update()
    {
        if (MissionCompleted.activeSelf)
            MissionCompletedTriggered = true;
        // flatten Y to avoid height affecting distance
        Vector3 flatPlayerPos = new Vector3(player.position.x, 0f, player.position.z);
        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);
        float distance = Vector3.Distance(flatPlayerPos, flatEnemyPos);

        ChasePlayer(distance); // method that holds the logic for enemy to chase player
        PlayerDeath(distance); // method that reloads the level when enemy catches player
    }

    private void PlayerDeath(float distance)
    {
        if (MissionCompletedTriggered)
            return;
        // if the distance is close enough to the player it reloads the scene
            if (distance < deathRange)
            {
                GameOverWindow.SetActive(true);
                return;
            }
    }

    private void ChasePlayer(float distance)
    {
        // if the distance is less than a certain threshold, move towards the player
        if (distance < chaseRange)
        {
            // calculate the direction towards the player, but ignore Y to prevent vertical floating
            Vector3 direction = player.position - transform.position;
            direction.y = 0f; // ignore vertical difference
            direction = direction.normalized;

            // move the enemy towards the player on XZ plane only
            transform.position += direction * moveSpeed * Time.deltaTime;

            // calculate the rotation towards the player on XZ plane
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // smoothly rotate towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // if the distance is less than a certain threshold, animate the enemy as running
            animator.SetBool("isRunning", true);
        }
        else
        {
            // if the distance is more than a certain threshold, animate the enemy as idle
            animator.SetBool("isRunning", false);
        }
    }
}
