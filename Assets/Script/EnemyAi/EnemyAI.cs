using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform shootPoint;
    public float attackCooldown = 2f; // �������� ����� ������� (� ��������)
    private float lastAttackTime = Mathf.NegativeInfinity; // ����� ���������� ��������

    public Transform player; // �����, �� �������� ����� ��������� ����
    public float chaseRange = 10f; // ������, � ������� ���� ����� ������������ ������
    public float attackRange = 2f; // ������ �����

    private NavMeshAgent navMeshAgent; // ����� ��� ������������ �� NavMesh
    private float distanceToPlayer = Mathf.Infinity; // ��������� �� ������
    private bool isProvoked = false; // ����, ����� ���� "�������������" � �������� �������� � ������

    private Animator animator; // ������ �� Animator ��� ���������� ����������

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // �������� ������ �� Animator
    }

    void Update()
    {
        // ������������ ��������� �� ������
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // ���� ���� ��������� � �������� ������� �������������, �� �������� ��������
        if (isProvoked || distanceToPlayer <= chaseRange)
        {
            EngagePlayer();
        }
        else
        {
            // ���� ���� �� ���������� ������, �� � ��������� Idle
            animator.SetBool("IsRunning", false); // ��������� ���
        }
    }

    private void EngagePlayer()
    {
        // ������� ����� � ������� ������
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // ������� �������

        if (distanceToPlayer >= navMeshAgent.stoppingDistance)
        {
            ChasePlayer(); // ������������� ������
        }

        if (distanceToPlayer <= navMeshAgent.stoppingDistance)
        {
            AttackPlayer(); // ����� ������, ���� � ������� �����
        }
    }

    private void ChasePlayer()
    {
        isProvoked = true;
        navMeshAgent.SetDestination(player.position); // ��������� ���� ��� ������ (�����)

        // �������� �������� ����
        animator.SetBool("IsRunning", true);
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown) // ���������, ������ �� ����� �����
        {
            Debug.Log("Attacking the player! Preparing to shoot...");

            Shoot(); // �������� ����� ��������

            lastAttackTime = Time.time; // ��������� ����� ��������� �����

            // ������������� ���, ��� ��� ���� �������
            animator.SetBool("IsRunning", false);
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        // ������� � ������� Raycast
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, attackRange))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // ������� ���� ������, ���� �� ��� �������
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(40); // ������� ���������� �����
                Debug.Log("Player hit and damaged.");
            }
        }

        Vector3 rayEnd = shootPoint.position + shootPoint.forward * attackRange;
        Debug.DrawLine(shootPoint.position, rayEnd, Color.red, 2f);
        Debug.DrawRay(shootPoint.position, shootPoint.forward * attackRange, Color.red, 2f); // ������ ���
        Debug.DrawLine(rayEnd - Vector3.up * 0.1f, rayEnd + Vector3.up * 0.1f, Color.red, 2f); // �������������� �����
        Debug.DrawLine(rayEnd - Vector3.right * 0.1f, rayEnd + Vector3.right * 0.1f, Color.red, 2f); // ������������ �����
    }

    // ���� ����� ������������� ������ ������������� � ����� ��� �������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
