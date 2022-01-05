using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text countText;
    [SerializeField]
    TMP_Text velocityText;
    unitController _unitController;

    int selectedUnitsCount;
    private void Start()
    {
        _unitController = GetComponent<unitController>();
        velocityText.gameObject.SetActive(false);
    }
    void Update()
    {
        int currentSelectedUnitsCount = _unitController.getSelectedUnitsCount();

        if (selectedUnitsCount != currentSelectedUnitsCount)
        {
            selectedUnitsCount = currentSelectedUnitsCount;

        countText.text = "units count : " + selectedUnitsCount;
        }
        if(currentSelectedUnitsCount == 1)
        {
            velocityText.gameObject.SetActive(true);
            float unitVelocity = _unitController.getSelectedUnit().getVelocity();
            velocityText.text = "velocity : " + unitVelocity.ToString("F1");
        }
        else
        {
            velocityText.gameObject.SetActive(false);
        }
    }
}
