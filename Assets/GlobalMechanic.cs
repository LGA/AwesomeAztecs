using UnityEngine;
using System.Collections;

public class GlobalMechanic : MonoBehaviour {

	public bool isPaused=false;
	public float vucanoCounter, resourceCounter;
	public GameObject reset, vulcanoOverlay, tribeTimer1, tribeTimer2, tribeTimer3;

	public TextMesh tTime1, tTime2, tBread1, tBread2, tWood1, tWood2, tMetal1, tMetal2;
	public TextMesh tribeTimerText1, tribeTimerText2, tribeTimerText3;

	private const int vulcanoDefault=300, tribeCounterDefault=50;
	private SpriteRenderer srender;
	private Vector3 sacrifice;
	private float lastPressed, playerTimer=60;
	private int activePlayer=0;

	// Use this for initialization
	void Start () {
		vucanoCounter=vulcanoDefault;
		srender = this.GetComponent<SpriteRenderer>();

		sacrifice = new Vector3(Random.Range(1,3), Random.Range(1,3), Random.Range(1,3));
		lastPressed=Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
		bool volcanoPressed=false;
		bool fieldPressed=false;

		if (Input.touchCount > 0 && lastPressed<(Time.time-0.5f)) {

			Touch touch=Input.GetTouch(0);
			
			for (int i = 0; i < Input.touchCount; i++) {
				touch = Input.GetTouch(i);
			}

			Vector3 fingerPos = Camera.main.ScreenToWorldPoint(touch.position);

			if(fingerPos.x < 1.45f && fingerPos.x > -2.6f && 
			   fingerPos.y < 2f && fingerPos.y > -2.1f) {
				volcanoPressed=true;
			} else {
				fieldPressed=true;
			}

			lastPressed=Time.time;
			Debug.Log("Pause:" + isPaused + " - volcanoPressed: " + volcanoPressed + " - fieldPressed: " + fieldPressed);

		}


		if(Mathf.Floor(vucanoCounter)<=0) {

			vulcanoOverlay.SetActive(true);



			if(volcanoPressed || fieldPressed) {
				vulcanoOverlay.SetActive(false);
				sacrifice = new Vector3(Random.Range(1,3), Random.Range(1,3), Random.Range(1,3));
				vucanoCounter=vulcanoDefault;
			}
		}
		else if(isPaused) {
			srender.color = new Color(srender.color.r, srender.color.g, srender.color.b, 0);
			reset.SetActive(true);


				if(volcanoPressed) {
					vucanoCounter=vulcanoDefault;
					isPaused=false;
					sacrifice = new Vector3(Random.Range(1,3), Random.Range(1,3), Random.Range(1,3));
					reset.SetActive(false);
				}
				if(fieldPressed) {
					isPaused=false;
					reset.SetActive(false);
				}

		}

		else {
			isPaused=volcanoPressed;
			vucanoCounter-=Time.deltaTime;
			resourceCounter+=Time.deltaTime;


			if(resourceCounter>20) {
				resourceCounter=0;
				sacrifice = new Vector3(sacrifice.x+Random.Range(0,2), sacrifice.y+Random.Range(0,2), sacrifice.z+Random.Range(0,2));	
			}

			updateTime();

			playerTimer-=Time.deltaTime;

			if(fieldPressed) {
				playerTimer=0;
			}

			if(Mathf.Floor(playerTimer) <=0) {
				playerTimer=tribeCounterDefault;
				activePlayer=(activePlayer+1)%3;
			}

			updatePlayerStatus();
		}

	}

	private void updatePlayerStatus() {

		bool harvestPhase=true;

		if(playerTimer<(tribeCounterDefault/2)) {
			harvestPhase=false;
		}

		switch (activePlayer)
		{
		case 0:
			tribeTimer1.SetActive(harvestPhase);
			tribeTimer2.SetActive(false);
			tribeTimer3.SetActive(false);
			tribeTimerText1.text=Mathf.Floor(playerTimer).ToString();
			tribeTimerText2.text="";
			tribeTimerText3.text="";
			break;
		case 1:
			tribeTimer1.SetActive(false);
			tribeTimer2.SetActive(harvestPhase);
			tribeTimer3.SetActive(false);
			tribeTimerText2.text=Mathf.Floor(playerTimer).ToString();
			tribeTimerText1.text="";
			tribeTimerText3.text="";
			break;
		default:
			tribeTimer1.SetActive(false);
			tribeTimer2.SetActive(false);
			tribeTimer3.SetActive(harvestPhase);
			tribeTimerText3.text=Mathf.Floor(playerTimer).ToString();
			tribeTimerText2.text="";
			tribeTimerText1.text="";
			break;
		}

	}

	private void updateTime() {

		float min = Mathf.Floor(vucanoCounter/60);
		float sec = Mathf.Floor(vucanoCounter%60);

		string time = min.ToString() + ":" + sec.ToString();

		tTime1.text = time;
		tTime2.text = time;
		tBread1.text = sacrifice.x.ToString();
		tBread2.text = sacrifice.x.ToString();
		tWood1.text = sacrifice.y.ToString();
		tWood2.text = sacrifice.y.ToString();
		tMetal1.text = sacrifice.z.ToString();
		tMetal2.text = sacrifice.z.ToString();



		srender.color = new Color(srender.color.r, srender.color.g, srender.color.b, 1-(vucanoCounter/vulcanoDefault));

	}

}
