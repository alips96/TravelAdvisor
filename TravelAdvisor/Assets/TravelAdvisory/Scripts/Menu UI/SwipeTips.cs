using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        SetInitialReferences();
        SetItemsLength();
        FilloutPosValues();

        if (posLength > 8)
        {
            imageContent.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
        }

        InstantiateTipsObjects();
    }

    private void SetInitialReferences()
    {
        //resultScript = GameObject.Find("ResultsMenu").GetComponent<Result>();
        byte b = resultScript.overallStatusIndex;
        Debug.Log(b);
    }

    private void SetItemsLength()
    {
        pos = new float[4];
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

    private void InstantiateTipsObjects()
    {
        for (int i = 0; i < posLength; i++)
        {
            Instantiate(Resources.Load("Prefabs/Tip"), transform);
            Button indexButton = Instantiate(buttonPrefab, imageContent);
            int x = i; //Closure Problem :/ why don't they fix this??
            indexButton.onClick.AddListener(delegate { WhichBtnClicked(x); });
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