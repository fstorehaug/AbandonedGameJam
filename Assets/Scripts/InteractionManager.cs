using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    public TileMapGenerator tileMap;

    public UnityAction<int, int> onTileSelect;
    public UnityAction<int, int> onTileHover;

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

    private void Update()
    {
        Vector3 hitPosLocal = GetTileCoordinateAtMouse();

        onTileHover?.Invoke((int)hitPosLocal.x, (int)hitPosLocal.z);
    }

    private void OnTileSelectPerformed(InputAction.CallbackContext context)
    {
        Vector3 hitPosLocal = GetTileCoordinateAtMouse();

        onTileSelect?.Invoke((int)hitPosLocal.x, (int)hitPosLocal.z);
    }

    private Vector3 GetTileCoordinateAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float d = (tileMap.transform.position.y - ray.origin.y) / ray.direction.y;
        Vector3 hitPosWorld = ray.origin + ray.direction * d;
        return tileMap.transform.InverseTransformPoint(hitPosWorld);
    }
}
