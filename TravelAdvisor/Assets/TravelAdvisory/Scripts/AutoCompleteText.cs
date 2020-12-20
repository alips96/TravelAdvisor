using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoCompleteText : MonoBehaviour
{
    private List<string> myList = new List<string>();
    private TMP_InputField inputField;
    private TMP_Dropdown dropDown;

    [SerializeField] private Country countrySO;

    private void Start()
    {
        myList = countrySO.AllRegions;
        inputField = GetComponent<TMP_InputField>();
        //dropDown = GetComponentInChildren<TMP_Dropdown>();
        dropDown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
    }

    public void InputValueChanged() //called by InputText UI
    {
        dropDown.ClearOptions();

        string word = inputField.text;

        if (!string.IsNullOrEmpty(word))
        {
            dropDown.gameObject.SetActive(true);

            List<string> found = myList.FindAll(w => w.StartsWith(word));

            if (found.Count > 0)
            {
                dropDown.AddOptions(new List<string> { inputField.text + ".." });
                dropDown.AddOptions(found);
            }
        }
        else
        {
            //dropDown.AddOptions(new List<string> { inputField.text + ".." });
            dropDown.gameObject.SetActive(false);
        }
    }
}
