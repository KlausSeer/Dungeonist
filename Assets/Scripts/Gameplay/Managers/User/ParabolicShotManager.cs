using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShotManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ammo;
    [SerializeField]
    private float launchforce;
    [SerializeField]
    private Transform shotPivot;
    private bool holdingShot;
    private Vector2 direction;

    [SerializeField]
    private GameObject point;
    private GameObject[] points;
    [SerializeField]
    private int numberOfPoints;
    [SerializeField]
    private float spaceBetweenPoints;
    [SerializeField]
    private float maxDistance;

    private void OnHoldingShotStarted()
    {
        if(!holdingShot)
        {
            holdingShot = true;
        }
    }

    private void OnHoldingShot()
    {
        if(holdingShot)
        {
            CalculateLaunchForce();
        }
    }

    private void OnHoldingShotEnded()
    {
        if (holdingShot)
        {
            holdingShot = false;
            Shoot();
            HidePath();
        }
    }

    private void Shoot()
    {
        GameObject newAmmo = Instantiate(ammo, shotPivot.position, shotPivot.rotation);
        newAmmo.GetComponent<Rigidbody2D>().velocity = transform.right * launchforce;
    }

    private void UpdateShotPivot()
    {
        Vector2 bowPositon = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = bowPositon - mousePosition;
        transform.right = direction;
        if(holdingShot)
            ShowPath();
    }

    private void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPivot.position, Quaternion.identity);
        }
    }

    private void ShowPath()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].transform.position = PointPosition((i * spaceBetweenPoints));
            if((Vector2)points[i].transform.position != Vector2.zero)
                points[i].SetActive(true);
        }
    }

    private void HidePath()
    {
        foreach (var point in points)
        {
            point.SetActive(false);
        }
    }
    
    private void CalculateLaunchForce()
    {
        Vector2 bowPositon = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(bowPositon, mousePosition);
        launchforce = LocalMath.map(distance, 0, 10, 8, 16);
    }

    private bool PointsInRange(Vector2 position)
    {
        bool inRange;

        Vector2 moveDist = (Vector2)shotPivot.position - position;

        inRange = moveDist.magnitude < maxDistance;

        return inRange;
    }

    private Vector2 PointPosition(float time)
    {
        Vector2 position = (Vector2)shotPivot.position + (direction.normalized * launchforce * time) + 0.5f * Physics2D.gravity * (time*time);
        if (PointsInRange(position))
            return position;
        else
            return Vector2.zero;
    }

    private void Update()
    {
        UpdateShotPivot();

        if(Input.GetMouseButtonDown(0))
        {
            OnHoldingShotStarted();
        }

        OnHoldingShot();

        if (Input.GetMouseButtonUp(0))
        {
            OnHoldingShotEnded();
        }
    }
}
