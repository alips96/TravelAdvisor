using TMPro;
using UnityEngine;

public class DestinationHandler : MonoBehaviour
{
    private TMP_Dropdown dropdownMenu;
    private TMP_InputField inputField;
    private string destination;
    private LocationMaster locationMaster;

    private void Start()
    {
        SetInitialReferences();
    }

    private void SetInitialReferences()
    {
        locationMaster = GameObject.Find("GameManager").GetComponent<LocationMaster>();

        inputField = GetComponent<TMP_InputField>();
        dropdownMenu = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
    }

    public void DropDownMenuChanged() //Called by Dropdown Menu UI
    {
        if(dropdownMenu != null)
        {
            destination = dropdownMenu.options[dropdownMenu.value].text;
            inputField.text = destination;

            locationMaster.CallEventDestinationCaptured(destination);
        }
        else
        {
            Debug.LogError("dropDown menu not assigned!");
        }
    }
}
