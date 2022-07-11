using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoomImage : MonoBehaviour, IScrollHandler
{
    private Vector3 initialScale;

    public Vector3 desiredScale;

    public GameObject[] children;

    public Vector3[] childrenScales;

    [SerializeField]
    private float zoomSpeed = 0.1f;
    [SerializeField]
    private float maxZoom = 10f;

    private void Awake()
    {
        initialScale = transform.localScale;
        GetChildrenScales();
    }

    public void GetChildrenScales()
    {
        for(int i = 0; i < children.Length; i++)
        {
            for(int h = 0; h < childrenScales.Length; h++)
            {
                childrenScales[i] = children[i].transform.localScale;
            }
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;

        //PassDesiredScaleToChildren();
    }

    public void PassDesiredScaleToChildren()
    {
        for(int i = 0; i < children.Length; i++)
        {
            //children[i].transform.localScale = desiredScale;
            children[i].transform.localScale = new Vector3((childrenScales[i].x * desiredScale.x),(childrenScales[i].y * desiredScale.y),(childrenScales[i].z * desiredScale.z));
        }
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
        return desiredScale;
    }
}