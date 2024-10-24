using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator; // Ссылка на Animator

    private void Start()
    {
        // Получаем компонент Animator, прикрепленный к объекту
        animator = GetComponent<Animator>();
    }

    // Этот метод вызывается для обновления анимационного состояния в зависимости от скорости движения
    public void UpdateAnimation(float moveInput)
    {
        // Если персонаж движется, включаем состояние "Walking", иначе "Idle"
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            animator.SetBool("Walking", true); // Переход к анимации Walking
        }
        else
        {
            animator.SetBool("Walking", false); // Переход к анимации Idle
        }
    }
}
