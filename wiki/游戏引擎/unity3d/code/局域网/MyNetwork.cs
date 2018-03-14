using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyNetwork : MonoBehaviour {

	public int connections = 10;
	public int listenPort = 8899;
	public bool useNat = false;
	public string IP = "127.0.0.1";
	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {

			if (GUILayout.Button ("create server")) {
				//create server
				NetworkConnectionError error = Network.InitializeServer (connections, listenPort, useNat);
				print (error);
			}

			if (GUILayout.Button ("connect server")) {
				NetworkConnectionError error = Network.Connect (IP, listenPort);
				print (error);
			}
				
		} else if (Network.peerType == NetworkPeerType.Server) {
			
			GUILayout.Label ("create server ok");
			GUILayout.Label ("IP:" + Network.player.ipAddress);

		} else if (Network.peerType == NetworkPeerType.Client) {

			GUILayout.Label ("connect server ok");
		}
		IP = GameObject.Find ("ServerIP").GetComponent<InputField>().text;
	}

	//server
	void OnServerInitialized(){
		print ("server init ok");

		int group = int.Parse (Network.player + "");
		Network.Instantiate (playerPrefab, new Vector3 (1, 1, 0), Quaternion.identity, group);

	}

	void OnPlayerConnected(NetworkPlayer player) {
		print ("OnPlayerConnected, index number:" + player);
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		print ("OnPlayerDisconnected, index number:" + player);
	}

	//client
	void OnConnectedToServer() {
		print ("OnConnectedToServer");

		int group = int.Parse (Network.player + "");
		Network.Instantiate (playerPrefab, new Vector3 (1, 2, 0), Quaternion.identity, group);
	}

	void OnDisconnectedFromServer() {
		print ("OnDisconnectedFromServer");
	}
}
