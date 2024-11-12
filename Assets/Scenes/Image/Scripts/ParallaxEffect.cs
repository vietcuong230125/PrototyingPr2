using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform; // Lấy tọa độ của camera
    public float parallaxFactor; // Mức độ chuyển động của layer , = 1 là khi layer xa nhất, = 0 khi layer gần nhất
    private Vector3 lastCameraPosition; // Lấy lại tọa độ của camera trong hàm update

    void Start()
    {
        // Lấy vị trí ban đầu của camera
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // Tính toán khoảng cách camera đã di chuyển chỉ theo trục X
        float delta = cameraTransform.position.x - lastCameraPosition.x;

        // Di chuyển Layer theo trục X
        transform.position += new Vector3(delta * parallaxFactor, 0, 0);

        // Lấy lại vị trí camera 
        lastCameraPosition = cameraTransform.position;
    }
}
