using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    Animator anim;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float rotSpeed = 10f;
    [SerializeField]
    float gravity = -9.81f;
    [SerializeField]
    float jumpPower = 10;

    float yVelocity = 0;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>(); 
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = h * Camera.main.transform.right + v * Camera.main.transform.forward;
        dir.y = 0;
        dir.Normalize();

        if (dir.magnitude > 0.1f)
        {
            anim.SetBool("IsMove", true);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }
        else
            anim.SetBool("IsMove", false);

        cc.Move(speed * dir * Time.deltaTime);

        if (IsGround())
        {
            anim.SetBool("Falling", false);
            yVelocity = 0;
        }
        else
        {
            anim.SetBool("Falling", true);
            yVelocity += gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGround())
        {
            anim.SetTrigger("Jump");
            yVelocity = jumpPower;
        }

        cc.Move(yVelocity * Vector3.up * Time.deltaTime);
    }

    bool IsGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.05f)) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}