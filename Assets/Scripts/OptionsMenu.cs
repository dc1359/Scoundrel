using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
	public GameObject Menu;
	public GameObject Options;
	private AudioSource ButtonClick;

	void Start () {
		Options =  GameObject.Find("OptionsMenu");
		ButtonClick = GetComponent<AudioSource>();
		Options.SetActive(false);
	}
		
	public void VideoOnClick () {
		Debug.Log("Video");
		ButtonClick.Play();
	}

	public void AudioOnClick () {
		Debug.Log("Audio");
		ButtonClick.Play();
	}

	public void BackOnClick () {
		Debug.Log("Back");
		ButtonClick.Play();
		Options.SetActive(false);
		Menu.SetActive(true);
	}

	public void ExitOnClick () {
		ButtonClick.Play();
		Application.Quit();
	}

}