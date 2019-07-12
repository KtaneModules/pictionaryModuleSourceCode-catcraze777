using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public class pictionaryModuleScript : MonoBehaviour {
	public KMBombInfo BombInfo;
    public KMBombModule BombModule;
    public KMAudio KMAudio;
	public KMSelectable button1;
	public KMSelectable button2;
	public KMSelectable button3;
	public KMSelectable button4;
	public KMSelectable button5;
	public KMSelectable button6;
	public KMSelectable button7;
	public KMSelectable button8;
	public KMSelectable button9;
	public KMSelectable button0;
	public GameObject[] squares;
	bool enable;
	bool solved;
	int currDig;
	string code;
	string cornerTLPatterns;
	string cornerTRPatterns;
	string cornerBLPatterns;
	string cornerBRPatterns;
	string succsessPattern;
	string digitTL;
	string digitTR;
	string digitBL;
	string digitBR;
	int TL;
	int TR;
	int BL;
	int BR;

    //necessary for logging
    static int moduleIdCounter = 1;
    int moduleId;

    // Use this for initialization
    void Start () {
        moduleId = moduleIdCounter++;
        enabled = false;
		solved = false;
		currDig = 0;
		cornerTLPatterns = "01101001011111010001111011110000";
		cornerTRPatterns = "10010110101111100010110111110000";
		cornerBLPatterns = "10010110001111100100101111110000";
		cornerBRPatterns = "01101001001111011000011111110000";
		succsessPattern = "0111101101101001";
		digitTL = "03452719";
		digitTR = "16034528";
		digitBL = "40256137";
		digitBR = "21058346";
		TL = Random.Range(0,8);
		TR = Random.Range(0,8);
		BL = Random.Range(0,8);
		BR = Random.Range(0,8);
		digitTL = "03472195";
		digitTR = "16054283";
		digitBL = "40216375";
		digitBR = "21038465";
		TL = UnityEngine.Random.Range(0,8);
		TR = UnityEngine.Random.Range(0,8);
		BL = UnityEngine.Random.Range(0,8);
		BR = UnityEngine.Random.Range(0,8);
		code = digitTL.Substring(TL,1) + digitTR.Substring(TR,1) + digitBL.Substring(BL,1) + digitBR.Substring(BR,1);
		//code = "1234";
		foreach(GameObject obj in squares){
			obj.SetActive(false);
		}
		button1.OnInteract += delegate () { keyPress(1); return false;};
		button2.OnInteract += delegate () { keyPress(2); return false;};
		button3.OnInteract += delegate () { keyPress(3); return false;};
		button4.OnInteract += delegate () { keyPress(4); return false;};
		button5.OnInteract += delegate () { keyPress(5); return false;};
		button6.OnInteract += delegate () { keyPress(6); return false;};
		button7.OnInteract += delegate () { keyPress(7); return false;};
		button8.OnInteract += delegate () { keyPress(8); return false;};
		button9.OnInteract += delegate () { keyPress(9); return false;};
		button0.OnInteract += delegate () { keyPress(0); return false;};
		GetComponent<KMBombModule>().OnActivate += OnActivate;
	}
	
	void OnActivate()
    {
		string currSeg = cornerTLPatterns.Substring(4 * TL, 4);
		for (int i = 0; i < squares.Length; i ++){
			if(i % 4 == 0){
				if(i / 4 == 0){
					currSeg = cornerTLPatterns.Substring(4 * TL, 4);
				}else if (i / 4 == 1){
					currSeg = cornerTRPatterns.Substring(4 * TR, 4);
				}else if (i / 4 == 2){
					currSeg = cornerBLPatterns.Substring(4 * BL, 4);
				}else if (i / 4 == 3){
					currSeg = cornerBRPatterns.Substring(4 * BR, 4);
				}
			}
			if (int.Parse(currSeg.Substring(i % 4, 1)) == 1)squares[i].SetActive(true);
			else squares[i].SetActive(false);
		}
		enable = true;
        //logging
        Debug.LogFormat("[Pictionary #{0}] For logging purposes the pictures will be referenced from the manual in reading order", moduleId);
        Debug.LogFormat("[Pictionary #{0}] The Top left section of the 4x4 is from Picture {1}", moduleId, TL+1);
        Debug.LogFormat("[Pictionary #{0}] The Top right section of the 4x4 is from Picture {1}", moduleId, TR+1);
        Debug.LogFormat("[Pictionary #{0}] The Bottom left section of the 4x4 is from Picture {1}", moduleId, BL+1);
        Debug.LogFormat("[Pictionary #{0}] The Bottom right section of the 4x4 is from Picture {1}", moduleId, BR+1);
        Debug.LogFormat("[Pictionary #{0}] The passcode is {1}", moduleId, code);
    }
	void keyPress(int num){
		if(num == 1)button1.AddInteractionPunch();
		else if (num == 2)button2.AddInteractionPunch();
		else if (num == 3)button3.AddInteractionPunch();
		else if (num == 4)button4.AddInteractionPunch();
		else if (num == 5)button5.AddInteractionPunch();
		else if (num == 6)button6.AddInteractionPunch();
		else if (num == 7)button7.AddInteractionPunch();
		else if (num == 8)button8.AddInteractionPunch();
		else if (num == 9)button9.AddInteractionPunch();
		else if (num == 0)button0.AddInteractionPunch();
		
		//Debug.Log(num + " " + code.Substring(currDig, 1));
		if (num == int.Parse(code.Substring(currDig, 1)) && ! solved && enable){
			if (currDig == 3){
				BombModule.HandlePass();
				solved = true;
				for (int i = 0; i < squares.Length; i ++){
					if (int.Parse(succsessPattern.Substring(i, 1)) == 1)squares[i].SetActive(true);
					else squares[i].SetActive(false);
				}
			}
			else currDig ++;
		}else if (! solved && enable){
			BombModule.HandleStrike();
			currDig = 0;
			TL = Random.Range(0,7);
			TR = Random.Range(0,7);
			BL = Random.Range(0,7);
			BR = Random.Range(0,7);
			code = digitTL.Substring(TL, 1) + digitTR.Substring(TR, 1) + digitBL.Substring(BL, 1) + digitBR.Substring(BR, 1);
			OnActivate();
			return;
		}
		KMAudio.PlaySoundAtTransform("tick", this.transform);
	}

    //twitch plays
    private bool inputIsValid(string cmd)
    {
        char[] validstuff = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        for(int i = 0; i < cmd.Length; i++)
        {
            if (!validstuff.Contains(cmd.ElementAt(i)))
            {

            }
        }
        if(cmd.Length != 4)
        {
            return false;
        }
        return true;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} submit <num> [Submits the passcode of <num>, valid passcodes must be 4 digits long]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 2)
            {
                if (inputIsValid(parameters[1]))
                {
                    yield return null;
                    for(int i = 0; i < parameters[1].Length; i++)
                    {
                        if (parameters[1].ElementAt(i).Equals('0'))
                        {
                            button0.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('1'))
                        {
                            button1.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('2'))
                        {
                            button2.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('3'))
                        {
                            button3.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('4'))
                        {
                            button4.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('5'))
                        {
                            button5.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('6'))
                        {
                            button6.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('7'))
                        {
                            button7.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('8'))
                        {
                            button8.OnInteract();
                        }
                        else if (parameters[1].ElementAt(i).Equals('9'))
                        {
                            button9.OnInteract();
                        }
                        yield return new WaitForSeconds(0.1f    );
                    }
                }
            }
            yield break;
        }
    }
}
