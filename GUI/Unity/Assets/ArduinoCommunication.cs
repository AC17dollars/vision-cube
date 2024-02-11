using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Numerics;
using System;

public class ArduinoCommunication : MonoBehaviour
{
    // Start is called before the first frame update
    public static string arduinoMoveString = "";

    SerialPort serialPort;
    public string portName = "COM3";
    public int baudRate = 9600;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();

    }

    // Update is called once per frame
    void Update()
    {
        if (arduinoMoveString.Length > 0)
        {
            print(arduinoMoveString);
            print(RephraseToBottomRotation(arduinoMoveString));
            arduinoMoveString = "";
            serialPort.Write(RephraseToBottomRotation(arduinoMoveString));
        }
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine();
            Debug.Log("Data from Arduino:" + data);
        }

    }

    string RephraseToBottomRotation(string moveString)
    {
        List<string> moves = new List<string>(moveString.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));

        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i][0] == 'D') continue;
            else if (moves[i][0] == 'R')
            {
                moves[i] = "->D" + moves[i].Substring(1);
                for (int j = i + 1; j < moves.Count; j++)
                {
                    switch (moves[j][0])
                    {
                        case 'R':
                            moves[j] = 'D' + moves[j].Substring(1);
                            break;
                        case 'L':
                            moves[j] = 'U' + moves[j].Substring(1);
                            break;
                        case 'U':
                            moves[j] = 'R' + moves[j].Substring(1);
                            break;
                        case 'D':
                            moves[j] = 'L' + moves[j].Substring(1);
                            break;
                        case 'F':
                        case 'B':
                            // Do nothing
                            break;
                    }
                }
            }
            else if (moves[i][0] == 'L')
            {
                moves[i] = "->->->D" + moves[i].Substring(1);
                for (int j = i + 1; j < moves.Count; j++)
                {
                    switch (moves[j][0])
                    {
                        case 'R':
                            moves[j] = 'U' + moves[j].Substring(1);
                            break;
                        case 'L':
                            moves[j] = 'D' + moves[j].Substring(1);
                            break;
                        case 'U':
                            moves[j] = 'L' + moves[j].Substring(1);
                            break;
                        case 'D':
                            moves[j] = 'R' + moves[j].Substring(1);
                            break;
                        case 'F':
                        case 'B':
                            // Do nothing
                            break;
                    }
                }
            }
            else if (moves[i][0] == 'U')
            {
                moves[i] = "->->D" + moves[i].Substring(1);
                for (int j = i + 1; j < moves.Count; j++)
                {
                    switch (moves[j][0])
                    {
                        case 'R':
                            moves[j] = 'L' + moves[j].Substring(1);
                            break;
                        case 'L':
                            moves[j] = 'R' + moves[j].Substring(1);
                            break;
                        case 'U':
                            moves[j] = 'D' + moves[j].Substring(1);
                            break;
                        case 'D':
                            moves[j] = 'U' + moves[j].Substring(1);
                            break;
                        case 'F':
                        case 'B':
                            // Do nothing
                            break;
                    }
                }
            }
            else if (moves[i][0] == 'F')
            {
                moves[i] = "=>->D" + moves[i].Substring(1);
                for (int j = i + 1; j < moves.Count; j++)
                {
                    switch (moves[j][0])
                    {
                        case 'R':
                            moves[j] = 'B' + moves[j].Substring(1);
                            break;
                        case 'L':
                            moves[j] = 'F' + moves[j].Substring(1);
                            break;
                        case 'U':
                            moves[j] = 'R' + moves[j].Substring(1);
                            break;
                        case 'D':
                            moves[j] = 'L' + moves[j].Substring(1);
                            break;
                        case 'F':
                            moves[j] = 'D' + moves[j].Substring(1);
                            break;
                        case 'B':
                            moves[j] = 'U' + moves[j].Substring(1);
                            break;
                    }
                }
            }
            else if (moves[i][0] == 'B')
            {
                moves[i] = "<=->D" + moves[i].Substring(1);
                for (int j = i + 1; j < moves.Count; j++)
                {
                    switch (moves[j][0])
                    {
                        case 'R':
                            moves[j] = 'F' + moves[j].Substring(1);
                            break;
                        case 'L':
                            moves[j] = 'B' + moves[j].Substring(1);
                            break;
                        case 'U':
                            moves[j] = 'R' + moves[j].Substring(1);
                            break;
                        case 'D':
                            moves[j] = 'L' + moves[j].Substring(1);
                            break;
                        case 'F':
                            moves[j] = 'U' + moves[j].Substring(1);
                            break;
                        case 'B':
                            moves[j] = 'D' + moves[j].Substring(1);
                            break;
                    }
                }
            }
        }

        return string.Join(" ", moves);

    }

    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}





//#include <iostream>
//#include <string>
//#include <vector>
//#include <sstream>

//using namespace std;

//int main()
//{

//    string sentence;

//    //    cout << "Enter a sentence: ";
//    //  getline(cin, sentence);

//    //string storedSentence = sentence;
//    sentence = "U D2 F R U2 F' L2 D' F R2 U2 L' B2 U B2 U2 F2 U' L2 U' B2 U";


//    stringstream ss(sentence);
//    string word;
//    vector<string> words;


//    while (ss >> word)
//    {
//        words.push_back(word);
//    }


//    for (int i = 0; i < words.size(); i++)
//    {
//        if (words[i][0] == 'D') continue;
//        else if (words[i][0] == 'R')
//        {
//            words[i] = "->D" + words[i].substr(1);
//            for (int j = i + 1; j < words.size(); j++)
//            {
//                if (words[j][0] == 'R')
//                {
//                    words[j][0] = 'D';
//                }
//                else if (words[j][0] == 'L')
//                {
//                    words[j][0] = 'U';
//                }
//                else if (words[j][0] == 'U')
//                {
//                    words[j][0] = 'R';
//                }
//                else if (words[j][0] == 'D')
//                {
//                    words[j][0] = 'L';
//                }
//                else if (words[j][0] == 'F')
//                {
//                    words[j][0] = 'F';
//                }
//                else if (words[j][0] == 'B')
//                {
//                    words[j][0] = 'B';
//                }
//            }
//        }

//        else if (words[i][0] == 'L')
//        {
//            words[i] = "->->->D" + words[i].substr(1);
//            for (int j = i + 1; j < words.size(); j++)
//            {
//                if (words[j][0] == 'R')
//                {
//                    words[j][0] = 'U';
//                }
//                else if (words[j][0] == 'L')
//                {
//                    words[j][0] = 'D';
//                }
//                else if (words[j][0] == 'U')
//                {
//                    words[j][0] = 'L';
//                }
//                else if (words[j][0] == 'D')
//                {
//                    words[j][0] = 'R';
//                }
//                else if (words[j][0] == 'F')
//                {
//                    words[j][0] = 'F';
//                }
//                else if (words[j][0] == 'B')
//                {
//                    words[j][0] = 'B';
//                }
//            }
//        }

//        else if (words[i][0] == 'U')
//        {
//            words[i] = "->->D" + words[i].substr(1);
//            for (int j = i + 1; j < words.size(); j++)
//            {
//                if (words[j][0] == 'R')
//                {
//                    words[j][0] = 'L';
//                }
//                else if (words[j][0] == 'L')
//                {
//                    words[j][0] = 'R';
//                }
//                else if (words[j][0] == 'U')
//                {
//                    words[j][0] = 'D';
//                }
//                else if (words[j][0] == 'D')
//                {
//                    words[j][0] = 'U';
//                }
//                else if (words[j][0] == 'F')
//                {
//                    words[j][0] = 'F';
//                }
//                else if (words[j][0] == 'B')
//                {
//                    words[j][0] = 'B';
//                }
//            }
//        }

//        else if (words[i][0] == 'F')
//        {
//            words[i] = "=>->D" + words[i].substr(1);
//            for (int j = i + 1; j < words.size(); j++)
//            {
//                if (words[j][0] == 'R')
//                {
//                    words[j][0] = 'B';
//                }
//                else if (words[j][0] == 'L')
//                {
//                    words[j][0] = 'F';
//                }
//                else if (words[j][0] == 'U')
//                {
//                    words[j][0] = 'R';
//                }
//                else if (words[j][0] == 'D')
//                {
//                    words[j][0] = 'L';
//                }
//                else if (words[j][0] == 'F')
//                {
//                    words[j][0] = 'D';
//                }
//                else if (words[j][0] == 'B')
//                {
//                    words[j][0] = 'U';
//                }
//            }
//        }

//        else if (words[i][0] == 'B')
//        {
//            words[i] = "<=->D" + words[i].substr(1);
//            for (int j = i + 1; j < words.size(); j++)
//            {
//                if (words[j][0] == 'R')
//                {
//                    words[j][0] = 'F';
//                }
//                else if (words[j][0] == 'L')
//                {
//                    words[j][0] = 'B';
//                }
//                else if (words[j][0] == 'U')
//                {
//                    words[j][0] = 'R';
//                }
//                else if (words[j][0] == 'D')
//                {
//                    words[j][0] = 'L';
//                }
//                else if (words[j][0] == 'F')
//                {
//                    words[j][0] = 'U';
//                }
//                else if (words[j][0] == 'B')
//                {
//                    words[j][0] = 'D';
//                }
//            }
//        }

//    }
//    int num = 1;
//    for (string & w: words)
//    {
//        cout << num << ". " << w << endl;
//        num++;
//    }
//    return 0;
//}
