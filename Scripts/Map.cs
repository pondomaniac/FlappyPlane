using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	// The amount of spacing between pipes
	public float obstacleSpacing;
	// Score:loadThreshold ratio required before generating more pipes
	public int loadThreshold;
	// The amount of pipes to generate at the start of the game
	public int startingObstacleCount;
	// The speed at which the map will be moving to the left
	public float moveSpeed;
	// References to the Obstacle objects and the Ground objects
	public GameObject obstacleObject;
	public GameObject groundObject;
	// Reference to the Game Manager class
	public GameManager manager;

	// Generic Lists that will hold all of our ground and obstacle pieces so we can keep track
	private List<GameObject> obstacles = new List<GameObject>();
	private List<GameObject> ground = new List<GameObject>();


	void Start () {

		// Create a few obstacles at the start
		for (int i = 0; i < startingObstacleCount; i++)
		{
			// Instantiate an obstacle based on the loop's index with offsets
			GameObject InsObject = (GameObject)Instantiate(obstacleObject, new Vector2((float)i * obstacleSpacing + 15f, (float)Random.Range(-10, -5)), Quaternion.identity);
			// Set the object's parent to the Map object
			InsObject.transform.parent = this.transform;
			// Add the created obstacle to a generic list that we can cycle through later to find the last object
			obstacles.Add(InsObject);
		}
		// Create a few ground pieces 
		for (int i = 0; i < 30; i++)
		{
			// Instantiate a ground piece based on the loop's index with offsets
			GameObject InsObject = (GameObject)Instantiate(groundObject, new Vector2((float)(i + obstacleSpacing) - 8f, -8f), Quaternion.identity);
			InsObject.transform.parent = this.transform;
			ground.Add(InsObject);
		}
		rigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Move the map pieces to the left at a constand velocity (we don't move the bird right)

	}

	public void Generate()
	{
		GenerateObstacles();
		GenerateGround();

	}

	// The method that generates our obstacles 
	void GenerateObstacles()
	{
	
		// Instantiate obstacles based on the position of the last obstacle in the obstacles list
		GameObject InsObject = (GameObject)Instantiate(obstacleObject, new Vector2(obstacles[obstacles.Count - 1].transform.position.x + obstacleSpacing, (float)Random.Range(-10, -5)), Quaternion.identity);
		InsObject.transform.parent = this.transform;
		obstacles.Add (InsObject);
		if (manager.curScore > 2)
		{
			Destroy(obstacles[0]);
			obstacles.Remove(obstacles[0]);
		}
	}

	// The method that generates our ground
	void GenerateGround()
	{
		// Instantate the ground pieces based on the last piece in the ground list
		for(int i = 0; i < 4; i++)
		{
			GameObject InsObject = (GameObject)Instantiate(groundObject, new Vector2((float)(ground[ground.Count - 1].transform.position.x + 1), -8f), Quaternion.identity);
			//GameObject InsObject = (GameObject)Instantiate(groundObject, new Vector2((float)(i + obstacleSpacing) - 10f, -8f), Quaternion.identity);
			InsObject.transform.parent = this.transform;
			ground.Add(InsObject);
			Destroy(ground[0]);
			ground.Remove(ground[0]);
		}

	}
}
