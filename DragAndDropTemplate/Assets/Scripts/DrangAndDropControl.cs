using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrangAndDropControl : MonoBehaviour
{
    private Vector2 touchPosition;
    private Vector3 pointTouchedInWorldGameSpace;

    private Draggrable lastDraggrableSelected;
    private SpriteRenderer lastDraggrableSelectedImg;
    [SerializeField]
    private int lastSortSelected;


    [SerializeField]
    private bool dragging;
    private bool dragStarted;
    float distanceTouchToDraggrableCenterX;
    float distanceTouchToDraggrableCenterY;

    private int sortingOrderDrag;
    
    void Awake()
    {
        dragging = false;
        dragStarted = false;

        sortingOrderDrag = 200;
    }


    void Update()
    {
        /*if(Input.GetMouseButton(0))
        {
            Vector3 mouseClick = Input.mousePosition;

            touchPosition.x = mouseClick.x;
            touchPosition.y = mouseClick.y;

        }else if(Input.touchCount>0)
        {
            touchPosition = Input.GetTouch(0).position;
        }*/

        //Transforms a point from screen space into world space
        //pointTouchedInWorldGameSpace = Camera.main.ScreenToWorldPoint(touchPosition);

        if(dragging)
        {
            drag();

        }

        if (dragging && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            drop();
        }
    }

 

    private void startDrag(bool IcanStart)
    {
        if(IcanStart)
        {
            dragging = true;
            Vector2 startDraggrablePosition = lastDraggrableSelected.transform.position;
            distanceTouchToDraggrableCenterX = startDraggrablePosition.x - pointTouchedInWorldGameSpace.x;
            distanceTouchToDraggrableCenterY = startDraggrablePosition.y - pointTouchedInWorldGameSpace.y;

            lastDraggrableSelectedImg = lastDraggrableSelected.gameObject.GetComponent<SpriteRenderer>();
            lastSortSelected = lastDraggrableSelectedImg.sortingOrder;
        }
    }

    private void drag()
    {
        
        pointTouchedInWorldGameSpace.z = 0;

        lastDraggrableSelected.transform.position = new Vector2(pointTouchedInWorldGameSpace.x + distanceTouchToDraggrableCenterX, +pointTouchedInWorldGameSpace.y + distanceTouchToDraggrableCenterY);
        
        lastDraggrableSelectedImg.sortingOrder = sortingOrderDrag;
    }

    private void drop()
    {
        //Debug.Log(lastDraggrableSelected.gameObject.name);
        lastDraggrableSelected.restartLayerOrder();
        lastDraggrableSelected.Drop();
        
        lastDraggrableSelected = null;
        dragging = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("tocou");
        startDrag(verifyHitsToStartDrag());
    }

    private bool verifyHitsToStartDrag()
    {
        pointTouchedInWorldGameSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (!dragging)
        {
            RaycastHit2D hit = Physics2D.Raycast(pointTouchedInWorldGameSpace, Vector2.zero);

            if (hit.collider != null)
            {
                Draggrable draggrable = hit.transform.gameObject.GetComponent<Draggrable>();

                if (draggrable != null)
                {
                    lastDraggrableSelected = draggrable;
                    
                    return true;
                }
            }
        }
        return false;
    }
}
