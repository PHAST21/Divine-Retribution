using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HideCanvasElementInEditor : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Image>().enabled = !Application.isEditor || Application.isPlaying;
    }
    void OnDisable()
    {
        GetComponent<Image>().enabled = true;
    }
}