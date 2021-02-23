using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    public TileMapGenerator tileMap;

    public UnityAction<int, int> onTileSelect;

    private PlayerControlls playerControlls;
    private Vector2 mousePosition;

    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    private void Start()
    {
        playerControlls = new PlayerControlls();

        playerControlls.Enable();
        playerControlls.KeyboardMouse.TileSelect.performed += OnTileSelectPerformed;
        playerControlls.KeyboardMouse.MousePosition.performed += context => mousePosition = context.ReadValue<Vector2>();
    }

    private void OnTileSelectPerformed(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float d = (tileMap.transform.position.y - ray.origin.y) / ray.direction.y;
        Vector3 hitPosWorld = ray.origin + ray.direction * d;
        Vector3 hitPosLocal = tileMap.transform.InverseTransformPoint(hitPosWorld);

        onTileSelect?.Invoke((int)hitPosLocal.x, (int)hitPosLocal.z);
    }
}
