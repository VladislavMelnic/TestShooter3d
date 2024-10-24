using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator; // ������ �� Animator

    private void Start()
    {
        // �������� ��������� Animator, ������������� � �������
        animator = GetComponent<Animator>();
    }

    // ���� ����� ���������� ��� ���������� ������������� ��������� � ����������� �� �������� ��������
    public void UpdateAnimation(float moveInput)
    {
        // ���� �������� ��������, �������� ��������� "Walking", ����� "Idle"
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            animator.SetBool("Walking", true); // ������� � �������� Walking
        }
        else
        {
            animator.SetBool("Walking", false); // ������� � �������� Idle
        }
    }
}
