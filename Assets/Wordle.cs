using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wordle : MonoBehaviour
{
    [SerializeField] string word;
    [SerializeField] string guess;
    [SerializeField] int attempt;
    [SerializeField] int points;
    [SerializeField] TextMeshProUGUI words, failedWord;
    [SerializeField] List<GameObject> lines = new List<GameObject>();
    [SerializeField] List<result> results = new List<result>();
    [SerializeField] List<List<TextMeshProUGUI>> letterLines = new List<List<TextMeshProUGUI>>();
    [SerializeField] List<TextMeshProUGUI> inputField = new List<TextMeshProUGUI>();
    [SerializeField] TextAsset wordFile;
    [SerializeField] string[] wordList;
    int guessPos;
    bool wordGuessed;
    [SerializeField] bool ranOutOfAttemts;

    
    public enum result
    {
        wrong,
        almost,
        right
    }

    void Awake()
    {
        word = UpdateWord();
        letterLines.Clear();
        for (int i = 0; i < lines.Count; i++)
        {
            letterLines.Add(lines[i].GetComponent<LineOfText>().getList());
        }
    }


    [ContextMenu("Guess")] public void newguess()
    {
        if (ranOutOfAttemts)
        {
            failedWord.text += (word + "\n");
            Clear();
            ranOutOfAttemts = false;
            word = UpdateWord();
            return;
        }
        if (wordGuessed)
        {
            words.text += (word + "\n");
            Clear();
            wordGuessed = false;
            word = UpdateWord();
            return;
        }
        if (guess.Length != word.Length)
        {
            return;
        }
        results.Clear();
        for (int i = 0; i < word.Length; i++)
        {
            if (guess[i] == word[i])
            {
                results.Add(result.right);
            }
            else if (word.IndexOf(guess[i]) != -1)
            {
                results.Add(result.almost);
            }
            else {
                results.Add(result.wrong);
            }
        }
        
        
        DrawWord(); 
        
        if (!wordGuessed){
            attempt++;
        }
        if (attempt >= letterLines.Count){
            ranOutOfAttemts = true;
            //Clear();
                for(int j = 0; j < word.Length; j++)
                {
                    letterLines[attempt-1][j].color = Color.red;
                    letterLines[attempt-1][j].SetText(word[j].ToString());
            }
        }
        ClearInputField();
        
    }

    [ContextMenu("Clear")] public void Clear()
    {
        attempt = 0;
        for (int i = 0; i < letterLines.Count; i++)
        {
            for (int j = 0; j < letterLines[i].Count; j++)
            {
                letterLines[i][j].color = Color.white;
                letterLines[i][j].SetText("*");
            }
        }
    }

    public void ClearInputField()
    {
        guessPos = 0;
        guess = "";
        for (int i = 0; i < inputField.Count; i++)
        {
            inputField[i].color = Color.white;
            inputField[i].SetText("*");
        }
    }

    public void DrawWord()
    {
        int right = 0;
        for (int i = 0; i < results.Count; i++)
        {
            Color color = Color.red;
            if (results[i] == result.almost)
            {
                color = Color.yellow;
            }
            else if (results[i] == result.right)
            {
                color = Color.green;
                right++;
            }
            letterLines[attempt][i].color = color;
            letterLines[attempt][i].SetText(guess[i].ToString());
            if (right == word.Length)
            {
                points++;
                wordGuessed = true;
                ranOutOfAttemts = false;
                return;
            }
        }
    }

    public string UpdateWord()
    {
        string stringWordFile = wordFile.text;
        wordList = stringWordFile.Split('\n');
        string rndWord = wordList[Random.Range(0, wordList.Length-1)];
        if (rndWord.Length != 5)
        {
            Debug.Log("ERROR");
            return "error";
        }
        return rndWord;
    }

    public void inputLetter(string letter)
    {
        if (guess.Length >= word.Length)
        {
            ClearInputField();
            guess = "";
        }

        guess += letter;
        inputField[guessPos].color = Color.green;
        inputField[guessPos].SetText(letter);

        guessPos++;


    }

    
   
}
