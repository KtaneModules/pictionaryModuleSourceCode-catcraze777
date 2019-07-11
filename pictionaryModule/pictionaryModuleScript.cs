using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

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
	
	// Use this for initialization
	void Start () {
		enabled = false;
		solved = false;
		currDig = 0;
		cornerTLPatterns = "01101001011111100001111100001101";
		cornerTRPatterns = "10010110101111010010111100001110";
		cornerBLPatterns = "10010110001110110100111100001110";
		cornerBRPatterns = "01101001001101111000111100001101";
		succsessPattern = "0111101101101001";
		digitTL = "03472195";
		digitTR = "16054283";
		digitBL = "40216375";
		digitBR = "21038465";
		TL = Random.Range(0,8);
		TR = Random.Range(0,8);
		BL = Random.Range(0,8);
		BR = Random.Range(0,8);
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
}
