using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    string nameText;
    
    
    
    public void TheNameIs()
    {
        nameText = inputField.text;
        Debug.Log("The name is " + nameText);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
