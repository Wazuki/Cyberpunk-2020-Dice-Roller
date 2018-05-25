using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringToFront : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.SetAsLastSibling();
    }
}
