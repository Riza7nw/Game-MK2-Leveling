using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 checkpointPosition;

    void Start()
    {
        // Posisi awal player jadi checkpoint pertama
        checkpointPosition = transform.position;
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        checkpointPosition = newCheckpoint;
    }

    public void Respawn()
    {
        transform.position = checkpointPosition;
        // reset health, animasi, dll kalau perlu
    }
}
