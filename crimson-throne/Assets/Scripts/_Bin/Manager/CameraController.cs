// using UnityEngine;
// using Cinemachine;

// public class LockCinemachineSize : MonoBehaviour
// {
//     [SerializeField] private CinemachineCam virtualCamera; 
//     [SerializeField] private float targetWidth = 10f; // Chiều rộng cố định trong thế giới

//     void Update()
//     {
//         Camera mainCamera = Camera.main;
//         if (mainCamera != null)
//         {
//             float screenAspect = (float)Screen.width / Screen.height; // Tỷ lệ khung hình
//             float orthographicSize = targetWidth / 2f / screenAspect;

//             // Gán giá trị orthographicSize cho Cinemachine Camera
//             var lensSettings = virtualCamera.m_Lens;
//             lensSettings.OrthographicSize = orthographicSize;
//             virtualCamera.m_Lens = lensSettings;
//         }
//     }
// }
