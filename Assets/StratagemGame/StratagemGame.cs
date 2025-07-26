using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StratagemGame : MonoBehaviour {

    string lastCode = "";

    public string[] defaultCodes = new string[] { 
        "wdsss",
        "wsdaw",
        "sswd",
        "swaswdsw",
        "dwws",
        "asdawd",
        "dsawas",
        "sss",
        "wswsws",
        "adadad",
        "dddaaa",
        "wdsd",
        "sadda",
        "ddsd",
        "dsaww",
        "dswwass",
        "sdddwasdaw",
        "wdsaw",
        "adsas",
        "wwddssaa",
        "dwasdwas",
        "wasd",
        "wwwwwwwwws",
        "asdwdsdw",
        "asdsdsw",
        "asdwasw",
        "dwasdaw",
        "sdaswdawdaswadswads",
        "wassdddwwwww",
        //Codigos de la pipol:
        "wadswasdw",
        "wdaswddsswadsws",
        "sssssssw",
        "wdsdadadws",
        "wwssadad",
        "ssssdwas"
    };


    public StratagemArrow defaultUPArrowPrefab;
    public StratagemArrow defaultLEFTArrowPrefab;
    public StratagemArrow defaultDOWNArrowPrefab;
    public StratagemArrow defaultRIGHTArrowPrefab;

    public Transform layoutForArrows;

    public AudioClip sndCorrectPress;
    public AudioClip sndWrongPress;
    public AudioClip sndVictory;

    public AudioSource sourceForAudio;

    public float disableArrowsOnErrorSeconds = 1f;

    public KeyCode[] upArrowAliases = new KeyCode[] {KeyCode.W, KeyCode.UpArrow};
    public KeyCode[] leftArrowAliases = new KeyCode[] { KeyCode.A, KeyCode.LeftArrow };
    public KeyCode[] downArrowAliases = new KeyCode[] { KeyCode.S, KeyCode.DownArrow };
    public KeyCode[] rightArrowAliases = new KeyCode[] { KeyCode.D, KeyCode.RightArrow };

    public int victoryCount;
    private bool playing;
    private bool inputEnabled = true;

    /// <summary>
    /// Settear en false luego de usarlo
    /// </summary>
    public bool gameVictory = false;
    public bool minorVictory = false;

    List<StratArrowContainer> newArrows;
    List<StratArrowContainer> usedArrows;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        newArrows = new List<StratArrowContainer>();
        usedArrows = new List<StratArrowContainer>();
    }

    // Update is called once per frame
    void Update() {
        if (playing && inputEnabled) {
            if (newArrows.Count == 0) {
                GameVictory();
            }
            if (Input.anyKeyDown) {
                CheckKeyPresses();
            }

        }
    }

    private void CheckKeyPresses() {

        foreach (KeyCode k in upArrowAliases) {
            if (Input.GetKeyDown(k)) {
                PressKey(ArrowDirection.up);
                return;
            }
        }
        foreach (KeyCode k in leftArrowAliases) {
            if (Input.GetKeyDown(k)) {
                PressKey(ArrowDirection.left);
                return;
            }
        }
        foreach (KeyCode k in downArrowAliases) {
            if (Input.GetKeyDown(k)) {
                PressKey(ArrowDirection.down);
                return;
            }
        }
        foreach (KeyCode k in rightArrowAliases) {
            if (Input.GetKeyDown(k)) {
                PressKey(ArrowDirection.right);
                return;
            }
        }

    }


    [ContextMenu("Run Test")]
    public void LoadTest() {
        StartArrowGame(defaultCodes[0]);
        
    }

    private void PressKey(ArrowDirection direction) {
        if (newArrows.Count == 0) {
            GameVictory();
        }
        if (newArrows[0].direction == direction) {
            newArrows[0].arrowScript.PressArrow();
            usedArrows.Add(newArrows[0]);
            newArrows.RemoveAt(0);
            if (newArrows.Count > 0) {
                sourceForAudio.PlayOneShot(sndCorrectPress);
            }
        } else {
            WrongInput();
        }
    }

    public void GameVictory() {
        sourceForAudio.PlayOneShot(sndVictory);
        StopGame();
        minorVictory = true;

    }

    public void WrongInput() {
        sourceForAudio.PlayOneShot(sndWrongPress);
        RestoreArrows(true, disableArrowsOnErrorSeconds);
        DisableInputsForTime(disableArrowsOnErrorSeconds);
    }

    public void RestoreArrows(bool doWrongAnimation = false, float wrongAnimationDuration = 1f) {
        List<StratArrowContainer> auxList = usedArrows;
        for (int i = 0; i < newArrows.Count; i++) {
            auxList.Add(newArrows[i]);
        }
        if (doWrongAnimation) {
            foreach (StratArrowContainer arrow in auxList) {
                arrow.arrowScript.DoWrongColorAnimation(wrongAnimationDuration);
            }
        } else {
            foreach (StratArrowContainer arrow in auxList) {
                arrow.arrowScript.UnPressArrow();
            }
        }
        newArrows = auxList;
        usedArrows = new List<StratArrowContainer>();
    }


    /// <summary>
    /// Comienza juego de flechas estilo estratagemas de Helldivers con el codigo deseado. El codigo está compuesto de las flechas arriba, izquierda, abajo, derecha, representadas por caracteres "wasd"
    /// </summary>
    /// <param name="arrowCode"></param>
    /// <returns></returns>
    public void StartArrowGame(string arrowCode, StratagemArrow prefabUP = null, StratagemArrow prefabLEFT = null, StratagemArrow prefabDOWN = null, StratagemArrow prefabRIGHT = null) {
        ClearArrows();
        newArrows = new List<StratArrowContainer>();
        usedArrows = new List<StratArrowContainer>();
        for (int i = 0; i < arrowCode.Length; i++) {
            switch (arrowCode[i]) {
                case 'w':
                    InstantiateArrow(prefabUP != null ? prefabUP : defaultUPArrowPrefab, ArrowDirection.up);;
                    break;
                case 'a':
                    InstantiateArrow(prefabLEFT != null ? prefabLEFT : defaultLEFTArrowPrefab, ArrowDirection.left);
                    break;
                case 's':
                    InstantiateArrow(prefabDOWN != null ? prefabDOWN : defaultDOWNArrowPrefab, ArrowDirection.down);
                    break;
                case 'd':
                    InstantiateArrow(prefabRIGHT != null ? prefabRIGHT : defaultRIGHTArrowPrefab, ArrowDirection.right);
                    break;
                default:
                    break;
            }
        }
        playing = true;


    }

    private void InstantiateArrow(StratagemArrow arrowToInstantiate, ArrowDirection direction) {
        StratagemArrow thisArrow = GameObject.Instantiate(arrowToInstantiate, layoutForArrows);
        StratArrowContainer arrowCont = new StratArrowContainer(thisArrow, direction);
        newArrows.Add(arrowCont);
    }

    public void ClearArrows() {
        foreach (StratArrowContainer arrow in newArrows) {
            Destroy(arrow.arrowScript.gameObject);
        }
        newArrows.Clear();
        foreach (StratArrowContainer arrow in usedArrows) {
            Destroy(arrow.arrowScript.gameObject);
        }
        usedArrows.Clear();
    }

    public void StopGame() {
        ClearArrows();
        playing = false;
    }

    private void LoadRandomCode() {
        LoadRandomCode(null, null, null, null);
    }

    private void LoadRandomCode(StratagemArrow prefabUP, StratagemArrow prefabLEFT, StratagemArrow prefabDOWN, StratagemArrow prefabRIGHT) {
        RandomDefaultStart:
        string code = defaultCodes[Random.Range(0, defaultCodes.Length)];
        if (code == lastCode) {
            goto RandomDefaultStart;
        }
        lastCode = code;
        StartArrowGame(code, prefabUP, prefabLEFT, prefabDOWN, prefabRIGHT);
    }

    public void DoArrowsGame(int victoriesNeeded = 1) {
        DoArrowsGame(victoriesNeeded, null, null, null, null);
    }

    public void DoArrowsGame(int victoriesNeeded, StratagemArrow prefabUP, StratagemArrow prefabLEFT, StratagemArrow prefabDOWN, StratagemArrow prefabRIGHT) {
        StartCodeFrenzy(victoriesNeeded, prefabUP, prefabLEFT, prefabDOWN, prefabRIGHT);
    }



    private Coroutine frenzyCR;

    public void StartEndlessCodes() {
        StartCodeFrenzy(9000, null, null, null, null);
    }

    public void StartCodeFrenzy(int victoriesNeeded, StratagemArrow prefabUP, StratagemArrow prefabLEFT, StratagemArrow prefabDOWN, StratagemArrow prefabRIGHT) {
        LoadRandomCode(prefabUP, prefabLEFT, prefabDOWN, prefabRIGHT);
        frenzyCR = StartCoroutine(CodeFrenzyCR(victoriesNeeded, prefabUP, prefabLEFT, prefabDOWN, prefabRIGHT));
    }

    public IEnumerator CodeFrenzyCR(int victoriesNeeded, StratagemArrow prefabUP = null, StratagemArrow prefabLEFT = null, StratagemArrow prefabDOWN = null, StratagemArrow prefabRIGHT = null) {
        victoryCount = 0;
        while (true) {
            if (minorVictory) {
                victoryCount++;
                if (victoryCount >= victoriesNeeded) {
                    victoryCount = 0;
                    gameVictory = true;
                    break;
                }
                minorVictory = false;
                LoadRandomCode(prefabUP, prefabLEFT, prefabDOWN, prefabRIGHT);
            }
            yield return null;
        }
    }



    public void StopFrenzy() {
        if (frenzyCR != null) {
            StopCoroutine(frenzyCR);
        }
        StopGame();
    }

    private void DisableInputsForTime(float durationSeconds) {
        StartCoroutine(DisableInputCR(durationSeconds));
    }

    private IEnumerator DisableInputCR(float durationSeconds) {
        inputEnabled = false;
        yield return new WaitForSeconds(durationSeconds);
        inputEnabled = true;
    }

}

public class StratArrowContainer {
    public StratagemArrow arrowScript;
    public ArrowDirection direction;

    public StratArrowContainer() {
        
    }


    public StratArrowContainer(StratagemArrow arrowScript, ArrowDirection arrowDirection) {
        this.direction = arrowDirection;
        this.arrowScript = arrowScript;
    }
}

public enum ArrowDirection {
    up,
    left,
    down,
    right
}
