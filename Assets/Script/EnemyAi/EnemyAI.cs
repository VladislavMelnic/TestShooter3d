using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform shootPoint;
    public float attackCooldown = 2f; // Задержка между атаками (в секундах)
    private float lastAttackTime = Mathf.NegativeInfinity; // Время последнего выстрела

    public Transform player; // Игрок, на которого будет охотиться враг
    public float chaseRange = 10f; // Радиус, в котором враг будет преследовать игрока
    public float attackRange = 2f; // Радиус атаки

    private NavMeshAgent navMeshAgent; // Агент для передвижения по NavMesh
    private float distanceToPlayer = Mathf.Infinity; // Дистанция до игрока
    private bool isProvoked = false; // Флаг, когда враг "спровоцирован" и начинает движение к игроку

    private Animator animator; // Ссылка на Animator для управления анимациями

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Получаем ссылку на Animator
    }

    void Update()
    {
        // Рассчитываем дистанцию до игрока
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Если враг находится в пределах радиуса преследования, он начинает движение
        if (isProvoked || distanceToPlayer <= chaseRange)
        {
            EngagePlayer();
        }
        else
        {
            // Если враг не преследует игрока, он в состоянии Idle
            animator.SetBool("IsRunning", false); // Отключаем бег
        }
    }

    private void EngagePlayer()
    {
        // Поворот врага в сторону игрока
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // Плавный поворот

        if (distanceToPlayer >= navMeshAgent.stoppingDistance)
        {
            ChasePlayer(); // Преследование игрока
        }

        if (distanceToPlayer <= navMeshAgent.stoppingDistance)
        {
            AttackPlayer(); // Атака игрока, если в радиусе атаки
        }
    }

    private void ChasePlayer()
    {
        isProvoked = true;
        navMeshAgent.SetDestination(player.position); // Установка цели для агента (игрок)

        // Включаем анимацию бега
        animator.SetBool("IsRunning", true);
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown) // Проверяем, прошло ли время атаки
        {
            Debug.Log("Attacking the player! Preparing to shoot...");

            Shoot(); // Вызываем метод стрельбы

            lastAttackTime = Time.time; // Обновляем время последней атаки

            // Останавливаем бег, так как враг атакует
            animator.SetBool("IsRunning", false);
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        // Выстрел с помощью Raycast
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, attackRange))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Наносим урон игроку, если он был поражен
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(40); // Укажите количество урона
                Debug.Log("Player hit and damaged.");
            }
        }

        Vector3 rayEnd = shootPoint.position + shootPoint.forward * attackRange;
        Debug.DrawLine(shootPoint.position, rayEnd, Color.red, 2f);
        Debug.DrawRay(shootPoint.position, shootPoint.forward * attackRange, Color.red, 2f); // Рисует луч
        Debug.DrawLine(rayEnd - Vector3.up * 0.1f, rayEnd + Vector3.up * 0.1f, Color.red, 2f); // Горизонтальная линия
        Debug.DrawLine(rayEnd - Vector3.right * 0.1f, rayEnd + Vector3.right * 0.1f, Color.red, 2f); // Вертикальная линия
    }

    // Этот метод визуализирует радиус преследования в сцене для отладки
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
