using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static int playerHP = 100;
    public static bool isGameOver;
    public TextMeshProUGUI playerHPText;
    [SerializeField] private GameObject hurt;
    public Animator blackAnim;
    public Animator charAnimDead;
    //sound and particle effect
    

    void Start()
    {
        //hurt = GameObject.Find("hurt");
        isGameOver = false;
        playerHP = 100;
    }

    void Update()
    {
        playerHPText.text = "+" + playerHP;
        if (isGameOver)
        {
            charAnimDead.SetTrigger("Dead");
            blackAnim.SetTrigger("FadeOut");
            StartCoroutine(FadeToScene());
        }
    }

    public void GainHealth(int healthAmount)
    {
        playerHP += healthAmount;
        Debug.Log("Is Healthing");
    }


    public IEnumerator TakeDamage(int damageAmount)
    {
        //Debug.Log("Is Taking Damage");
        hurt.SetActive(true);
        //charAnimHurt.SetBool("Hurt", true);
        //boom.SetActive(true);

        //explosion.SetActive(true);

        playerHP -= damageAmount;
        if (playerHP <= 0)
        {
            isGameOver = true;
        }

        yield return new WaitForSeconds(0.5f);
        hurt.SetActive(false);
        //boom.SetActive(false);

        //charAnimHurt.SetBool("Hurt", false);


        yield return new WaitForSeconds(1f);
        //explosion.SetActive(false);
    }


    IEnumerator FadeToScene()
    {
        yield return new WaitForSeconds(3.20f);
        SceneManager.LoadScene("YouLoose");
        Debug.Log("perdite guey");
    }
}
