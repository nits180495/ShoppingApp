using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CustomerState
{
    goingShop,
    waiting,
    goingHome
}

public enum ProductType
{
    Red,
    Blue
}

public class CustomerHandler : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoints;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f; // New variable for rotation speed
    public Image indication;
    public ProductType productType;

    private int currentWaypointIndex = 0;
    private CustomerState customerState;
    private bool isDoneOrder = false;


    private void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }
        animator.SetBool("isWalking", true);
        customerState = CustomerState.goingShop;
    }

    /// <summary>
    /// Start getting order. Moving towartds the shop.
    /// </summary>
    internal void GetOrder()
    {
        StartCoroutine(WaitForMessage());
    }

    IEnumerator WaitForMessage()
    {
        indication.color = productType == ProductType.Red ? Color.red : Color.blue;
        indication.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        customerState = CustomerState.goingHome;
        animator.SetBool("isWalking", true);
        yield return new WaitForSeconds(2f);
        indication.color = Color.white;
        indication.gameObject.SetActive(false);
    }


    private void Update()
    {
        // AI for customer using Waypoints.
        if (isDoneOrder) return;

        if (Input.GetKeyDown(KeyCode.Space))
            GetOrder();

        if (customerState == CustomerState.goingShop)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Rotation logic
            Vector3 directionToTarget = targetPosition - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                currentWaypointIndex++;
            }

            if (currentWaypointIndex >= waypoints.Length)
            {
                indication.gameObject.SetActive(true);
                customerState = CustomerState.waiting;
                animator.SetBool("isWalking", false);
            }
        }
        if(customerState == CustomerState.goingHome)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex - 1].position;
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Rotation logic
            Vector3 directionToTarget = targetPosition - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                currentWaypointIndex--;
            }

            if(currentWaypointIndex <= 0)
            {
                isDoneOrder = true;
                animator.SetBool("isWalking", false);
            }
        }
    }
}