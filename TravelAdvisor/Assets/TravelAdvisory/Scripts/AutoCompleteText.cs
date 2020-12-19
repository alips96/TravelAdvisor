using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCompleteText : MonoBehaviour
{
    List<string> myList = new List<string>();
    [SerializeField] private Dropdown dropDown;
    [SerializeField] private InputField inputField;
    [SerializeField] private Country countrySO;

    private void Start()
    {
        myList = countrySO.AllRegions;
    }

    public void InputValueChanged() //called by InputText UI
    {
        dropDown.ClearOptions();

        string word = inputField.text;

        if (!string.IsNullOrEmpty(word))
        {
            List<string> found = myList.FindAll(w => w.StartsWith(word));

            if (found.Count > 0)
            {
                dropDown.AddOptions(new List<string> { inputField.text + ".." });
                dropDown.AddOptions(found);
            }
        }
        else
        {
            dropDown.AddOptions(new List<string> { inputField.text + ".." });
        }
    }
}
