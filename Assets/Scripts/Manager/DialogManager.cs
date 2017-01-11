using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum DialogActions
{
	Restart,
	Ok,
	Cancel,
	GoHome
}
public class DialogManager : MonoBehaviour 
{

	public static  DialogManager instance;
	
	public static DialogManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<DialogManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

	private DialogActions dialogActions; 
	public GameObject errorWindow , pauseWindow, gameOverWindow;
	public Text dialogText;
	public Text gameOverText;

	void OnEnable()
	{

	}

	void OnDisable()
	{

	}

	void Start()
	{
		HideWindow();
	}
 
	public void ShowErrorWindow()
	{
		errorWindow.SetActive(true);
	}
	public void ShowErrorWindow(string message,DialogActions actions )
	{
		 errorWindow.SetActive(true);
		 dialogText.text = message;
		 CallBackErrorWindow(actions);
	}

	public void CallBackErrorWindow(DialogActions actions)
	{
		switch(actions)
		{
			case DialogActions.Restart:

			// Restart the current level
			Restart();
			break;

			case DialogActions.Ok:
			//proceed
			break;

			case DialogActions.Cancel:

			// close the dialog
			break;
			case DialogActions.GoHome:
			GoHome();
			// load mainscene
			break;
		}
	}
	
	public void ShowPauseWindow()
	{
		pauseWindow.SetActive(true);
	}
	public void ShowGameOverWindow(string score) 
	{
		gameOverWindow.SetActive(true);
		gameOverText.text = score;
	}

	public void Restart()
	{
		// restart current level
		HideWindow();
		GameManager.Instance.GameRestart();
	}

	public void GamePause()
	{
		GameManager.Instance.GamePause();
		ShowPauseWindow();

	}
	public void Resume()
	{
		HideWindow();
		GameManager.Instance.GameResume();
		
	}

	public void GoHome()
	{
		// go to home level
		HideWindow();
		
	}

	public void ShowWindow()
	{
		errorWindow.SetActive(true);
		gameOverWindow.SetActive(true);
		pauseWindow.SetActive(true);
	}

	public void HideWindow()
	{
		errorWindow.SetActive(false);
		gameOverWindow.SetActive(false);
		pauseWindow.SetActive(false);
	}

}
