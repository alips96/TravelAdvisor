﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Transform scrollbar, imageContent;
    private float scroll_pos = 0;
    private float[] pos;
    private Button takeTheBtn;
    private int btnNumber;

    private float distance;
    private int posLength;

    private void Start()
    {
        pos = new float[transform.childCount];
        posLength = pos.Length;
        distance = 1f / (posLength - 1f);

        for (int i = 0; i < posLength; i++)
        {
            pos[i] = distance * i;
        }

        if(posLength > 8)
        {
            imageContent.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
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
                //Debug.Log("Current Selected Image" + i);
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);

                Transform childButton = imageContent.GetChild(i);
                childButton.localScale = Vector2.Lerp(imageContent.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                //childButton.GetComponent<Image>().color = new Color(1.0f, 0.64f, 0.0f);
                childButton.GetComponent<Image>().color = Color.yellow;

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

    private void SmallButtonHandler(float distance, float[] pos, Button btn)
    {
        for (int i = 0; i < posLength; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);
            }
        }

        int childCount = btn.transform.parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            btn.transform.name = ".";
        }

    }
    public void WhichBtnClicked(Button btn)
    {
        btn.transform.name = "clicked";
        int childCount = btn.transform.parent.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                btnNumber = i;
                takeTheBtn = btn;
                scroll_pos = pos[btnNumber];

                SmallButtonHandler(distance, pos, takeTheBtn);
            }
        }
    }

}