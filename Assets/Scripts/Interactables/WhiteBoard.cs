using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoard : MonoBehaviour
{
    public static event Action<WhiteBoard> OnWhiteBoardClicked;
    
    private void OnMouseDown()
    {
        OnWhiteBoardClicked?.Invoke(this);
    }
}
