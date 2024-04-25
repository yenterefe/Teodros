using UnityEngine;
using UnityEngine.UI;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private Animator animator;
    private bool shieldActive;
   

    // Don't delete
    //[SerializeField] private ParticleSystem sparkle;
    //[SerializeField] private ParticleSystem blood;

    private void OnTriggerEnter(Collider other)
    {
        if (animator.isActiveAndEnabled ==true)
        {
            shieldActive = animator.GetBehaviour<BlockStateA>().Blocking();
        }
      
        if (other.gameObject.CompareTag("Enemy") && shieldActive == false)
        {
            enemyHealthBar.GetComponent<Slider>().value -=20;

            // play hit animation 

            // blood.Play();
        }

        if (other.gameObject.CompareTag("Shield") && shieldActive ==true)
        {
            // Don't delete
            // sparkle.Play();

            // Don't delete
            //play block animation
        }
    }
}
