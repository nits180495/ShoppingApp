using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    //For movement
    public float moveSpeed;
    public float rotationSpeed;
    public DynamicJoystick floatingJoystick;

    //Aniamtor
    public Animator animator;

    //For Count
    public static int productRedCount = 0;
    public static int productBlueCount = 0;
    public Text productRedCountText;
    public Text productBlueCountText;

    //For Coustomer
    private CustomerHandler currentCustomer;

    private void OnEnable()
    {
        SelfProduct.OnSingleProductPicked += UpdateProduct;
    }

    private void OnDisable()
    {
        SelfProduct.OnSingleProductPicked -= UpdateProduct;
    }

    private void Update()
    {
        // Calculate the rotation direction based on joystick input
        Vector3 joystickInput = new Vector3(floatingJoystick.Horizontal, 0f, floatingJoystick.Vertical);
        if (joystickInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(joystickInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("isWalking",true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Move the player
        transform.Translate(joystickInput * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Self"))
        {
            Debug.Log("Start Picking");
            other.gameObject.GetComponent<SelfProduct>().StarPickingObject();
        }
        if (other.CompareTag("Customer"))
        {
            Debug.Log("Start Droping");
            currentCustomer = other.GetComponent<CustomerHandler>();
            if ((currentCustomer.productType == ProductType.Red && productRedCount == 0)
                || (currentCustomer.productType == ProductType.Blue && productBlueCount == 0)) return;

            currentCustomer.GetOrder();
            UpdateProduct(currentCustomer.productType, -1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Self"))
        {
            Debug.Log("Stop Picking");
            other.gameObject.GetComponent<SelfProduct>().StopPicking();
        }
    }

    /// <summary>
    /// Method to Update Product count.
    /// </summary>
    /// <param name="product"></param>
    /// <param name="value"></param>
    void UpdateProduct(ProductType product, int value)
    {
        switch (product)
        {
            case ProductType.Red:
                productRedCount += value;
                productRedCountText.text = productRedCount + "";
                break;

            case ProductType.Blue:
                productBlueCount += value;
                productBlueCountText.text = productRedCount + "";
                break;
        }
        currentCustomer = null;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}