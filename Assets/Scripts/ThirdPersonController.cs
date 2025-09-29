using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public RatAnimator ratAnimator;
    public Transform camera;
    public Transform itemSpawnPoint;
    public Transform dropSpawnPoint;

    public Text displayPoopCounter;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //DISPLAY//TODO:MAKE OWN SCRIPT

    public GameObject display_pickUpItem;

    // new for input system
    public InputActionAsset InputActions;
    private PlayerInputController _playerInputController;

    private InputAction _pauseAction;
    private InputAction _sprintAction;
    private InputAction _consumeAction;
    private InputAction _poopAction;
    private InputAction _moveAction;

    public void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    public void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _consumeAction = InputSystem.actions.FindAction("Interact");
        _poopAction = InputSystem.actions.FindAction("Poop");
        _pauseAction = InputSystem.actions.FindAction("Pause");
    }

    public void Update()
    {
        displayPoopCounter.text = poopCounter.ToString();

        DropItem();
        ThirdPersonMovement();

        if (_poopAction.WasPressedThisFrame())
        {
            Relieve();
        }

        if (_pauseAction.WasPressedThisFrame())
        {
            PauseGame();
        }

        if (_sprintAction.WasPressedThisFrame())
        {
            Sprint();
        }
    }

    public void ThirdPersonMovement()
    {
        float horizontal = _playerInputController.MovementInputVector.x;
        float vertical = _playerInputController.MovementInputVector.y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        ratAnimator.SetMoveSpeed(direction.normalized.magnitude * speed);
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;
        }
        else
        {
            speed = 6f;
        }
    }
    public bool isHoldingItem;
    public bool stomachFull;
    public int poopCounter;
    public Transform pooper;
    public GameObject poop;

    public GameObject displayConsume;

    public void Consume()
    {
        
    }
    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "collectable" & !isHoldingItem)
        {
            //Display Prompt
            display_pickUpItem.SetActive(true);
            displayConsume.SetActive(false);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ratAnimator.Attack();
                Debug.Log("You've picked up an item");
                PickUpItem(col.gameObject);
            }

        }
        else if (col.gameObject.tag == "collectable" & isHoldingItem)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("You're already holding something");
                //TODO:Add Display Test on Screen
            }
        }
        else
        {
            if (col.gameObject.tag == "consummable")
            {
                displayConsume.SetActive(true);
                display_pickUpItem.SetActive(false);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(col.gameObject);
                }
            }
            Debug.Log("Nothing bumped in to");

        }
    }

    public void OnTriggerExit(Collider col)
    {
        display_pickUpItem.SetActive(false);
        displayConsume.SetActive(false);
    }
    public void AddToPoop()
    {
        CheckStomachState();
        if (stomachFull)
        {
            Debug.Log("You're too full");
        }
        else
        {
            poopCounter++;
        }
    }
    public void PickUpItem(GameObject go)
    {
        float objectDistanceFromMouseY = 0.66f;


        if (go.GetComponent<BaseItem>().itemName == "Big Cheese")
        {
            Debug.Log("You have picked up the big cheese");
            Instantiate(go.GetComponent<BaseItem>().go, new Vector3(itemSpawnPoint.position.x, itemSpawnPoint.position.y + objectDistanceFromMouseY, itemSpawnPoint.position.z), Quaternion.identity, itemSpawnPoint);

            //TODO:Add Value to Player Score
            Destroy(go);
            isHoldingItem = true;
        }
        if (go.GetComponent<BaseItem>().itemName == "Small Cheese")
        {
            ratAnimator.Eat();
            AddToPoop();
            Destroy(go);
            isHoldingItem = false;
        }
    }

    public void Relieve()
    {
        CheckStomachState();
        bool pooping = true;
        if (poopCounter != 0 & pooping == true)
        {
            Instantiate(poop, pooper.position, poop.transform.rotation * Quaternion.Euler(90f, 0, 30f));
            pooping = false;
            poopCounter--;
        }
        else
        {
            Debug.Log("You're stomach is empty. Eat cheese to poop");
            pooping = false;
        }
    }
    public void CheckStomachState()
    {
        if (poopCounter <= 0)
        {
            stomachFull = false;
        }
        else if (poopCounter >= 5)
        {
            poopCounter = 5;
            stomachFull = true;
        }
        else
        {
            stomachFull = false;
        }
    }
    public void DropItem()
    {
        float objectDistanceFromMouseY = 0.66f;

        if (isHoldingItem & Input.GetKeyDown(KeyCode.Q))
        {
            GameObject go = itemSpawnPoint.transform.GetChild(0).gameObject.GetComponent<BaseItem>().go;
            Destroy(itemSpawnPoint.transform.GetChild(0).gameObject);
            if (itemSpawnPoint.transform.GetChild(0).gameObject.name.Contains("Big Cheese Model"))
            {
                Instantiate(go, dropSpawnPoint.position, Quaternion.identity);
                //go.transform.localPosition = dropSpawnPoint.localPosition; 
            }

            isHoldingItem = false;
        }
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused. Lol. Lmao.");
    }
}
