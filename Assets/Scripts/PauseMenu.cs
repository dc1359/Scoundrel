using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PauseMenu : MonoBehaviour {
	public GameObject Menu;
	public GameObject Options;
	public Button Video;
	private AudioSource ButtonClick;

	void Start () {
		Menu =  GameObject.Find("PauseMenu");
		ButtonClick = GetComponent<AudioSource>();
		Menu.SetActive(false);
	}

	public void InventoryOnClick () {
		Debug.Log("Inventory");
		ButtonClick.Play();
	}

	public void OptionsOnClick () {
		Debug.Log("Options");
		Options.SetActive(true);
		ButtonClick.Play();
		Menu.SetActive(false);
		Video.Select();
	}

	public void RestartOnClick () {
		ButtonClick.Play();
		SceneManager.LoadScene("Test");
		Menu.SetActive(false);
		Time.timeScale = 1;
	}

	public void ExitOnClick () {
		ButtonClick.Play();
		Application.Quit();
	}

}