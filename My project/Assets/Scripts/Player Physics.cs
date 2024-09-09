using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhysics : MonoBehaviour
{
    // Declaring Variables and referencing Rigidbody for player phyisics
    public Rigidbody2D rb;
    Vector2 speed;
    [SerializeField] public float speedForce;
    [SerializeField] public float dashForce;
    bool dash = false;

    // Declaring Variables for Stamina
    public Image staminaBar;
    public float stamina, maxStamina;
    public float dashCost;
    public float chargeRate;
    private Coroutine recharge;
    
    // Awake is called on the game loading.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame which is inconsistent, so I register if the game is currently in play, if it is I register their inputs if it isnt then I set their speed to be 0 stopping their movement.
    private void Update()
    {
        if(GameController.instance.gamePlaying)
        {
            speed.x = (Input.GetAxisRaw("Horizontal"));
            speed.y = (Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                dash = true;
            }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    dash = false;
                }
            if (stamina <= 0) dash = false;
        }
        else
        {
            speed.x = 0;
            speed.y = 0;
        }
    }

    // FixedUpdate is called once per tick which is consistent, so I do the calculations here. 
    // Firstly I Identify if the user is currently using the dash ability, they are I amplify their movement speed and deduct stamina, I also ensure that stamina fall below 0 and that the stamina wont recharge during the dash.
    private void FixedUpdate()
    {
        if (dash == true)
        {
            rb.MovePosition(rb.position + speed.normalized * dashForce * speedForce * Time.fixedDeltaTime);
            stamina -= dashCost * Time.fixedDeltaTime;
            if(stamina < 0) 
            {
                stamina = 0;
            }
            staminaBar.fillAmount = stamina / maxStamina;
            if(recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
        else
        {
        rb.MovePosition(rb.position + speed.normalized * speedForce * Time.fixedDeltaTime);
        }
    }
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while(stamina < maxStamina) 
        {
            stamina += chargeRate / 10f;
            if (stamina > maxStamina) 
            {
            stamina = maxStamina;
            }
            staminaBar.fillAmount = stamina / maxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }

}
