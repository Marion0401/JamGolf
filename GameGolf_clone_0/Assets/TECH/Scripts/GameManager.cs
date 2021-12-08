//-----------------------------------------------------------------------------------------------------------------
// 
//	Mushroom Example
//	Created by : Luis Filipe (filipe@seines.pt)
//	Dec 2010
//
//	Source code in this example is in the public domain.
//  The naruto character model in this demo is copyrighted by Ben Mathis.
//  See Assets/Models/naruto.txt for more details
//
//-----------------------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private Connection pioconnection;
	private List<Message> msgList = new List<Message>(); //  Messsage queue implementation
	public static GameManager instance;
	public BallMover currBallMover;
	public GameObject playerPrefab = null;
	public GameObject uiPowerBar = null;
	public GameObject myBall;
	private GameObject enemyBall;
	public CameraController cameraController = null;
	public Transform[] startPoins = new Transform[0];

	private int levelIndex = 0;

	public Team _clientTeam;

	// UI stuff
	private string infomsg = "";

	#region OnEnable / OnDisable
	private void OnEnable()
	{
		EndRound.BallInHole += IsEndMap;
	}

	private void OnDisable()
	{
		EndRound.BallInHole -= IsEndMap;
	}

	#endregion

	public void Awake()
    {
		instance = this;
    }
	void Start() {
		Application.runInBackground = true;

		// Create a random userid 
		System.Random random = new System.Random();
		string userid = "Guest" + random.Next(0, 10000);

		Debug.Log("Starting");

		PlayerIO.Authenticate(
			"golfgame-izebw30wzukbgaq1zovezw",            //Your game id
			"public",                               //Your connection id
			new Dictionary<string, string> {        //Authentication arguments
				{ "userId", userid },
			},
			null,                                   //PlayerInsight segments
			delegate (Client client) {
				Debug.Log("Successfully connected to Player.IO");
				infomsg = "Successfully connected to Player.IO";

				Debug.Log("Create ServerEndpoint");
				// Comment out the line below to use the live servers instead of your development server
				client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

				Debug.Log("CreateJoinRoom");
				//Create or join the room 
				client.Multiplayer.CreateJoinRoom(
					"UnityDemoRoom",                    //Room id. If set to null a random roomid is used
					"UnityMushrooms",                   //The room type started on the server
					true,                               //Should the room be visible in the lobby?
					null,
					null,
					delegate (Connection connection) {
						Debug.Log("Joined Room.");
						infomsg = "Joined Room.";
						// We successfully joined a room so set up the message handler
						pioconnection = connection;
						pioconnection.OnMessage += handlemessage;
						
					},
					delegate (PlayerIOError error) {
						Debug.Log("Error Joining Room: " + error.ToString());
						infomsg = error.ToString();
					}
				);
			},
			delegate (PlayerIOError error) {
				Debug.Log("Error connecting: " + error.ToString());
				infomsg = error.ToString();
			}
		);
	}

	void handlemessage(object sender, Message m) {
		msgList.Add(m);
	}

	void FixedUpdate() {
		// process message queue
		foreach (Message m in msgList) {
			switch (m.Type) 
			{
				case "Move":
					Vector3 direction = new Vector3(m.GetFloat(0), m.GetFloat(1), m.GetFloat(2));
					MoveBall(direction, m.GetFloat(3));
					break;

				case "StartGame":
					break;

				case "InitializeGame":
					_clientTeam = (Team)m.GetInt(0);

					if(m.GetInt(0) == 0)
                    {
						myBall = Instantiate(playerPrefab, startPoins[levelIndex].position, Quaternion.identity);
						myBall.GetComponent<PlayerIdentity>().SetTeam((Team)0);
						myBall.GetComponent<BallMover>()._powerBarImg = uiPowerBar.GetComponent<Image>();

						enemyBall = Instantiate(playerPrefab, startPoins[levelIndex].position, Quaternion.identity);
						enemyBall.GetComponent<PlayerIdentity>().SetTeam((Team)1);
						currBallMover = enemyBall.GetComponent<BallMover>();
					}
					else if(m.GetInt(0) == 1)
                    {
						myBall = Instantiate(playerPrefab, startPoins[levelIndex].position, Quaternion.identity);
						myBall.GetComponent<PlayerIdentity>().SetTeam((Team)1);
						myBall.GetComponent<BallMover>()._powerBarImg = uiPowerBar.GetComponent<Image>();


						enemyBall = Instantiate(playerPrefab, startPoins[levelIndex].position, Quaternion.identity);
						enemyBall.GetComponent<PlayerIdentity>().SetTeam((Team)0);
						currBallMover = enemyBall.GetComponent<BallMover>();
					}
					break;

				case "CameraChange":
					if(m.GetInt(0) == (int)_clientTeam)
                    {
						cameraController.SetBallForCamera(myBall);
                    }
                    else
                    {
						cameraController.SetBallForCamera(enemyBall);
					}
					break;

				case "CanPlay":
					myBall.GetComponent<BallMover>()._canPlay = true;
					break;

				case "AdjustBallPos":
					enemyBall.GetComponent<BallMover>().MoveBallAtWorldPos(new Vector3(m.GetFloat(0), m.GetFloat(1), m.GetFloat(2)));
					break;

				case "SwitchLevel":
					levelIndex++;
					myBall.GetComponent<BallMover>().MoveBallAtWorldPos(startPoins[levelIndex].position);
					enemyBall.GetComponent<BallMover>().MoveBallAtWorldPos(startPoins[levelIndex].position);

					pioconnection.Send("StartNewLevel");
					break;
			}
		}

		// clear message queue after it's been processed
		msgList.Clear();
	}

	public void MoveBall(Vector3 newDir, float newPower)
	{
		currBallMover.MoveBall(newDir, newPower);		
	}

	public void MoveBallServer(Vector3 newDir, float newPower)
    {
		pioconnection.Send("Move", newDir.x, newDir.y, newDir.z, newPower);
    }

	public void AdjustBallPosServer(Vector3 pos)
    {
		pioconnection.Send("AdjustBallPos", pos.x, pos.y, pos.z);
    }

	public void PlayerEndRound()
    {
		pioconnection.Send("EndRound");
    }

	private void IsEndMap(PlayerIdentity currPlayer)
	{
		if(currPlayer == myBall.GetComponent<PlayerIdentity>())
        {
			pioconnection.Send("EndMap");
        }
	}

	void OnGUI() 
	{		
		if (infomsg != "") {
			GUI.Label(new Rect(10, 180, Screen.width, 20), infomsg);
		}
	}
		
}
