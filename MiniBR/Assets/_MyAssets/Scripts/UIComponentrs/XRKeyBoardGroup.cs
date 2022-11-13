
using System.Collections.Generic;
using UnityEngine;

public class XRKeyBoardGroup : MonoBehaviour
{
    public List<GameObject> keyboards = new List<GameObject>();

    public void ActivateCanvas(int objectIndex)
    {
        for (int x = 0; x < keyboards.Count; x++)
        {
            if (keyboards[x] != null)
            {
                keyboards[x].SetActive(x == objectIndex);
            }
        }
    }
}
