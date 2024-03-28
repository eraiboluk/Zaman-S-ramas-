using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float acc = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    public bool isMoving = true;
    public bool canMove = true;
    public bool AR;
    public string curpos;
    public string prepos= "";

    public bool isRunning = false;
    private Animator animator;
    public Terrain t;

    public float x;
    public float y;
    public float z;
    public Vector3 ogPosition;

    public Vector3 forward;
    public Vector3 right;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        t = GetComponent<Terrain>();
    }

    void Update()
    {
        var player= GameObject.Find("Player");
        ogPosition = player.transform.position;
        curpos = GetLayerName(ogPosition, t);
        
        if (characterController.velocity.x != 0 && animator.GetBool("isGrounded"))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        if(!isMoving){    //durma
            animator.SetInteger("Speed", 0);
            FindObjectOfType<AudioManager>().Toggle(curpos+"Walk",0);
            FindObjectOfType<AudioManager>().Toggle(curpos+"Run",0);
            FindObjectOfType<AudioManager>().Toggle(prepos+"Walk",0);
            FindObjectOfType<AudioManager>().Toggle(prepos+"Run",0);
            prepos="";
        }
        else if(!isRunning)//yurume
        {
            animator.SetInteger("Speed", 6);
            
            if (!FindObjectOfType<AudioManager>().isPlay(curpos + "Walk")){
                FindObjectOfType<AudioManager>().Toggle(curpos+"Run",0);
                FindObjectOfType<AudioManager>().Toggle(prepos+"Run",0);
                FindObjectOfType<AudioManager>().Toggle(prepos+"Walk",0);
                FindObjectOfType<AudioManager>().Toggle(curpos+"Walk",1);
            }
            else if(!(curpos==prepos)){
                FindObjectOfType<AudioManager>().Toggle(prepos+"Walk",0);
                FindObjectOfType<AudioManager>().Toggle(curpos+"Run",0);//can be removed
                FindObjectOfType<AudioManager>().Toggle(prepos+"Run",0);//can be removed
                FindObjectOfType<AudioManager>().Toggle(curpos+"Walk",1);
            }
        }
        else if(isRunning){    //kosma

            animator.SetInteger("Speed", 12);

            if (!FindObjectOfType<AudioManager>().isPlay(curpos + "Run")){
                FindObjectOfType<AudioManager>().Toggle(prepos+"Run",0);
                FindObjectOfType<AudioManager>().Toggle(curpos+"Walk",0);
                FindObjectOfType<AudioManager>().Toggle(prepos+"Walk",0);
                FindObjectOfType<AudioManager>().Toggle(curpos+"Run",1);
            }
            else if(!(curpos==prepos)){
                FindObjectOfType<AudioManager>().Toggle(prepos+"Run",0);      
                FindObjectOfType<AudioManager>().Toggle(curpos+"Walk",0);//can be removed
                FindObjectOfType<AudioManager>().Toggle(prepos+"Walk",0);//can be removed
                FindObjectOfType<AudioManager>().Toggle(curpos+"Run",1);
            }   
        }
        
        prepos=curpos;
        
        forward = transform.TransformDirection(Vector3.forward);
        right = transform.TransformDirection(Vector3.right);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            animator.SetBool("isJumping", true);
            moveDirection.y = jumpPower;
        }
        else
        {
            animator.SetBool("isJumping", false);
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            animator.SetBool("isGrounded", false);
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            //walkSpeed = 6f;
            //runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    
        x=characterController.velocity.x;
        y=characterController.velocity.y;
        z=characterController.velocity.z;
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        
        if((forward.x*x) > 0 && (z*forward.z) > 0){
            animator.SetInteger("Direction", 0);
        }
        else if((forward.x*x) < 0 && (z*forward.z) < 0){
            animator.SetInteger("Direction", 1);
        }
        else if((forward.z*x) < 0 && (z*forward.x) > 0){
            animator.SetInteger("Direction", 3);
        }
        else if((forward.z*x) > 0 && (z*forward.x) < 0){
            animator.SetInteger("Direction", 2);
        }

        if (characterController.isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
    }
    
    
    private float[] GetTextureMix(Vector3 PlayerPos, Terrain t){
        Vector3 tPos = t.transform.position;
        TerrainData tData = t.terrainData;
        
        int mapX = Mathf.RoundToInt((PlayerPos.x) / tData.size.x * tData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((PlayerPos.z) / tData.size.z * tData.alphamapHeight);
        float[,,] splatMapData = tData.GetAlphamaps(mapX,mapZ, 1, 1);
        
        float[] cellmix = new float[splatMapData.GetUpperBound(2) + 1];
        for(int i = 0;  i < cellmix.Length; i++){
            cellmix[i] = splatMapData[0,0,i];
        }
        
        return cellmix;
    }
    
    public string GetLayerName(Vector3 PlayerPos, Terrain t){
        float[] cellmix = GetTextureMix(PlayerPos, t);
        float strongest = 0;
        int maxIndex = 0;
        for(int i = 0; i < cellmix.Length; i++){
            if(cellmix[i] > strongest){
                maxIndex = i;
                strongest = cellmix[i];
            }
        }
        return t.terrainData.terrainLayers[maxIndex].name;
    }
}

