using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm : MonoBehaviour
{
    public float speed = 10.0f; // Tekerlekli modelin hareket hýzý
    private float horizontalInput; // Yatay eksen için input
    private float verticalInput; // Dikey eksen için input
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

      //  transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
    }
}
