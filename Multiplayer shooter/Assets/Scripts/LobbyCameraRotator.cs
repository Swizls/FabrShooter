using UnityEngine;

public class LobbyCameraRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + _rotationSpeed * Time.deltaTime, 0);   
    }
}
