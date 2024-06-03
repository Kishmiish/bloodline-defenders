using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class HoverMoveRight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition;
    private bool isHovering;

    [SerializeField] private float moveAmount = 10f;
    [SerializeField] private float speed = 20f;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void OnClick()
    {
        isHovering = false;
        transform.position = originalPosition;
    }

    private void Update()
    {
        if (isHovering && transform.position.x - originalPosition.x < moveAmount)
        {
            
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if(!isHovering && transform.position.x >= originalPosition.x)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
