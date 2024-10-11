using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    
    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameRoot.Instance.Is2D)
        {
            HandleMovement();
        }
        
    }

    void HandleMovement()
    {
        Vector2 moveInput =  new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")); 

        rb2D.MovePosition(rb2D.position + moveInput * MoveSpeed * Time.deltaTime);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);

    }


}
