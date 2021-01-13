using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

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
        dropDown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
    }

    public void InputValueChanged() //called by InputText UI
    {
        dropDown.ClearOptions();

        string word = ConvertToStandardForm(inputField.text);

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
            dropDown.gameObject.SetActive(false);
        }
    }

    private string ConvertToStandardForm(string text)
    {
        if (string.IsNullOrEmpty(text))
            return null;

        string oldString = text.Trim();

        StringBuilder newText = new StringBuilder(oldString);
        newText.Replace(oldString[0], char.ToUpper(oldString[0]), 0, 1);

        return newText.ToString();
    }
}
