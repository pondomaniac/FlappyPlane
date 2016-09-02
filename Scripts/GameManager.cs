using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameManager : MonoBehaviour {
	// The score that the player currently has
	[HideInInspector]
	public int curScore; 
	// The highest score the player has reached (saved)
	private int highscore;
	// Reference to our custom gui skin
	public GUISkin skin;
	// Values defining the width and height of our game over screen
	public Vector2 losePromptWH;
	// Boolean to check if we need to end the game or not
	[HideInInspector]
	public bool showGameOver;
	[HideInInspector]
	public bool isPaused = false ;
	[HideInInspector]
	public bool isEnded = false ;
	//Tutorial
	public GameObject _Tutorial;
	public GameObject _PauseButton;
	public GameObject _ReloadButton;
	public GameObject _PlayButton;
	public GameObject _MenuButton;
	public GameObject _PausedBackground;
	float timing = 0; 
	public GameObject _ScoreBoard;
	public AudioClip MenuSound;
	public GameObject _CurrentScore;
	public GameObject _BestScore;
	public AudioClip TimeUpSound;
	public AudioClip EndSound;


	void Start () {
		// Grab the last saved highscore from the player prefs file
		highscore = PlayerPrefs.GetInt("Highscore");
		StartCoroutine(Init ());
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		//Detecting if the player clicked on the left mouse button and also if there is no animation playing
		if (Input.GetButtonDown ("Fire1")) {
						//The 3 following lines is to get the clicked GameObject and getting the RaycastHit2D that will help us know the clicked object
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
						if (hit.transform != null) {
								if (hit.transform.gameObject.name == _MenuButton.name) {
										audio.PlayOneShot (MenuSound);
										hit.transform.localScale = new Vector3 (1.1f, 1.1f, 0);
				
										Application.LoadLevel ("MainMenu");
								}
								if (hit.transform.gameObject.name == _ReloadButton.name) {
										audio.PlayOneShot (MenuSound);
										Time.timeScale = 1;
										//isPaused = false;
										//HOTween.Play ();
										hit.transform.localScale = new Vector3 (1.1f, 1.1f, 0);
										Application.LoadLevel (Application.loadedLevelName);
								}
								if (hit.transform.gameObject.name == _PauseButton.name && !isPaused && !isEnded && HOTween.GetTweenersByTarget (_PlayButton.transform, false).Count == 0 && HOTween.GetTweenersByTarget (_MenuButton.transform, false).Count == 0) {
										audio.PlayOneShot (MenuSound);
										StartCoroutine (ShowMenu ());
										hit.transform.localScale = new Vector3 (1.1f, 1.1f, 0);
								} else if ((hit.transform.gameObject.name == _PauseButton.name || hit.transform.gameObject.name == _PlayButton.name) && !isEnded && isPaused && HOTween.GetTweenersByTarget (_PlayButton.transform, false).Count == 0 && HOTween.GetTweenersByTarget (_MenuButton.transform, false).Count == 0) {
										audio.PlayOneShot (MenuSound);
										StartCoroutine (HideMenu ());
										hit.transform.localScale = new Vector3 (1f, 1f, 0);
								}
						}
				}
		// If the bird died and our current score is greater than our saved highscore
		if (showGameOver && curScore > highscore)
		{
			// Set the highscore to our current score
			highscore = curScore;
			// Now save the score as our new highscore
			PlayerPrefs.SetInt("Highscore", highscore);
		}
	}

	void OnGUI()
	{
		// Set the GUI's skin to our custom skin
		GUI.skin = skin;
		// Show our current score value at the top center of the screen 
		// (note: it uses the custom Score style in our skin)
		GUI.Label (new Rect(Screen.width/2 - 100,10f,200,200), curScore.ToString(), skin.GetStyle("Score"));

		// If the bird died, show the game over screen
		if (showGameOver && !isEnded)
		{
			StartCoroutine(ShowBoardScore());
			showGameOver= false;
			isEnded=true;
		}
	}

	IEnumerator ShowMenu()
	{  	 isPaused=true ; 
		HOTween.Pause() ;
		audio.Pause();
		
		TweenParms parms = new TweenParms().Prop("position", new Vector3(_PausedBackground.transform.position.x,0.5f,-5)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_PausedBackground.transform, 0.05f, parms).WaitForCompletion();
		
		parms = new TweenParms().Prop("position", new Vector3(_PlayButton.transform.position.x,0.5f,-6)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_PlayButton.transform, 0.07f, parms).WaitForCompletion();
		
		parms = new TweenParms().Prop("position", new Vector3(_ReloadButton.transform.position.x,0.5f,-6)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_ReloadButton.transform, 0.09f, parms).WaitForCompletion();
		
		parms = new TweenParms().Prop("position", new Vector3(_MenuButton.transform.position.x,0.5f,-6)).Ease(EaseType.EaseOutQuart);
		yield return StartCoroutine(HOTween.To(_MenuButton.transform, 0.1f, parms).WaitForCompletion());
		
		
		Time.timeScale=0;
		
		
	}
	IEnumerator HideMenu()
	{Time.timeScale=1; HOTween.Play();
		
		TweenParms parms = new TweenParms().Prop("position", new Vector3(_PausedBackground.transform.position.x,16,-5)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_PausedBackground.transform, 0.6f, parms).WaitForCompletion();
		
		parms = new TweenParms().Prop("position", new Vector3(_PlayButton.transform.position.x,16,-6)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_PlayButton.transform, 0.4f, parms).WaitForCompletion();
		audio.Play();
		
		parms = new TweenParms().Prop("position", new Vector3(_ReloadButton.transform.position.x,16,-6)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_ReloadButton.transform, 0.5f, parms).WaitForCompletion();
		
		
		parms = new TweenParms().Prop("position", new Vector3(_MenuButton.transform.position.x,16,-6)).Ease(EaseType.EaseOutQuart);
		yield return StartCoroutine(HOTween.To(_MenuButton.transform, 0.2f, parms).WaitForCompletion());
		isPaused=false;
	}

	IEnumerator ShowBoardScore()
	{    
		audio.Stop ();
		audio.PlayOneShot(TimeUpSound);
		yield return new WaitForSeconds(0.5f);
		audio.PlayOneShot (EndSound);
		(_BestScore.GetComponent (typeof(TextMesh))as TextMesh).text = ""+PlayerPrefs.GetInt ("Highscore");
		(_CurrentScore.GetComponent (typeof(TextMesh))as TextMesh).text = ""+curScore;
		yield return new WaitForSeconds(1);
		TweenParms parms = new TweenParms().Prop("position", new Vector3(_ScoreBoard.transform.position.x,1f,_ScoreBoard.transform.position.z)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_ScoreBoard.transform, 0.5f, parms);
		parms = new TweenParms().Prop("position", new Vector3(_MenuButton.transform.position.x,-2.5f,-8)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_MenuButton.transform, 0.7f, parms).WaitForCompletion();
		parms = new TweenParms().Prop("position", new Vector3(_ReloadButton.transform.position.x,-2.5f,-8)).Ease(EaseType.EaseOutQuart);
		HOTween.To(_ReloadButton.transform, 0.9f, parms).WaitForCompletion();
	}

	IEnumerator Init ()
	{ isPaused = true;Time.timeScale=0;
		if (PlayerPrefs.GetInt ("Tutorial") != 1) {
			var isOkay = false;
			
			while (isOkay==false) { 
				if (Input.GetButtonDown ("Fire1")) {
					isOkay = true;
				Time.timeScale=1;
					TweenParms parms = new TweenParms ().Prop ("localPosition", new Vector3 (100, 0, -10)).Ease (EaseType.EaseOutQuart);
					HOTween.To (_Tutorial.transform, 3f, parms);
					PlayerPrefs.SetInt ("Tutorial", 1);
					
				}
				yield return 0;	
			}
			
		} else {
			Time.timeScale=1;
			_Tutorial.transform.localPosition = new Vector3 (100, 0, -10);
			
		}
		isPaused = false;
		}
}
