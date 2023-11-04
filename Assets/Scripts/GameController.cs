using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
   [SerializeField]GameObject[] hangmanObjects;
   [SerializeField]Transform wordTransform;
   [SerializeField]Transform keyboardTransform;
   [SerializeField]GameObject letterBase;
   [SerializeField]TextAsset gameWords;


   private String word;
   private int incorrectGuesses = 0 , correctGuesses = 0;
   public void Start()
   {
      InstantiateButtons();
      InitializeGame();
   }

   void InitializeGame()
   {
      incorrectGuesses = 0;
      correctGuesses = 0;
      foreach(Button keyboardButton in keyboardTransform.GetComponentsInChildren<Button>())
      {
         keyboardButton.interactable = true;
      }
      foreach(Transform wordLetters in wordTransform.GetComponentInChildren<Transform>())
      {
         Destroy(wordLetters.gameObject);
      }
      foreach(GameObject stage in hangmanObjects)
      {
         stage.SetActive(false);
      }

      word = GenerateWord().ToUpper();
      foreach(char letter in word)
      {
         GameObject temp = Instantiate(letterBase, wordTransform);
         temp.GetComponentInChildren<TextMeshProUGUI>().text = "";
      }
   }

   void InstantiateButtons()
   {
      for(int i = 65; i < 91 ; i++)
      {
         GameObject currLetter = GameObject.Instantiate(letterBase, keyboardTransform);
         currLetter.GetComponentInChildren<TextMeshProUGUI>().text = ((char)i).ToString();
         String tempStr = ((char)i).ToString();
         currLetter.GetComponent<Button>().onClick.AddListener( 
            delegate 
            { 
               CheckLetter(tempStr);
            });
      }
   }


   String GenerateWord()
   {
      String[] wordList = gameWords.text.Split("\n");
      String line = wordList[UnityEngine.Random.Range(0, wordList.Length -1)];
      return line.Substring(0, line.Length -1);
   }

   void CheckLetter(String inputLetter)
   {
      Boolean isLetterInWord = false; 
      for(int i = 0; i < word.Length; i++)
      {
         if(word[i] == inputLetter.ToCharArray()[0])
         {
            isLetterInWord = true;
            correctGuesses ++;
            wordTransform.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter.ToString();
         }
      }

      if(!isLetterInWord)
      {
         incorrectGuesses++;
         hangmanObjects[incorrectGuesses - 1].SetActive(true);
      }
      CheckGameState();
   }


   void CheckGameState()
   {
      if(correctGuesses == word.Length)
      {
         foreach (TextMeshProUGUI temp in wordTransform.GetComponentsInChildren<TextMeshProUGUI>())
         {
            temp.color = Color.green;
         }
         Invoke("InitializeGame" , 3f);
      }

      if(incorrectGuesses == hangmanObjects.Length)
      {
         int i = 0;
         foreach (TextMeshProUGUI temp in wordTransform.GetComponentsInChildren<TextMeshProUGUI>())
         {
            temp.color = Color.red;
            temp.text = word[i].ToString();
            i++;
         }
         Invoke("InitializeGame" , 3f);
      }
   }
}
