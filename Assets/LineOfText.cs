using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineOfText : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> letters = new List<TextMeshProUGUI>();

    public List<TextMeshProUGUI> getList()
    {
        return letters;
    }
}
