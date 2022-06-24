using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Draggrable : MonoBehaviour
{
    [SerializeField]
    private int defaultLayerOrder;
    private SpriteRenderer sprite;

    [SerializeField]
    private GameObject[] putBases = new GameObject[8];
    //private int countPutBasesCollisions = 0;

    [SerializeField]
    private int wrongPositionCount;

    public bool dropped;

    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        defaultLayerOrder = sprite.sortingOrder;
        SortLayer.objListUpdate(gameObject);
        startPosition = gameObject.transform.position;
        wrongPositionCount = 0;
    }

    public void restartLayerOrder()
    {
        //sprite.sortingOrder = defaultLayerOrder;
        SortLayer.sortDefaultsLayerOrder(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisioned = collision.gameObject;

        //bool putBasesContainsCollisioned = Array.Exists(putBases, gameObject => gameObject == collisioned);

        if (!verifyPutBasesContain(collisioned) && collisioned.tag == "putBase")
        {
            putObjInPutBases(collisioned);
        }

        verifyWrongPositionEnter(collision);


        //Debug.Log(putBasesContainsCollisioned);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collisioned = collision.gameObject;

        if (collisioned.tag == "putBase")
        {
            //Debug.Log(collisioned.name);
            clearExitPutBase(collisioned);
        }

        verifyWrongPositionExit(collision);
    }

    private void putObjInPutBases(GameObject putBase)
    {
        for(int i = 0; i < putBases.Length; i++)
        {
            if(putBases[i]==null && !verifyPutBasesContain(putBase))
            putBases[i] = putBase;
        }

    }

    public void Drop()
    {
        if (inWrongPosition())
            restartPosition();
        else
            verifyClosiestPutBase();
    }

    private void verifyClosiestPutBase()
    {
        GameObject selectedBase = null;

        float baseX = 0;
        float xDistance = 0;
        float baseY = 0;
        float yDistance = 0;
        float distance = 5000;
        for (int i = 0; i < putBases.Length; i++)
        {
            SpriteRenderer spriteBase = sprite;

            if (putBases[i]!=null)
            {
                spriteBase = putBases[i].GetComponent<SpriteRenderer>();
                baseX = putBases[i].transform.position.x;
                
                if (gameObject.transform.position.x < baseX)
                {
                    xDistance = Math.Abs(baseX - gameObject.transform.position.x);
                }
                else
                {
                    xDistance = Math.Abs(gameObject.transform.position.x - baseX);
                }

                baseY = putBases[i].transform.position.y;

                if (gameObject.transform.position.y < baseY)
                {
                    yDistance = Math.Abs(baseY - gameObject.transform.position.y);
                }
                else
                {
                    yDistance = Math.Abs(gameObject.transform.position.y - baseY);
                }

                /*Debug.Log((spriteBase.size.x * spriteBase.transform.localScale.x) / 2 + " >= " + xDistance + " && " + (spriteBase.size.y * spriteBase.transform.localScale.y) / 2 + ">=" + yDistance);
                if ((spriteBase.size.x * spriteBase.transform.localScale.x) /2  >= xDistance && (spriteBase.size.y * spriteBase.transform.localScale.y) / 2 >= yDistance )
                {
                    selectedBase = putBases[i];
                }*/

                //Debug.Log((spriteBase.bounds.size.x ) + " >= " + xDistance * 2+ " && " + (spriteBase.bounds.size.y) + ">=" + yDistance*2);
                if ((spriteBase.bounds.size.x) >= xDistance*2 && (spriteBase.bounds.size.y) >= yDistance*2 && Vector2.Distance(putBases[i].transform.position, gameObject.transform.position)<distance)
                {
                    selectedBase = putBases[i];
                    distance = Vector2.Distance(putBases[i].transform.position, gameObject.transform.position);
                }

                //Debug.Log("size x: " + spriteBase.size.x + "----size y: " + spriteBase.size.y);
            }

            //Debug.Log("loop " + i);
            //Debug.Log(spriteBase.size.x / 2 +">"+ xDistance+ "&&"+ spriteBase.size.y / 2+ ">"+ yDistance);
            
        }

        if(selectedBase != null)
        {
            gameObject.transform.position = selectedBase.transform.position;
        }else
        {
            restartPosition();
        }
    }

    private void clearExitPutBase(GameObject selectedBase)
    {
        int index = 0;
        foreach(GameObject putBase in putBases)
        {
            if(putBase == selectedBase)
            {
                putBases[index] = null;
            }
            index++;
        }
    }

    private bool verifyPutBasesContain(GameObject objExist)
    {
        //Debug.Log("contains veirfy --------------");
        return Array.Exists(putBases, gameObject => gameObject == objExist);
    }

    private void addWrongPositionCount()
    {
        if(wrongPositionCount<0)
            resetWrongPositionCount();

        wrongPositionCount++;
    }

    private void subtractWrongPositionCount()
    {
        wrongPositionCount--;
    }

    private void resetWrongPositionCount()
    {
        wrongPositionCount = 0;
    }

    private bool inWrongPosition()
    {
        if (wrongPositionCount > 0)
        {
            resetWrongPositionCount();
            return true;
        }

        resetWrongPositionCount();
        return false;
    }

    private void verifyWrongPositionEnter(Collider2D obj)
    {
        if (obj.tag == "wrongPosition")
            addWrongPositionCount();
    }

    private void verifyWrongPositionExit(Collider2D obj)
    {
        if (obj.tag == "wrongPosition")
            subtractWrongPositionCount();
    }

    private void restartPosition()
    {
        gameObject.transform.position = startPosition;
    }


    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
