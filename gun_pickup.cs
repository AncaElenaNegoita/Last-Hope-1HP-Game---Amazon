using UnityEngine;

public class gun_pickup : MonoBehaviour
{

    public gun arma; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PickUp()
    {
        Debug.Log($"Picked up: {this.gameObject.name}");
        // You could trigger animations, add to inventory, etc. here
        gameObject.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
