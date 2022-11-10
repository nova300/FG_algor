using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzBuzz : MonoBehaviour
{
    [SerializeField] int limit = 100;
    [SerializeField] List<string> fizzbuzzlist;
    [ContextMenu("Run FizzBuzz")]
    void Run(){
        fizzbuzzlist.Clear();
        for(int i = 0; i < limit; i++){
            Debug.Log(FizzBuzzifier(i));
            fizzbuzzlist.Add(FizzBuzzifier(i));
        }
    }


    string FizzBuzzifier(int n)
    {
        if (n % 15 == 0)
        {
            return " FizzBuzz";
        } 
        else if (n % 5 == 0)
        {
            return " Buzz";
        }
        else if (n % 3 == 0)
        {
            return " Fizz";
        }

        return "" + n;
    }
}
