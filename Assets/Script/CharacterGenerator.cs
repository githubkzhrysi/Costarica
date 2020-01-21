using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    int playerNum = StartPageController.playerNum;
    public int[,] plyrPieceLeft;

    public bool done;

    List<Vector3> startPositions = new List<Vector3>
    {
        new Vector3(-5f,0f, 0f),
        new Vector3( 5f,0f, 0f),
        new Vector3(-2.5f,0f,-5f),
        new Vector3(-2.5f,0f, 5f),
        new Vector3( 2.5f,0f, 5f),
        new Vector3( 2.5f,0f,-5f)
    };

    public List<GameObject> Player;
    List<GameObject> PlyrPieces; 

    void Start()
    {
        PlyrPieces = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            GeneratePlayers(i);
        }
        plyrPieceLeft = new int[playerNum,6];
        for (int i = 0; i < playerNum; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                plyrPieceLeft[i, j] = 1;
            }            
        }
    }

    //生成された駒たちがどういうリストに保存されているのか謎すぎる。
    void GeneratePlayers (int posNum)
    {
        GameObject plyr = Instantiate(Player[0], startPositions[posNum], Quaternion.identity);
        plyr.tag = posNum.ToString();
        PlyrPieces.Add(plyr);
    }

    public List<Vector3> GetStartPos()
    {
        return startPositions;
    }

}


