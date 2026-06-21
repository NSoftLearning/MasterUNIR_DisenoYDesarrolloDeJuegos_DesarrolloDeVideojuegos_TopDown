using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference _movement;
    [SerializeField] private InputActionReference _attack;
    [SerializeField] private InputActionReference _interact;
    [SerializeField] private InputActionReference _roll;
    [SerializeField] private InputActionReference _changeWeapon;
    [SerializeField] private InputActionReference _quickAccessOne;
    [SerializeField] private InputActionReference _quickAccessTwo;
    [SerializeField] private InputActionReference _inventory;

    private CustomCharacterController _characterController;
    private PlayerWeaponController _weaponController;

    private Vector2 _rawMove;
    private IInteractables currentInteractable;

    private void Awake()
    {
        _characterController = GetComponent<CustomCharacterController>();
        _weaponController = GetComponent<PlayerWeaponController>();
    }

    private void OnEnable()
    {
        EnableAction(_movement);
        EnableAction(_attack);
        EnableAction(_interact);
        EnableAction(_roll);
        EnableAction(_changeWeapon);
        EnableAction(_quickAccessOne);
        EnableAction(_quickAccessTwo);
        EnableAction(_inventory);

        if (_movement != null)
        {
            _movement.action.started += Move;
            _movement.action.performed += Move;
            _movement.action.canceled += Move;
        }

        if (_interact != null)
            _interact.action.performed += OnInteract;

        if (_roll != null)
            _roll.action.performed += OnRoll;

        if (_attack != null)
            _attack.action.performed += OnAttack;

        if (_changeWeapon != null)
            _changeWeapon.action.performed += OnChangeWeapon;

        if (_quickAccessOne != null)
            _quickAccessOne.action.performed += OnQuickAccessOne;

        if (_quickAccessTwo != null)
            _quickAccessTwo.action.performed += OnQuickAccessTwo;

        if (_inventory != null)
            _inventory.action.performed += OnInventory;

    }

    private void OnDisable()
    {
        if (_movement != null)
        {
            _movement.action.started -= Move;
            _movement.action.performed -= Move;
            _movement.action.canceled -= Move;
        }

        if (_interact != null)
            _interact.action.performed -= OnInteract;

        if (_roll != null)
            _roll.action.performed -= OnRoll;

        if (_attack != null)
            _attack.action.performed -= OnAttack;

        if (_changeWeapon != null)
            _changeWeapon.action.performed -= OnChangeWeapon;

        if (_quickAccessOne != null)
            _quickAccessOne.action.performed -= OnQuickAccessOne;

        if (_quickAccessTwo != null)
            _quickAccessTwo.action.performed -= OnQuickAccessTwo;

        if (_inventory != null)
            _inventory.action.performed -= OnInventory;

        DisableAction(_movement);
        DisableAction(_attack);
        DisableAction(_interact);
        DisableAction(_roll);
        DisableAction(_changeWeapon);
        DisableAction(_quickAccessOne);
        DisableAction(_quickAccessTwo);
        DisableAction(_inventory);
    }

    private void EnableAction(InputActionReference actionReference)
    {
        if (actionReference == null || actionReference.action == null)
            return;

        actionReference.action.Enable();
    }

    private void DisableAction(InputActionReference actionReference)
    {
        if (actionReference == null || actionReference.action == null)
            return;

        actionReference.action.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        _rawMove = context.ReadValue<Vector2>();

        if (_characterController != null)
        {
            _characterController.SetRawMovement(_rawMove);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        currentInteractable?.StartInteraction();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_characterController != null)
        {
            _characterController.Roll();
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_characterController != null)
        {
            _characterController.Attack();
        }
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_weaponController != null && _weaponController.IsAttacking())
            return;

        InventoryManager inventoryManager = GetInventoryManager();

        if (inventoryManager != null)
        {
            inventoryManager.EquipNextWeapon();
        }
        else if (_weaponController != null)
        {
            _weaponController.ChangeNextWeapon();
        }
    }

    private void OnQuickAccessOne(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        InventoryManager inventoryManager = GetInventoryManager();

        if (inventoryManager != null)
        {
            inventoryManager.UseQuickAccessItem(0);
        }
    }

    private void OnQuickAccessTwo(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        InventoryManager inventoryManager = GetInventoryManager();

        if (inventoryManager != null)
        {
            inventoryManager.UseQuickAccessItem(1);
        }
    }

    private void OnInventory(InputAction.CallbackContext context)
    {
        GetInventoryManager().ToggleInventory();
    }

    private InventoryManager GetInventoryManager()
    {
        if (ComponentLocatorService.Components != null &&
            ComponentLocatorService.Components.InventoryManager != null)
        {
            return ComponentLocatorService.Components.InventoryManager;
        }

        return InventoryManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractables interactable))
        {
            currentInteractable = interactable;
            currentInteractable.Select();
            return;
        }

        if (collision.TryGetComponent(out CollectibleWeapon collectibleWeapon))
        {
            WeaponData weaponData = collectibleWeapon.GetWeapon();

            if (weaponData == null)
                return;

            InventoryManager inventoryManager = GetInventoryManager();

            if (inventoryManager != null)
            {
                inventoryManager.AddWeapon(weaponData);
                inventoryManager.EquipWeapon(weaponData);
            }
            else if (_weaponController != null)
            {
                _weaponController.NewWeapon(collision.gameObject);
            }

            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractables interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable.Unselect();
                currentInteractable = null;
            }
        }
    }
}