using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayer : MonoBehaviour
{
    private static GameObject[] objsList;

    private static bool startedObjsList = false;

    public static void objListUpdate(GameObject newObjToList)
    {
        if(!startedObjsList)
        {
            objsList = new GameObject[1];
            objsList[0] = newObjToList;

            startedObjsList = true;
        }else
        {
            GameObject[] newObjsList = new GameObject[objsList.Length + 1];

            objsList.CopyTo(newObjsList, 0);

            newObjsList[newObjsList.Length-1] = newObjToList;

            objsList = newObjsList;
        }
        //showObjsList();
    }

    private static void showObjsList()
    {
        Debug.Log("QUANTIDADE NA ARRAY == " + objsList.Length);
        for(int i =0; i<objsList.Length; i++)
        {
            Debug.Log("array[" + i + "] == " + objsList[i].name);
        }
    }

    public static void sortDefaultsLayerOrder(GameObject mainObj)
    {
        int erasePoint = 0;

        for(int i = 0; i < objsList.Length; i++)
        {
            if (objsList[i] == mainObj)
                erasePoint = i;
        }

        bool erasePointFound = false;

        for (int j = 0; j < objsList.Length; j++)
        {
            if (j == erasePoint)
            {
                erasePointFound = true;
            }

            if (erasePointFound)
            {
                if (j < objsList.Length - 1)
                {
                    objsList[j] = objsList[j+1];
                }
                else
                {
                    objsList[j] = mainObj;
                }
            }
        }

        for(int j =0; j < objsList.Length; j++)
        {
            objsList[j].GetComponent<SpriteRenderer>().sortingOrder = j;
        }
        //showObjsList();

    }
}
