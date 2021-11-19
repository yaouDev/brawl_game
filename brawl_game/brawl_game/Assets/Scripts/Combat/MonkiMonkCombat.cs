using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MonkiMonkCombat : PlayerCombat
{
    //make him not big ondeath
    //add visual timer for big duration?

    [Header("Pray")]
    public float prayTime = 3f;
    public Image bigVisualTimer;

    [Header("Pray-Big")]
    public float bigTime = 6f;
    private float bigTimer;
    public Vector3 bigScale = new Vector3(35f, 35f);
    public AnimationCurve bigCurve;

    [Header("Read Only")]
    public bool isPraying;
    public bool isBig;
    [SerializeField] private Vector3 originalScale;

    private Rigidbody2D rb;
    private MonkiMonkMovement movement;

    private void Start()
    {

        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<MonkiMonkMovement>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(bigTimer > 0f)
        {
            bigTimer -= Time.fixedDeltaTime;
        }
        else if(!isPraying && isBig)
        {
            TurnSmol();
        }
    }

    public override void SpecialAttack(InputAction.CallbackContext ctx)
    {
        if(CanAttack(ctx, AttackType.special) && !isPraying)
        {
            StartCoroutine(Pray());
        }
    }

    public IEnumerator Pray()
    {
        isPraying = true;
        rb.velocity = Vector2.zero;
        movement.StopMoving();
        bigVisualTimer.enabled = true;

        float ratio = 0f;

        float end = Time.time + prayTime;
        while(Time.time < end)
        {
            ratio += Time.deltaTime / prayTime;
            bigVisualTimer.fillAmount = ratio;
            float evaluation = bigCurve.Evaluate(ratio);
            transform.localScale = Vector3.Lerp(originalScale, bigScale, evaluation);
            yield return null;
        }

        bigVisualTimer.enabled = false;
        isPraying = false;
        bigTimer += bigTime;
        TurnBig();
    }

    private void TurnBig()
    {
        isBig = true;

        //if(transform.localScale != bigScale) transform.localScale = bigScale;
    }

    private void TurnSmol()
    {
        isBig = false;

        //add poof effect
        Debug.Log("Going smol!");

        transform.localScale = originalScale;
    }
}
