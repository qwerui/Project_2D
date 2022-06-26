using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugScript : MonoBehaviour
{
    public void MyException(string sentence)
    {
        throw new Exception(sentence);
    }
}
