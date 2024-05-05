using UnityEngine;
using UnityEngine.UI;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private Animator animator;
    private bool isShieldActive;
   

    // Don't delete
    //[SerializeField] private ParticleSystem sparkle;
    //[SerializeField] private ParticleSystem blood;

    private void OnTriggerEnter(Collider other)
    {
        if (animator.isActiveAndEnabled ==true)
        {
            isShieldActive = animator.GetBehaviour<BlockStateA>().Blocking();
        }
      
        if (other.gameObject.CompareTag("Enemy") && !isShieldActive)
        {
            enemyHealthBar.GetComponent<Slider>().value -=20;

            // play hit animation 

            // blood.Play();
        }

        if (other.gameObject.CompareTag("Shield") && isShieldActive)
        {
            // Don't delete
            // sparkle.Play();

            // Don't delete
            //play block animation
        }
    }
}
