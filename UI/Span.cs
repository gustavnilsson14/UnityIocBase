using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Span : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text value;
    public Func<object, string> getValue;
    public void SetFontSize(float size)
    {
        title.enableAutoSizing = false;
        value.enableAutoSizing = false;
        title.fontSize = size;
        value.fontSize = size;
    }
}
