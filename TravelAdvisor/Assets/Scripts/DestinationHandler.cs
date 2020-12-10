using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DestinationHandler : MonoBehaviour
{
    [SerializeField] Dropdown dropdownMenu;
    [SerializeField] InputField inputField;
    private string destination;
    private LocationMaster locationMaster;

    private void Start()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        locationMaster = GetComponent<LocationMaster>();
    }

    public void DropDownMenuChanged() //Called by Dropdown Menu UI
    {
        if(dropdownMenu != null)
        {
            destination = dropdownMenu.GetComponentInChildren<Text>().text;
            inputField.text = destination;

            locationMaster.CallEventDestinationCaptured(destination);
        }
        else
        {
            Debug.LogError("dropDown menu not assigned!");
        }
    }
}
