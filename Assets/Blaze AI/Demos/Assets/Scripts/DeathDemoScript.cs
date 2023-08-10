using UnityEngine;
using BlazeAISpace;
using BlazeAIDemo;


public class DeathDemoScript : MonoBehaviour
{
    public BlazeAI[] blazeAI;

    void Update()
    {
        // hit the AI
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            for (int i=0; i<blazeAI.Length; i++) {
                blazeAI[i].Hit();
                BlazeAIDemo.Health blazeHealth = blazeAI[i].GetComponent<BlazeAIDemo.Health>();

                if (blazeHealth.currentHealth > 0) {
                    blazeHealth.currentHealth -= 10;
                }

                if (blazeHealth.currentHealth <= 0) {
                    if (i < 2) {
                        // plays either death animation or ragdolls instantly depending on inspector
                        blazeAI[i].Death();
                    }
                    else {
                        // plays death animation and then ragdolls midway
                        blazeAI[i].DeathDoll(0.5f);
                    }
                }
            }
        }


        // return alive
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            for (int i=0; i<blazeAI.Length; i++) {
                BlazeAIDemo.Health blazeHealth = blazeAI[i].GetComponent<BlazeAIDemo.Health>();
                blazeHealth.currentHealth = blazeHealth.maxHealth;
                blazeAI[i].ChangeState("normal");
            }
        }
    }
}
