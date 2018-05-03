using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
	public GameObject Menu;
	public GameObject Options;
	public Button Inventory;
	public GameObject Volume;
	public Slider VolumeSlider;
	private AudioSource ButtonClick;
	private bool isVolumeActive;
	public static float VolumeLevel = 100;
	public Text VolumeText;

	void Start () {
		Options =  GameObject.Find("OptionsMenu");
		ButtonClick = GetComponent<AudioSource>();
		Options.SetActive(false);
		Volume.SetActive(false);
		isVolumeActive = false;
		VolumeSlider.value = VolumeLevel;
	}
		
	public void VideoOnClick () {
		Debug.Log("Video");
		ButtonClick.Play();
	}

	public void AudioOnClick () {
		ButtonClick.Play();

		if (isVolumeActive) 
		{
			Volume.SetActive(false);
			isVolumeActive = false;
		} 
		else 
		{
			Volume.SetActive(true);
			isVolumeActive = true;
		}
		Debug.Log(VolumeSlider.value);
		VolumeLevel = VolumeSlider.value;
	}

	public void BackOnClick () {
		Debug.Log("Back");
		ButtonClick.Play();
		isVolumeActive = false;
		Volume.SetActive(false);
		Options.SetActive(false);
		Menu.SetActive(true);
		Inventory.Select();
	}

	// Sets the volume to whatever percentage the slider is at
	void Update () {
		VolumeText.text = VolumeSlider.value.ToString();
		AudioListener.volume = VolumeLevel / 100;
	}

	public void ExitOnClick () {
		ButtonClick.Play();
		Application.Quit();
	}

}