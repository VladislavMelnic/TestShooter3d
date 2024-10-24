using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� ��������
    public float sprintMultiplier = 2f; // ��������� �������� ��� ����
    private float currentSpeed = 5f; // ������� �������� (������� ��� ���)

    public float jumpForce = 5f; // ���� ������
    private Vector2 moveInput; // ������ ��� �������� �����
    private PlayerControls controls; // ������ �� InputActionAsset
    private InputAction moveAction; // ������ �� ���������� �������� Move
    private InputAction jumpAction; // ������ �� �������� Jump
    private InputAction shootAction; // ������ �� �������� Jump
    private InputAction sprintAction; // ������ �� �������� Sprint
    private InputAction switchWeaponAction; // ��� ����� ������

    private Rigidbody rb; // ������ �� Rigidbody
    private bool isGrounded; // ��������, �� ����� �� �����


    // public GameObject bulletPrefab;
    public Transform shootPoint; // �����, ������ ����� ������������� ��������
    public float maxRange = 2f; // ������������ ��������� ��������

    public WeaponManager weaponManager;
    private bool isShooting = false;
    private float nextFireTime = 0f;

    private Animator animator;

    private void Awake()
    {
        controls = new PlayerControls(); // ������������� ������ InputActionAsset
        moveAction = controls.BaseCombat.Move; // �������� �������� Move
        jumpAction = controls.BaseCombat.Jump; // �������� �������� Jump
        shootAction = controls.BaseCombat.Shoot;
        sprintAction = controls.BaseCombat.Sprint;
        switchWeaponAction = controls.BaseCombat.SwitchWeapon;

        rb = GetComponent<Rigidbody>(); // �������� ��������� Rigidbody
        animator = GetComponent<Animator>();

        // ������������� WeaponManager, ���� �� ��������� � ����������
        if (weaponManager == null)
        {
            weaponManager = GetComponent<WeaponManager>(); // ���� WeaponManager �� ��� �� �������
        }
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        jumpAction.performed += OnJump; // ������������� �� ������� ������
        shootAction.performed += OnShoot;
        shootAction.canceled += OnShoot; // ���������: ������������ ���������� ������
        sprintAction.performed += OnSprintStart; // ���������� ������� �� ���
        sprintAction.canceled += OnSprintStop; // ���������� ���������� ����
        switchWeaponAction.performed += OnSwitchWeapon;

        controls.BaseCombat.Enable(); // ��������� ����
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        jumpAction.performed -= OnJump; // ������������ �� ������� ������
        shootAction.performed -= OnShoot;
        shootAction.canceled -= OnShoot; // ����������� �� ������� ����������
        sprintAction.performed -= OnSprintStart;
        sprintAction.canceled -= OnSprintStop;
        switchWeaponAction.performed -= OnSwitchWeapon;

        controls.BaseCombat.Disable(); // ���������� ����
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        bool isWalking = moveInput.magnitude > 0.1f; // �������� �� ������� ��������
        animator.SetBool("Walking", isWalking); // ��������� ��������� ��������

        Debug.Log("Miskare" + isWalking);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded) // ���������, �� ����� �� �����
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // ��������� ���� ������
        isGrounded = false; // ����� ������ ����� ��� �� �� �����
    }

    private void OnSprintStart(InputAction.CallbackContext context)
    {
        currentSpeed = moveSpeed * sprintMultiplier; // ����������� �������� ��� ����
    }

    private void OnSprintStop(InputAction.CallbackContext context)
    {
        currentSpeed = moveSpeed; // ���������� ������� ��������, ����� ����� ��������� Shift
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        // �������� �������� ��������� �������� ����
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (scrollValue.y > 0)
        {
            weaponManager.SwitchWeapon(1); // ����������� �� ��������� ������
        }
        else if (scrollValue.y < 0)
        {
            weaponManager.SwitchWeapon(-1); // ����������� �� ���������� ������
        }
    }



    private void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * currentSpeed * Time.deltaTime;
        transform.Translate(move);


        

        CheckGrounded(); // ��������, ��������� �� ����� �� �����
        UpdateAmmoText(); // ��������� ����� ��������

        {
            if (weaponManager == null)
            {
                Debug.LogError("WeaponManager �� ���������������!");
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
        animator.SetBool("Walking", isWalking); // ������������ ��������
    }

    // ����� ��� �������� ����������� � ������� Raycast
    private void CheckGrounded()
    {
        // ���������� Raycast ��� �������� ����� �� ���������� ������� ������ ������ ������
        RaycastHit hit;
        float distanceToGround = 1.2f; // �������� ���������� ��� ��������
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround))
        {
            // �������������� �������� �� ������� ���� "Ground" � �������, � ������� ������������
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false; // ���� �� ����� �����, �� �� �� �����
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        Weapon currentWeapon = weaponManager.GetCurrentWeapon(); // �������� ������� ������

        if (currentWeapon != null)
        {
            if (currentWeapon.isAutomatic) // ���� ������ ��������������
            {
                if (context.performed) // ������ �������
                {
                    isShooting = true;
                }
                if (context.canceled) // ����������� �������
                {
                    isShooting = false;
                }


            }
            else // ���� ������ � ��������� ���������
            {
                if (context.performed) // �������� ���������� �������
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

        // �������� �� ������� ��������
        if (currentWeapon.currentBullets <= 0)
        {
            // ���� �������� ���, ������������
            currentWeapon.Reload();
            return; // ���������� ���������� ������
        }

        // ������ ��������
        if (currentWeapon.Shoot()) // ���� ������� ��� ��������
        {
            // ���������� Particle System (���� ����)
            if (currentWeapon.muzzleFlash != null)
            {
                ParticleSystem muzzleFlashInstance = Instantiate(currentWeapon.muzzleFlash, shootPoint.position, shootPoint.rotation, shootPoint);
                muzzleFlashInstance.Play();
            }

            RaycastHit hit;

            // ������� � ������� Raycast
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, maxRange))
            {
                Debug.Log("Hit: " + hit.collider.name);

                // ������� ���� �����, ���� �� ��� �������
                EnemyHP enemy = hit.collider.GetComponent<EnemyHP>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentWeapon.damage);
                    Debug.Log("Enemy hit and damaged.");
                }
            }

            // ������������ Raycast
            Vector3 rayEnd = shootPoint.position + shootPoint.forward * maxRange;
            Debug.DrawLine(shootPoint.position, rayEnd, Color.red, 2f);
            Debug.DrawRay(shootPoint.position, shootPoint.forward * maxRange, Color.red, 2f); // ������ ���
            Debug.DrawLine(rayEnd - Vector3.up * 0.1f, rayEnd + Vector3.up * 0.1f, Color.red, 2f); // �������������� �����
            Debug.DrawLine(rayEnd - Vector3.right * 0.1f, rayEnd + Vector3.right * 0.1f, Color.red, 2f); // ������������ �����
        }
    }

    public TMP_Text ammoText; // ������ �� UI TextMeshPro ���������
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