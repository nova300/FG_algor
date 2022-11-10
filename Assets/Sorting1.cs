using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting1 : MonoBehaviour
{
    [SerializeField] int[] thingsToSort;

    [ContextMenu("Sorting / Bubble Sort")]
    void BubbleSort(){
        BubbleSort(thingsToSort);
    }

    void BubbleSort(int[] intergers)
    {

        bool swapped = true;
        int n = intergers.Length;
        for (int j = 0; j < n && swapped; j++)
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if (intergers[i-1] > intergers[i])
                {
                    (intergers[i-1], intergers[i]) = (intergers[i], intergers[i-1]);
                    swapped = true;
                }
            }
        }
    }

    [ContextMenu("Sorting / Unsort")]
    void Unsort(){
        Unsort(thingsToSort);
    }

    void Unsort(int[] input)
    {
        int n = input.Length;
        for (int i = 0; i < n; i++)
        {
            input[i] = Random.Range(1, 999);
        }
    }
}
