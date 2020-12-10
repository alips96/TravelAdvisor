using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCompleteText : MonoBehaviour
{
    List<string> myList = new List<string>();
    public Dropdown dropDown;
    public InputField inputField;

    private void Start()
    {
        myList.Add("Ali");
        myList.Add("Nariman");
        myList.Add("Ali, Germany");
        myList.Add("Nariman, Iran");
    }
    public void InputValueChanged() //called by dokme
    {
        dropDown.ClearOptions();
        string word = inputField.text;

        if (!string.IsNullOrEmpty(word))
        {
            List<string> found = myList.FindAll(w => w.StartsWith(word));

            if (found.Count > 0)
            {
                dropDown.AddOptions(found);
            }
            else
            {
                dropDown.ClearOptions();
            }
        }
        else
        {
            dropDown.ClearOptions();
        }
    }
}
