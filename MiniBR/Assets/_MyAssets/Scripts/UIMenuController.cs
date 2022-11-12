using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
   

    [SerializeField] GameObject[] uiPages;
    int currentPage = 0;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeUIPageGAmeObject(currentPage);
    }

     public void NextPage()
    {
        if(currentPage < uiPages.Length)
        {
            currentPage++;
        }
        else
        {
            currentPage = 0;
        }
    }

    public void PrevPage()
    {
        if(currentPage < uiPages.Length && currentPage > 0)
        {
            currentPage--;
        }
    }

    void ChangeUIPageGAmeObject(int pageID)
    {
        for(int i = 0; i < uiPages.Length; i++)
        {
            uiPages[i].SetActive(false);
        }

        if(pageID < uiPages.Length)
        {
            uiPages[pageID].SetActive(true);
        }
    }
}
