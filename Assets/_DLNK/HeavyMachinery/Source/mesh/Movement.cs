using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;
using System;

public class Movement : MonoBehaviour
{

    ClientWebSocket websocket;

    public GameObject lightR;
    public GameObject lightL;
    private bool lights;


    public bool isForward = false;
    public bool isBackward = false;
    public bool isTurnRight = false;
    public bool isTurnLeft = false;

    
    public float currentRotatingConst = 0.0f;
    private float maxRotatingConst = 20.0f;
    private float minRotatingConst = -20.0f;
    public float rotatingSpeed = 0.0f;
    private float rotatingAcceleration = 100.0f;
    private float rotatingDeceleration = 100.0f;
    private float acceleration = 3.0f;
    private float deceleration = 3.0f;
    private float maxSpeed = 5.0f;
    private float minSpeed = -5.0f;
    public float currentSpeed = 0.0f;
    


    // Start is called before the first frame update
    async void Start()
    {
        lights = false;
       // lightR.SetActive(false);
        //lightL.SetActive(false);

        websocket = new ClientWebSocket();

        // Connect to the WebSocket server
        await websocket.ConnectAsync(new Uri("ws://localhost:8080"), CancellationToken.None);

        // Send a message to the WebSocket server
        string message = "Hello, server!";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

        // Receive messages from the WebSocket server
        byte[] receiveBuffer = new byte[1024];
        while (true)
        {
            WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                break;
            }
            else
            {
                string receivedMessage = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                Debug.Log("Received message: " + receivedMessage);


                
                if (receivedMessage == "forward")
                {
                    isForward = true;
                }
                if (receivedMessage == "forwardstop")
                {
                    isForward = false;
                }
                if (receivedMessage == "backward")
                {
                    isBackward = true;
                }
                if (receivedMessage == "backwardstop")
                {
                    isBackward = false;
                }
                if (receivedMessage == "turnleft")
                {
                    isTurnLeft = true;
                }
                if (receivedMessage == "turnleftstop")
                {
                    isTurnLeft = false;
                }
                if (receivedMessage == "turnright")
                {
                    isTurnRight = true;
                }
                if (receivedMessage == "turnrightstop")
                {
                    isTurnRight = false;
                }



            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isForward == true)
        {
            if (currentSpeed >= 0) currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0.0f, maxSpeed);
            else currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, currentSpeed, maxSpeed);
        }
        else if (isBackward == true)
        {
            if(currentSpeed <= 0) currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.deltaTime, minSpeed, 0.0f);
            else currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.deltaTime, minSpeed, currentSpeed);
        }
        else
        {
            if(currentSpeed > 0.0f) currentSpeed = Mathf.Clamp(currentSpeed - deceleration * Time.deltaTime, 0.0f, maxSpeed);
            if (currentSpeed < 0.0f) currentSpeed = Mathf.Clamp(currentSpeed + deceleration * Time.deltaTime, currentSpeed, 0.0f);
        }
        if (isTurnLeft == true)
        {
            if (currentRotatingConst <= 0) currentRotatingConst = Mathf.Clamp(currentRotatingConst - rotatingAcceleration * Time.deltaTime, minRotatingConst, 0.0f);
            else currentRotatingConst = Mathf.Clamp(currentRotatingConst - rotatingAcceleration * Time.deltaTime, minRotatingConst, currentRotatingConst);
        }
        else if (isTurnRight == true)
        {
            if (currentRotatingConst >= 0) currentRotatingConst = Mathf.Clamp(currentRotatingConst + rotatingAcceleration * Time.deltaTime, 0.0f, maxRotatingConst);
            else currentRotatingConst = Mathf.Clamp(currentRotatingConst + rotatingAcceleration * Time.deltaTime, currentRotatingConst, maxRotatingConst);
        }
        else
        {
            if (currentRotatingConst > 0.0f) currentRotatingConst = Mathf.Clamp(currentRotatingConst - rotatingDeceleration * Time.deltaTime, 0.0f, maxRotatingConst);
            if (currentRotatingConst < 0.0f) currentRotatingConst = Mathf.Clamp(currentRotatingConst + rotatingDeceleration * Time.deltaTime, currentRotatingConst, 0.0f);
        }
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
        rotatingSpeed = currentRotatingConst * currentSpeed;
        transform.Rotate(Vector3.up * rotatingSpeed * Time.deltaTime);
        
        
        
    }
}
