using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitController : MonoBehaviour
{
    [SerializeField]
    GameObject selectionArea;
    [SerializeField]
    Camera cam;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask unitLayer;
    [SerializeField]
    LayerMask targetLayer;
    [SerializeField]
    float targetMoveSpeed = 10;

    Vector3 startPosition;
    Vector3 lastMousePosition;
    Transform currentTarget;
    List<unit> units;
    float unitsSpeed = 3.5f;
    private void Start()
    {
        units = new List<unit>();
        selectionArea.SetActive(false);
    }
    void Update()
    {
        getSelectionInputs();
        getTargetInputs();
    }
    void getTargetInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, targetLayer))
            {
                currentTarget = hit.transform;
                setTarget(currentTarget);
            }
        }
        if (Input.GetMouseButton(0))
        {
            moveTarget();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (currentTarget != null)
            {
                setTarget(currentTarget);
            }
            currentTarget = null;
        }
    }
    private void moveTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.position = Vector3.Lerp(currentTarget.position,
                getMousePositionInWorldSpace(), targetMoveSpeed * Time.deltaTime);
        }
    }
    void setTarget(Transform target)
    {
        setUnitsSpeed(unitsSpeed);

        List<Vector3> targetPostionsList = getPositionsAround(target.position, 2f, units.Count);

        int targetPosIndex = 0;
        foreach (unit _unit in units)
        {
            _unit.goTo(targetPostionsList[targetPosIndex],currentTarget.position);
            targetPosIndex += 1;
        }
    }
    public void setUnitsSpeed(float speed)
    {
        foreach (unit _unit in units)
        {
            _unit.setSpeed(speed);
        }
        unitsSpeed = speed;
    }
    void getSelectionInputs()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectionArea.SetActive(true);
            startPosition = getMousePositionInWorldSpace();

        }
        if (Input.GetMouseButton(1))
        {
            resizeSelectionArea();

        }
        if (Input.GetMouseButtonUp(1))
        {
            selectionArea.SetActive(false);
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                deselectAndClearUnits();
            }
            selectUnitsInSelectionArea();

            selectUnitIfThereIs();
        }
    }
    List<Vector3> getPositionsAround(Vector3 startPos,float distance, int posCount)
    {
        List<Vector3> positionsList = new List<Vector3>();
        for (int i = 0; i < posCount; i++)
        {
            float angle = i * (360f / posCount);

            Vector3 dir = applyRotationToVector(new Vector3(1, 0, 0), angle);

            Vector3 position = startPos + dir * distance;
            positionsList.Add(position);
        }
        return positionsList;
    }
    Vector3 applyRotationToVector(Vector3 vector,float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vector;
    }

    void resizeSelectionArea()
    {
        Vector3 currentMousePosition = getMousePositionInWorldSpace();


        Vector3 bottomLeft = new Vector3(
            Mathf.Min(startPosition.x, currentMousePosition.x),
            0,
            Mathf.Min(startPosition.z, currentMousePosition.z)
            );


        Vector3 topRight = new Vector3(
            Mathf.Max(startPosition.x, currentMousePosition.x),
            0,
            Mathf.Max(startPosition.z, currentMousePosition.z)
            );
        selectionArea.transform.position = bottomLeft;
        Vector3 scale = topRight - bottomLeft;
        scale.y = 1;
        selectionArea.transform.localScale = scale;
    }
    void selectUnitsInSelectionArea()
    {
        BoxCollider selectionAreaCollider = selectionArea.GetComponentInChildren<BoxCollider>();

        Vector3 worldCenter = selectionAreaCollider.transform.TransformPoint(selectionAreaCollider.center);
        Vector3 worldHalfExtents = selectionAreaCollider.transform.TransformVector(selectionAreaCollider.size * 0.5f);

        Collider[] colliders = Physics.OverlapBox(worldCenter, worldHalfExtents, selectionAreaCollider.transform.rotation, unitLayer);
        foreach (Collider collider in colliders)
        {
            unit _unit = collider.GetComponent<unit>();
            if (!_unit.isSelected())
            {
                _unit.select(true);
                units.Add(_unit);
            }
        }
    }
    Vector3 getMousePositionInWorldSpace()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            lastMousePosition = hit.point;
        }
        return lastMousePosition;
    }
    void deselectAndClearUnits()
    {
        foreach (unit _unit in units)
        {
            _unit.select(false);
        }
        units.Clear();
    }
    void selectUnitIfThereIs()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, unitLayer))
        {
            unit _unit = hit.transform.GetComponent<unit>();
            if (!_unit.isSelected())
            {
                units.Add(_unit);
                _unit.select(true);
            }
        }
    }
    public int getSelectedUnitsCount()
    {
        return units.Count;
    }
    public unit getSelectedUnit()
    {
        return units[0];
    }
}
