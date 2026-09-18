using UnityEngine;

public class ScriptRailHandler : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;

    [Header("Movement")]
    [SerializeField] private float speed = 14f;
    [SerializeField] private float rotationSpeed = 2.5f;
    [SerializeField] private float reachDistance = 2f;

    private int currentIndex = 0;

    public bool IsMoving { get ; set; } = true;

    public bool ReachedEnd { get; private set;}
  
    void Update()
    {
        if (!IsMoving || ReachedEnd || waypoints ==null || waypoints.Length == 0) 
        return;
        
        Transform target = waypoints[currentIndex];
        if (target == null)return;
        
        Vector3 toTarget = target.position - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, 
                                                target.position, 
                                                speed * Time.deltaTime);

        if (toTarget.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                lookRotation,
                                                rotationSpeed * Time.deltaTime);
        }

        if (toTarget.magnitude <= reachDistance)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                ReachedEnd = true;
                IsMoving = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            Gizmos.DrawWireSphere(waypoints[i].position, 1f);

            if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
