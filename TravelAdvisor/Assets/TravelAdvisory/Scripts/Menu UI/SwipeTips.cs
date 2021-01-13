using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeTips : MonoBehaviour
{
    [SerializeField] private Transform scrollbar, imageContent;

    private float scroll_pos = 0;
    private float[] pos;

    private float distance;
    private int posLength;

    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Result resultScript;

    private byte statusIndex = 5;

    [SerializeField] private Tip[] tipsPriority1;
    [SerializeField] private Tip[] tipsPriority2;
    [SerializeField] private Tip[] tipsPriority3;
    [SerializeField] private Tip[] tipsPriority4;

    public void ShowTips() //Called by tips button
    {
        if (resultScript.overallStatusIndex != statusIndex)
        {
            RemovePreviousObjects();

            statusIndex = resultScript.overallStatusIndex;

            switch (statusIndex)
            {
                case 0:
                    SetItemsLength(tipsPriority1.Length);
                    FilloutPosValues();
                    InstantiateTipsObjects(new List<Tip[]> { tipsPriority1 });
                    break;

                case 1:
                    SetItemsLength(tipsPriority1.Length + tipsPriority2.Length);
                    FilloutPosValues();
                    InstantiateTipsObjects(new List<Tip[]> { tipsPriority1, tipsPriority2 });
                    break;

                case 2:
                    SetItemsLength(tipsPriority1.Length + tipsPriority2.Length + tipsPriority3.Length);
                    FilloutPosValues();
                    InstantiateTipsObjects(new List<Tip[]> { tipsPriority1, tipsPriority2, tipsPriority3 });
                    break;

                case 3:
                    SetItemsLength(tipsPriority1.Length + tipsPriority2.Length + tipsPriority3.Length + tipsPriority4.Length);
                    FilloutPosValues();
                    InstantiateTipsObjects(new List<Tip[]> { tipsPriority1, tipsPriority2, tipsPriority3, tipsPriority4 });
                    break;
            }

            if (posLength > 8)
            {
                imageContent.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
            }
        }
    }

    private void RemovePreviousObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform item in imageContent)
        {
            Destroy(item.gameObject);
        }
    }

    private void SetItemsLength(int length)
    {
        pos = new float[length];
        posLength = pos.Length;
    }

    private void FilloutPosValues()
    {
        distance = 1f / (posLength - 1f);

        for (int i = 0; i < posLength; i++)
        {
            pos[i] = distance * i;
        }
    }

    private void InstantiateTipsObjects(List<Tip[]> myList)
    {
        int i = 0;

        foreach (Tip[] tipArr in myList)
        {
            foreach (Tip item in tipArr)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/Tip"), transform) as GameObject;
                go.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
                go.transform.GetChild(1).GetComponent<TMP_Text>().text = item.title;
                go.transform.GetChild(2).GetComponent<TMP_Text>().text = item.content;

                Button indexButton = Instantiate(buttonPrefab, imageContent);
                int x = i++; //Closure Problem
                indexButton.onClick.AddListener(delegate { WhichBtnClicked(x); });
            }
        }
    }

    void Update()
    {
        float scrollbarValue = scrollbar.GetComponent<Scrollbar>().value;

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbarValue;
        }
        else
        {
            for (int i = 0; i < posLength; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbarValue, pos[i], 0.1f);
                }
            }
        }


        for (int i = 0; i < posLength; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);

                Transform childButton = imageContent.GetChild(i);
                childButton.localScale = Vector2.Lerp(imageContent.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                childButton.GetComponent<Image>().color = new Color(1.0f, 0.72f, 0f);

                for (int j = 0; j < posLength; j++)
                {
                    if (j != i)
                    {
                        Transform smallButton = imageContent.GetChild(j);
                        Transform childTransform = transform.GetChild(j);

                        smallButton.GetComponent<Image>().color = Color.grey;
                        smallButton.localScale = Vector2.Lerp(smallButton.localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        childTransform.localScale = Vector2.Lerp(childTransform.localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    private void SmallButtonHandler(float distance, float[] pos, int buttonIndex)
    {
        for (int i = 0; i < posLength; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[buttonIndex], 1f * Time.deltaTime);
            }
        }
    }

    public void WhichBtnClicked(int i)
    {
        scroll_pos = pos[i];
        SmallButtonHandler(distance, pos, i);
    }

}