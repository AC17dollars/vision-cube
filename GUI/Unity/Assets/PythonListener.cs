using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Diagnostics;


public class PythonListener : MonoBehaviour
{
    Thread thread;
    Process process = new Process();
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;
    public string data = "";
    public string scrambleStep = "";
    public string solveStep = "";

    void Start()
    {
        // Start the server
        thread = new Thread(new ThreadStart(StartServer));
        thread.Start();
    }

    void OnDestroy()
    {
        // Clean up resources when the MonoBehaviour is being destroyed
        running = false;

        // Close the client, server, and process
        SendQuitMessage();
        client?.Close();
        server?.Stop();
        process.Close();

        // Wait for the thread to finish
        thread?.Join();
    }

    void StartServer()
    {
        try
        {
            // Create the server
            server = new TcpListener(IPAddress.Any, connectionPort);
            server.Start();
            UnityEngine.Debug.Log("Server is listening on port " + connectionPort);

            // Accept client connection

            while (true)
            {
                connectClient();
            }

        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error in StartServer: {ex.Message}");
        }
    }

    void connectClient()
    {
        client = server.AcceptTcpClient();
        UnityEngine.Debug.Log("Client connected!");

        // Start sending "Launch Camera" message
        //SendLaunchCameraMessage();

        // Start listening for data
        running = true;
        while (running)
        {
            running = Connection();
        }
    }


    void SendQuitMessage()
    {
        try
        {
            // Get the network stream and send the message
            NetworkStream nwStream = client.GetStream();
            string launchMessage = "Quit";
            byte[] launchBytes = Encoding.UTF8.GetBytes(launchMessage);
            nwStream.Write(launchBytes, 0, launchBytes.Length);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error sending launch message: {ex.Message}");
        }
    }

    bool Connection()
    {
        try
        {
            // Read data from the network stream
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            // Decode the bytes into a string
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (!string.IsNullOrEmpty(dataReceived))
            {
                data = dataReceived;
                UnityEngine.Debug.Log("Data received: " + data);
                if (data[0] == 'z')
                {
                    data = data.Substring(1);
                    scrambleStep = data;
                    data = "";
                    string response = "Scramble step received";
                    byte[] responseByte = Encoding.UTF8.GetBytes(response);
                    nwStream.Write(responseByte, 0, responseByte.Length);
                    SendScrambleSteps();
                    return true;
                }
                // Handle data according to your requirements
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error in Connection: {ex.Message}");
            return false;
        }
        return true;
    }



    void Update()
    {


    }


    public List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

    public void SendScrambleSteps()
    {
        print('1');
        //scrambleStep = "B U2 F2 B2 U' B2 U L' D' F' B D2 B2 R2 U2 D B2 L2 D B2 L2";
        if (scrambleStep != null && scrambleStep != "" && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag)
        {
            print(scrambleStep);
            keyboardControl.scrambleSteps = StringToList(scrambleStep);
        }
    }

    public void SendSolutionSteps()
    {
        print('2');

        //solveStep = "R L' B' U R' U B L F L2 F' D2 B2 U' D2 F2 L2 D2 F2 L2";
        if (solveStep != null && solveStep != "" && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag)
        {
            print(solveStep);
            keyboardControl.solveSteps = StringToList(solveStep);
            solveStep = "";
        }
    }

    public void launchPython()
    {
        try
        {
            // Get the network stream and send the message
            NetworkStream nwStream = client.GetStream();
            string launchMessage = "Launch Camera";
            byte[] launchBytes = Encoding.UTF8.GetBytes(launchMessage);
            nwStream.Write(launchBytes, 0, launchBytes.Length);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error sending launch message: {ex.Message}");
        }
    }

}



