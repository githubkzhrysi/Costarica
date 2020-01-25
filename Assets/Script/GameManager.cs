using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MapGenerator mg;
    public CharacterGenerator cg;
    public Text turnLabel;
    public Text masterLabel;
    public GameObject plyrNoticePanel;
    public GameObject masterNoticePanel;
    public Text plyrNotice;
    public GameObject effectPrefab;
    public LayerMask tileMask;
    public LayerMask playerMask;
    public Text masterNoticeText;
    public GameObject continueAskPannel;
    public GameObject passConfirmPanel;
    public Button passYesBtn;
    public Button passNoBtn;
    public Button capturedAnimalPanelBtn;
    public GameObject capturedAnimalAnnouncePanel;
    public Text animalText;

    int playerNum;
    int turn;
    int whoseTurn;
    int plyr;
    int phase;
    int type;
    int number;
    int danger;
    int dangerCount=0;
    GameObject selectedPlyrPiece;
    Vector3 selectedTilePos;
    
    bool PNIsDone = false;
    bool pieceSelectIsDone = false;
    bool isDead = false;
    bool stopSelect = false;
    bool isSickAnimals;
    //bool newTurnReady = true;



    List<Vector3> PlyrsPos = new List<Vector3>();
    List<int[]> plyrsLeft=new List<int[]> ();
    List<Vector3> MovableTiles = new List<Vector3>();
    RaycastHit selected;
    List<List<int[]>> AnimalCapturedList= new List<List<int[]>>();
    List<List<int[]>> AnimalWithDiseaseList = new List<List<int[]>>();
    Dictionary<int, string> animalInfo = new Dictionary<int, string>()
    {
        {10,"サル" },
        {11,"ヘラクレスオオカブト" },
        {20,"アマガエル" },
        {21,"ワニ" },
        {30,"オウム" },
        {31,"ヒョウ" }
    };

    // Start is called before the first frame update
    void Start()
    {
        playerNum = StartPageController.playerNum;
        plyrNoticePanel.SetActive(true);
        //ようこそコスタリカの世界へ！たくさんの珍しい動物たちを持ち帰ることができるように頑張ってください！
        plyrNotice.text = "ようこそコスタリカの世界へ";
        //探検隊の初期位置設定
        PlyrsPos = cg.GetStartPos();
        //プレイヤーの残りゴマの把握リスト作成
        for (int i =0; i < playerNum; i++)
        {
            int[] plyrpiece = { 1, 1, 1, 1, 1, 1 };
            plyrsLeft.Add(plyrpiece);
        }
        //探検隊の捕獲動物の把握リスト作成
        for (int i = 0; i < playerNum; i++)
        {
            
            AnimalCapturedList.Add(new List<int[]>());
        }
        //病気の動物の把握リスト作成
        for (int i = 0; i< playerNum; i++)
        {
            AnimalWithDiseaseList.Add(new List<int[]>());
        }

        turn = 1;
        phase =0;
    }

    // Update is called once per frame
    void Update()
    {
        //ターン開始のお知らせ
        if (phase == 1)
        {
            TurnChecker();
            if (!PNIsDone)
            {
                PlyrNotification();
                PNIsDone = true;
            }
        }
        //探検グループの選定と行動
        if (phase == 2)
        {
            //turn
            masterLabel.text = "探検させるグループを選択してください";
            if (!pieceSelectIsDone)
            {
                
                foreach (Vector3 v3 in PlyrsPos)
                {                    
                    EffectOn(v3);
                }
            }

            //グループの選択
            if (!pieceSelectIsDone && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f, playerMask))
                {
                    string selectedTag = hit.collider.gameObject.transform.parent.gameObject.tag;
                    stopSelect = true;
                    if (cg.plyrPieceLeft[plyr, int.Parse(selectedTag)] == 1)
                    {
                        stopSelect = false;
                        Vector3 selectedPlyrPos = hit.collider.gameObject.transform.parent.gameObject.transform.position;                   
                        pieceSelectIsDone = true;
                        selectedPlyrPiece = hit.collider.gameObject.transform.parent.gameObject;
                    }
                    else
                    {
                        masterNoticePanel.SetActive(true);
                        masterNoticeText.text = "その隊にはあなたの隊員はいません";
                    }                    
                }
            }
            //タイルの選択
            if (pieceSelectIsDone)
            {
                isSickAnimals = false;
                if (selectedPlyrPiece != null)
                {
                    EffectOn(selectedPlyrPiece.transform.position);
                    CheckMovability(selectedPlyrPiece.transform.position);
                }
                foreach (Vector3 v3 in MovableTiles)
                {
                    EffectOn(v3);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //Hitに格納されるのは駒の頭の部分だけでなので注意!!
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100.0f, tileMask))
                    {
                        if (MovableTiles.Contains(hit.collider.gameObject.transform.position)&&!stopSelect)
                        {
                            PlyrsPos.Remove(selectedPlyrPiece.transform.position);                            
                            selectedPlyrPiece.transform.position=hit.collider.gameObject.transform.position;
                            PlyrsPos.Add(selectedPlyrPiece.transform.position);

                            TileOpen(hit.collider.gameObject);                            
                            if(!isDead)
                            {
                                continueAskPannel.SetActive(true);
                                stopSelect = true;
                            }                            
                        }                       
                    }
                }

                if (isDead)
                {
                    Dead();
                    NewTurnStart();
                } 
            }
        }
    }
    void NewTurnStart()
    {
        turn++;
        PNIsDone = false;
        pieceSelectIsDone = false;
        isDead = false;
        selectedPlyrPiece = null;
        dangerCount = 0;
        phase = 1;
        isSickAnimals = false;
    }
    void TurnChecker()
    {
        plyr = turn % playerNum;
        whoseTurn = plyr;
        if (plyr == 0)
        {
            whoseTurn = playerNum;
        }
        turnLabel.text = $"現在はプレーヤー{whoseTurn}の番です。 手番カウント"+turn;        
    }

    void PlyrNotification()
    {
        plyrNotice.text = $"プレーヤー{whoseTurn}の番になりました。";
        plyrNoticePanel.SetActive(true);
    }

    public void PNoticePanelButtonClicked()
    {
        plyrNoticePanel.SetActive(false);
        phase++;
    }

    public void EffectOn(Vector3 pos)
    {
        Instantiate(effectPrefab, pos, Quaternion.identity);

    }

    void CheckMovability(Vector3 pos)
    {
        MovableTiles.Clear();

        List<Vector3> possibleTiles = new List<Vector3>();
        possibleTiles.Add(pos + new Vector3(1f, 0f, 0));
        possibleTiles.Add(pos + new Vector3(0.5f, 0, 1f));
        possibleTiles.Add(pos + new Vector3(-0.5f, 0, 1f));
        possibleTiles.Add(pos + new Vector3(-1f, 0, 0));
        possibleTiles.Add(pos + new Vector3(-0.5f, 0, -1f));
        possibleTiles.Add(pos + new Vector3(0.5f, 0, -1f));

        foreach (Vector3 v3 in possibleTiles)
        {
            if (mg.MapPosition.Contains(v3))
            {
                MovableTiles.Add(v3);
            }
        }
        pieceSelectIsDone = true;
    }


     void TileOpen(GameObject tile)
    {
        switch (tile.tag)
        {
            case "Steppe":
                
                number = Random.Range(0, 4);
                danger = Random.Range(0, 6);
                if (danger <1)
                {
                    dangerCount++;
                    isSickAnimals = true;
                    DangerNotice();
                    
                    if (isDead)
                    {
                        break;
                    }
                }
                if (number < 3)
                {
                    number = 1;                    
                }
                else
                {
                    number = 2;
                }
                int[] animalCapturedSteppe = new int[number];
                for (int i = 0;i<number;i++)
                {
                    animalCapturedSteppe[i] = 10 + Random.Range(0, 2);
                }
                AnimalAdd(animalCapturedSteppe);
                AnimalCapturedAnnounce();
                Destroy(tile);

                break;
            case "River":
                type = Random.Range(0, 2);
                number = Random.Range(0, 4);
                danger = Random.Range(0, 6);
                if(danger < 2)
                {
                    dangerCount++;
                    isSickAnimals = true;
                    DangerNotice();

                    if (isDead)
                    {
                        break;
                    }
                }
                if (number < 2)
                {
                    number = 1;
                }
                else
                {
                    number = 2;
                }
                int[] animalCapturedRiver = new int[number];
                for (int i = 0; i < number; i++)
                {
                    animalCapturedRiver[i] = 20 + Random.Range(0, 2);
                }
                AnimalAdd(animalCapturedRiver);
                AnimalCapturedAnnounce();
                Destroy(tile);
                break;
            case "Mountain":
                type = Random.Range(0, 2);
                number = Random.Range(0, 4);
                danger = Random.Range(0, 6);
                if(danger < 3)
                {
                    dangerCount++;
                    isSickAnimals = true;
                    DangerNotice();

                    if (isDead)
                    {
                        break;
                    }
                }
                if (number < 1)
                {
                    number = 1;
                }
                else
                {
                    number = 2;
                }
                int[] animalCapturedMountain = new int[number];
                for (int i = 0; i < number; i++)
                {
                    animalCapturedMountain[i] = 30 + Random.Range(0, 2);
                }
                AnimalAdd(animalCapturedMountain);
                AnimalCapturedAnnounce();
                Destroy(tile);
                break;
        }
    }
    //タイルの中身の結果のリストへの追加
    void AnimalAdd(int [] capturedAnimals)
    {
        if (isSickAnimals)
        {
            AnimalWithDiseaseList[plyr].Add(capturedAnimals);            
            isSickAnimals = false;
        }
        else
        {
            AnimalCapturedList[plyr].Add(capturedAnimals);
        }
    }
    public void AnimalCapturedAnnounce()
    {
        capturedAnimalAnnouncePanel.SetActive(true);
        int turnPlyr = plyr + 1;
        string animals = "プレイヤー" + turnPlyr + "さんの場合\n";
        if (AnimalCapturedList[plyr].Count > 0)
        {
            for (int i = 0; i < AnimalCapturedList[plyr].Count; i++)
            {
                for (int j = 0; j < AnimalCapturedList[plyr][i].Length; j++)
                {
                    animals += animalInfo[AnimalCapturedList[plyr][i][j]] + "\n";
                }
            }
        }
        animals += "↓病気の動物たち↓\n";
        if (AnimalWithDiseaseList[plyr].Count > 0)
        {
            for (int i = 0; i < AnimalWithDiseaseList[plyr].Count; i++)
            {
                for (int j = 0; j < AnimalWithDiseaseList[plyr][i].Length; j++)
                {
                    animals += animalInfo[AnimalWithDiseaseList[plyr][i][j]] + "\n";
                }
            }
        }        
        animalText.text = animals;
    }
    
    public void ShowCaptureAnimalBtnClicked()
    {        
        AnimalCapturedAnnounce();
    }

    public void CapAniBtnClicked()
    {
        capturedAnimalAnnouncePanel.SetActive(false);
    }

     void MasterNoticePanelButtonClicked()
    {
        masterNoticePanel.SetActive(false);
    }

    void DangerNotice()
    {
        masterNoticePanel.SetActive(true);
        masterNoticeText.text = "病気にかかってしまった。";
        if (dangerCount == 2)
        {
            masterNoticeText.text = "病気が悪化して死んでしまった。";
            isDead = true;            
        }
    }
    void Dead()
    {
        AnimalWithDiseaseList.Clear();
        PlayerPieceCheck();
        stopSelect = false;
    }

    void PlayerPieceCheck()
    {
        if (selectedPlyrPiece != null)
        {
            cg.plyrPieceLeft[plyr, int.Parse(selectedPlyrPiece.tag)] = 0;
            bool isEmpty = true;
            for (int i = 0; i < playerNum; i++)
            {
                if (cg.plyrPieceLeft[i, int.Parse(selectedPlyrPiece.tag)] == 1)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {
                PlyrsPos.Remove(selectedPlyrPiece.transform.position);
                Destroy(selectedPlyrPiece);
            }
        }        
    }
    public void ContinueBtnClicked()
    {
        continueAskPannel.SetActive(false);
        stopSelect = false;
    }
    public void EndBtnClicked()
    {
        continueAskPannel.SetActive(false);
        if (AnimalWithDiseaseList[plyr].Count > 0)
        {
            for(int i = 0;i < AnimalWithDiseaseList[plyr].Count; i++)
            {
                AnimalCapturedList[plyr].Add(AnimalWithDiseaseList[plyr][i]);
            }
        }
        Dead();
        NewTurnStart();
    }    
    public void PassBtnClicked()
    {
        passConfirmPanel.SetActive(true);
    }
    public void PassYesClicked()
    {
        passConfirmPanel.SetActive(false);
        Dead();
        NewTurnStart();
    }
    public void PassNoClicked()
    {
        passConfirmPanel.SetActive(false);
    }
    public void MasterNoticePanelClicked()
    {
        masterNoticePanel.SetActive(false);
        stopSelect = false;
    }
}
//改善点エフェクトが毎フレームごとに増加していくので、それをコルーチンにして一定時間あたりに生成する数を制御する。
