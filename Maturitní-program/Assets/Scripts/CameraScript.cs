using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Rychlost pohybu kamery
    public float xEdgeThreshold = 50f; // Okraj obrazovky pro aktivaci pohybu
    public float yEdgeThreshold = 200f; // Myš musí být pod touto hranicí na Y
    private float maxLeft = -19f;
    private float maxRight = 10.5f;

    private void Start()
    {
        
    }

    void Update()
    {
        float posX = transform.position.x;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 cameraMove = Vector3.zero;

        // Podmínka: Kamera se pohybuje pouze, pokud je myš více než 50 px od horního okraje
        
        if (mouseY > screenHeight - yEdgeThreshold)
        {
            return; // Pokud je myš v horním 50px pásmu, nepohybuj kamerou
        }

        // Pohyb doprava
        if (posX < maxRight)
        {
            if (mouseX >= screenWidth - xEdgeThreshold)
            {
                cameraMove.x += moveSpeed * Time.deltaTime;
            }

        }

        // Pohyb doleva
        if(posX > maxLeft)
        {
            if (mouseX <= xEdgeThreshold)
            {
                cameraMove.x -= moveSpeed * Time.deltaTime;
            }

        }

        // Aplikace pohybu
        transform.position += cameraMove;
    }
}