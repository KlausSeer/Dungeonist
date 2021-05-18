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
    private int numberOfDisplayPoints;
    [SerializeField]
    private float spaceBetweenPoints;

    private void OnHoldingShotStarted()
    {
        if(!holdingShot)
        {
            holdingShot = true;
            ShowPath();
            Debug.Log("Ready");
        }
    }

    private void OnHoldingShot()
    {
        if(holdingShot)
        {
            Debug.Log("Steady");
        }
    }

    private void OnHoldingShotEnded()
    {
        if (holdingShot)
        {
            holdingShot = false;
            Debug.Log("Fire");
            Shoot();
            HidePath();
        }
    }

    private void Shoot()
    {
        GameObject newAmmo = Instantiate(ammo, shotPivot.position, shotPivot.rotation);
        newAmmo.GetComponent<Rigidbody2D>().velocity = transform.right * launchforce;
        Debug.Log("I'm an arrow");
    }

    private void UpdateShotPivot()
    {
        if(!holdingShot)
        {
            Vector2 bowPositon = transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePosition - bowPositon;
            transform.right = direction;
        }

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

    private Vector2 PointPosition(float time)
    {
        Vector2 position = (Vector2)shotPivot.position + (direction.normalized * launchforce * time) + 0.5f * Physics2D.gravity * (time*time);
        return position;
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
