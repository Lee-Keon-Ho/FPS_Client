using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        image.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
