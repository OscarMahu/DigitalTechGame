using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   
    // Setting public Variables which can be assigned in unity to different Objects / UI
    public static GameController instance;
    public GameObject objectiveContainer, hudContainer, gameOverScreen;
    public Text objectiveCounter, timeCounter, endTime, countdownText;

    // Setting up other Variabels (e.g. GamePlaying which can be read anywhere but only modified in this file)
    public bool gamePlaying { get; private set; }
    public int countdownTime;
    private int numTotalobjectives, numCollectedObjectives;
    private float startTime, elapsedTime;

    TimeSpan timePlaying;

    private void Awake()
    {
        instance = this;
    }
    // Start is called when the game first loads, in this function I hide away the gameOver screen and ensure that the hud is showing. 
    // After that I then check how many objectives I have in my game and set my numTotalobjectives variable to that, I reset my numCollectedObjectives to 0, I then setup my objectivecounter text,
    // set gamePlaying to false so the user cant move during the countdown and start the coroutine countdown.
    private void Start()
    {   
        gameOverScreen.SetActive(false);
        hudContainer.SetActive(true);
        numTotalobjectives = objectiveContainer.transform.childCount;
        numCollectedObjectives = 0;
        objectiveCounter.text = "Plastic: 0 / " + numTotalobjectives;
        gamePlaying = false;
        StartCoroutine(CountdownToStart());
    }

    // After the countdown has finished Begin Game is called which sets the gamePlaying boolean to be true which lets the player move. 
    private void BeginGame()
    {
        gamePlaying = true;
        // Time.time starts at the start of the game is zero but increments with time, to remove any delay we set our startTime variable to Time.time
        startTime = Time.time;
    }

    // Whilst the game is being played we find the difference between the startTime and Time.time to find their elapsed time or how long the player has been playing the game for,
    // then after that we set convert our elapsed time into our TimeSpan variable called timePlaying which then allows us to format it and make a string which you can see in the line below,
    // On the final line I set the timeCounter.text which is the timer displayed in my UI to the timePlayingString which is the text that I want to be displayed. 
    private void Update()
    {   
        if(gamePlaying)
        elapsedTime = Time.time - startTime;
        timePlaying = TimeSpan.FromSeconds(elapsedTime);
        string timePlayingString = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
        timeCounter.text = timePlayingString;
    }

    // This is called whenever a objective is capured, firstly I add 1 to numCollectedObjectives, then I update and apply the objectivecounterstring to the objectivecountertext,
    // I also check for if the num of collected objectives is greater or equal to the num total of objectives and if it is then I call the EndGame function which stops the game and opens the game over screen.
    public void CaptureObjective()
    {
        numCollectedObjectives ++;
        string objectiveCounterString = "Plastic: " + numCollectedObjectives + " / " + numTotalobjectives; 
        objectiveCounter.text = objectiveCounterString;

        if(numCollectedObjectives >= numTotalobjectives)
        {
            EndGame();
        }
    }

    // When the game is completed this function is called I firstly stop player movement and then show the game over screen after that.
    private void EndGame()
    {
        gamePlaying = false;
        Invoke("ShowGameOverScreen", 0.5f);
    }

    // When Show game over screen is called I firstly show the game over screen, hide the hud and then I make the endTime variable which is then displayed on the users screen to see how quick they could clean up the plastic. 
    private void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        hudContainer.SetActive(false);
        endTime.text = timeCounter.text;
    }

    // This function is dedicated to the countdown which is called the start of my game, firstly I check that the countdown is greater than 0 if it is then the displayed countdown text is equal to the countdown time which is set to three in unity
    // It then waits for 1 second before subtracting 1 from the total of countdown time before returning to the start of the function.
    // Once countdown has finished the BeginGame function is called allowing the player to start, then Start! is printed on the players screen and after 1s it is removed.
    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        BeginGame();
        countdownText.text = "Start!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
}
