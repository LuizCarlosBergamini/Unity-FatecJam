using UnityEngine;

public class NoteMovement : MonoBehaviour {
    public NoteData noteData { get; private set; }

    private Vector3 spawnPoint; // Top of the lane
    private Vector3 targetPoint;  // Judgment line

    private float travelDuration;

    // TODO: Fix this shit code
    private Vector3 lane1Spawn = new Vector3((float)9.81, (float)1.96, (float)0.06111138);
    private Vector3 lane2Spawn = new Vector3((float)9.81, (float)0.91, (float)0.06111138);
    private Vector3 lane3Spawn = new Vector3((float)9.81, (float)-0.08, (float)0.06111138);
    private Vector3 lane4Spawn = new Vector3((float)9.81, (float)-1.1, (float)0.06111138);
    private Vector3 lane5Spawn = new Vector3((float)9.81, (float)-2.06, (float)0.06111138);

    private Vector3 lane1Target = new Vector3((float)-10.03, (float)1.96, (float)0.06111138);
    private Vector3 lane2Target = new Vector3((float)-10.03, (float)0.91, (float)0.06111138);
    private Vector3 lane3Target = new Vector3((float)-10.03, (float)-0.08, (float)0.06111138);
    private Vector3 lane4Target = new Vector3((float)-10.03, (float)-1.1, (float)0.06111138);
    private Vector3 lane5Target = new Vector3((float)-10.03, (float)-2.06, (float)0.06111138);
    public void Initialize(NoteData data, float lookAheadTime)
    {
        this.noteData = data;

        // TODO: Fix this shit code (maybe with a LaneManager)
        switch (data.laneIndex)
        {
            case 0:
                spawnPoint = lane1Spawn;
                targetPoint = lane1Target;
                break;
            case 1:
                spawnPoint = lane2Spawn;
                targetPoint = lane2Target;
                break;
            case 2:
                spawnPoint = lane3Spawn;
                targetPoint = lane3Target;
                break;
            case 3:
                spawnPoint = lane4Spawn;
                targetPoint = lane4Target;
                break;
            case 4:
                spawnPoint = lane5Spawn;
                targetPoint = lane5Target;
                break;


        }

        // The duration of travel is the lookahead time used by the spawner
        travelDuration = lookAheadTime;
    }

    void Update()
    {
        if (this.noteData != null)
        {
            float songTime = Conductor.instance.songPosition;
            float secPerBeat = Conductor.instance.secPerBeat;

            double noteTargetTime = noteData.timestamp * secPerBeat;
            double timeToTarget = noteTargetTime - songTime;

            // Calculate interpolation factor (0 = at spawn, 1 = at target)
            float lerpT = 1f - ((float)timeToTarget / travelDuration);

            // Update position
            transform.position = Vector3.Lerp(spawnPoint, targetPoint, lerpT);

            // Despawn logic (if missed)
            if (lerpT > 1.1f) // A small buffer past the judgment line
            {
                // Notify game systems of a miss
                Debug.Log("Missed Note");
                Destroy(gameObject);
            }
        }
    }
}