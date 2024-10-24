using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения
    public float sprintMultiplier = 2f; // Множитель скорости при беге
    private float currentSpeed = 5f; // Текущая скорость (обычная или бег)

    public float jumpForce = 5f; // Сила прыжка
    private Vector2 moveInput; // Вектор для хранения ввода
    private PlayerControls controls; // Ссылка на InputActionAsset
    private InputAction moveAction; // Ссылка на конкретное действие Move
    private InputAction jumpAction; // Ссылка на действие Jump
    private InputAction shootAction; // Ссылка на действие Jump
    private InputAction sprintAction; // Ссылка на действие Sprint
    private InputAction switchWeaponAction; // Для смены оружия

    private Rigidbody rb; // Ссылка на Rigidbody
    private bool isGrounded; // Проверка, на земле ли игрок


    // public GameObject bulletPrefab;
    public Transform shootPoint; // Точка, откуда будет производиться стрельба
    public float maxRange = 2f; // Максимальная дистанция стрельбы

    public WeaponManager weaponManager;
    private bool isShooting = false;
    private float nextFireTime = 0f;

    private Animator animator;

    private void Awake()
    {
        controls = new PlayerControls(); // Инициализация вашего InputActionAsset
        moveAction = controls.BaseCombat.Move; // Получаем действие Move
        jumpAction = controls.BaseCombat.Jump; // Получаем действие Jump
        shootAction = controls.BaseCombat.Shoot;
        sprintAction = controls.BaseCombat.Sprint;
        switchWeaponAction = controls.BaseCombat.SwitchWeapon;

        rb = GetComponent<Rigidbody>(); // Получаем компонент Rigidbody
        animator = GetComponent<Animator>();

        // Инициализация WeaponManager, если не присвоено в инспекторе
        if (weaponManager == null)
        {
            weaponManager = GetComponent<WeaponManager>(); // Если WeaponManager на том же объекте
        }
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        jumpAction.performed += OnJump; // Подписываемся на событие прыжка
        shootAction.performed += OnShoot;
        shootAction.canceled += OnShoot; // Добавлено: отслеживание отпускания кнопки
        sprintAction.performed += OnSprintStart; // Обработчик нажатия на бег
        sprintAction.canceled += OnSprintStop; // Обработчик отпускания бега
        switchWeaponAction.performed += OnSwitchWeapon;

        controls.BaseCombat.Enable(); // Включение мапы
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        jumpAction.performed -= OnJump; // Отписываемся от события прыжка
        shootAction.performed -= OnShoot;
        shootAction.canceled -= OnShoot; // Отписывание от события отпускания
        sprintAction.performed -= OnSprintStart;
        sprintAction.canceled -= OnSprintStop;
        switchWeaponAction.performed -= OnSwitchWeapon;

        controls.BaseCombat.Disable(); // Отключение мапы
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        bool isWalking = moveInput.magnitude > 0.1f; // Проверка на наличие движения
        animator.SetBool("Walking", isWalking); // Установка состояния анимации

        Debug.Log("Miskare" + isWalking);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded) // Проверяем, на земле ли игрок
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Применяем силу прыжка
        isGrounded = false; // После прыжка игрок уже не на земле
    }

    private void OnSprintStart(InputAction.CallbackContext context)
    {
        currentSpeed = moveSpeed * sprintMultiplier; // Увеличиваем скорость при беге
    }

    private void OnSprintStop(InputAction.CallbackContext context)
    {
        currentSpeed = moveSpeed; // Возвращаем обычную скорость, когда игрок отпускает Shift
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        // Получаем значение прокрутки колесика мыши
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (scrollValue.y > 0)
        {
            weaponManager.SwitchWeapon(1); // Переключаем на следующее оружие
        }
        else if (scrollValue.y < 0)
        {
            weaponManager.SwitchWeapon(-1); // Переключаем на предыдущее оружие
        }
    }



    private void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * currentSpeed * Time.deltaTime;
        transform.Translate(move);


        

        CheckGrounded(); // Проверка, находится ли игрок на земле
        UpdateAmmoText(); // Обновляем текст патронов

        {
            if (weaponManager == null)
            {
                Debug.LogError("WeaponManager не инициализирован!");
                return;
            }

            Weapon currentWeapon = weaponManager.GetCurrentWeapon();

            if (currentWeapon != null && currentWeapon.isAutomatic && isShooting && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + currentWeapon.fireRate;
            }
        }

    }

    private void UpdateAnimation()
    {
        bool isWalking = moveInput.magnitude > 0.1f;
        animator.SetBool("Walking", isWalking); // Переключение анимации
    }

    // Метод для проверки приземления с помощью Raycast
    private void CheckGrounded()
    {
        // Используем Raycast для проверки земли на расстоянии немного больше высоты игрока
        RaycastHit hit;
        float distanceToGround = 1.2f; // Увеличим расстояние для проверки
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround))
        {
            // Дополнительная проверка на наличие тега "Ground" у объекта, с которым сталкиваемся
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false; // Если не нашли землю, то не на земле
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        Weapon currentWeapon = weaponManager.GetCurrentWeapon(); // Получаем текущее оружие

        if (currentWeapon != null)
        {
            if (currentWeapon.isAutomatic) // Если оружие автоматическое
            {
                if (context.performed) // Начало зажатия
                {
                    isShooting = true;
                }
                if (context.canceled) // Прекращение зажатия
                {
                    isShooting = false;
                }


            }
            else // Если оружие с одиночным выстрелом
            {
                if (context.performed) // Проверка одиночного нажатия
                {
                    Shoot();
                }
            }
        }
    }


    private void Shoot()
    {
        Weapon currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon == null) return;

        // Проверка на наличие патронов
        if (currentWeapon.currentBullets <= 0)
        {
            // Если патронов нет, перезаряжаем
            currentWeapon.Reload();
            return; // Прекращаем выполнение метода
        }

        // Логика выстрела
        if (currentWeapon.Shoot()) // Если выстрел был успешным
        {
            // Активируем Particle System (если есть)
            if (currentWeapon.muzzleFlash != null)
            {
                ParticleSystem muzzleFlashInstance = Instantiate(currentWeapon.muzzleFlash, shootPoint.position, shootPoint.rotation, shootPoint);
                muzzleFlashInstance.Play();
            }

            RaycastHit hit;

            // Выстрел с помощью Raycast
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, maxRange))
            {
                Debug.Log("Hit: " + hit.collider.name);

                // Наносим урон врагу, если он был поражен
                EnemyHP enemy = hit.collider.GetComponent<EnemyHP>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentWeapon.damage);
                    Debug.Log("Enemy hit and damaged.");
                }
            }

            // Визуализация Raycast
            Vector3 rayEnd = shootPoint.position + shootPoint.forward * maxRange;
            Debug.DrawLine(shootPoint.position, rayEnd, Color.red, 2f);
            Debug.DrawRay(shootPoint.position, shootPoint.forward * maxRange, Color.red, 2f); // Рисует луч
            Debug.DrawLine(rayEnd - Vector3.up * 0.1f, rayEnd + Vector3.up * 0.1f, Color.red, 2f); // Горизонтальная линия
            Debug.DrawLine(rayEnd - Vector3.right * 0.1f, rayEnd + Vector3.right * 0.1f, Color.red, 2f); // Вертикальная линия
        }
    }

    public TMP_Text ammoText; // Ссылка на UI TextMeshPro компонент
    public Image uiImage;

    private void UpdateAmmoText()
    {
        Weapon currentWeapon = weaponManager.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            ammoText.text = $"Patrons: {currentWeapon.currentBullets}/{currentWeapon.totalBullets}";
        }
        if (currentWeapon.imageSprite != null)
        {
            uiImage.sprite = currentWeapon.imageSprite;
        }
    }




}