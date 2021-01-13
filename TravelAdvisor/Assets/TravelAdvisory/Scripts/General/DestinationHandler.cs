using TMPro;
using UnityEngine;

public class DestinationHandler : MonoBehaviour
{
    private TMP_Dropdown dropdownMenu;
    private TMP_InputField inputField;
    private string destination;
    private LocationMaster locationMaster;

    [SerializeField] GameObject destinationResultUI;
    [SerializeField] TMP_Text destinationText;

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
        if (dropdownMenu != null)
        {
            
            destination = dropdownMenu.options[dropdownMenu.value].text;
            inputField.text = destination;

            locationMaster.CallEventDestinationCaptured(destination);

            dropdownMenu.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);
            destinationResultUI.SetActive(true);
            destinationText.text = destination;
        }
    }

    public void ClearDropDownContents() //Called by input menu Back & Edit buttons.
    {
        inputField.text = "";
        dropdownMenu.gameObject.SetActive(false);
    }
}
