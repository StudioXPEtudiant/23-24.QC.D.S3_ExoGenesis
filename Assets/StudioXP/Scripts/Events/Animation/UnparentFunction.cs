using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentFunction : MonoBehaviour
{
    public void Unparent()
    {
        transform.parent = null;
    }
}
