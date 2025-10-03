using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
enum すごろく
{
    Lobby,
    Start,
    Move,
    Stop,
    NormalPoint,
    PlusPoint,
    MinusPoint,
    Goal
}

public class GameController : MonoBehaviour
{
    bool finished = false;
    bool isPushed = false;
    bool reverseWinding = false;
    bool right = false;
    bool minusPoint = false;
    float timer = 0;
    int windingPer = 1;
    int direction = 0;
    int nowPoint = 0;
    int saikoro;
    すごろく GameWave;
    Vector3 nowPos;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject player;
    [SerializeField] List<PointDatas> pointDatas = new List<PointDatas>();
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        GameObject points = GameObject.Find("Points");
        while (points.transform.childCount != i)
        {
            pointDatas.Add(points.transform.GetChild(i).GetComponent<PointDatas>());
            i++;
        }
        GameWave = すごろく.Lobby;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameWave)
        {
            default:
                GameWave = すごろく.NormalPoint;
                break;
            case すごろく.Lobby:
                if (Input.GetAxis("Jump") > 0)
                {
                    GameWave++;
                    text.text = "Start!!!";
                }
                break;
            case すごろく.Start:
                if (isPushed) return;
                if (Input.GetKeyUp(KeyCode.Space)) isPushed = true;
                if (isPushed)
                {
                    saikoro = 2;//デバック
                    //saikoro = UnityEngine.Random.Range(0, 6) + 1;
                    text.text += "\nサイコロの目は" + (2);//デバック
                    //text.text += "\nサイコロの目は" + (saikoro);
                    if (minusPoint)
                    {
                        reverseWinding = true;
                    }
                    if (nowPoint == pointDatas.Count)
                    {
                        GameWave = すごろく.Goal;
                    }
                    else
                    {
                        GameWave = すごろく.Move;
                        nowPos = player.transform.position;
                        text.text += "\n移動中...";
                    }
                }
                break;
            case すごろく.Move:
                if (!minusPoint)
                {
                    reverseWinding = false;
                }
                right = pointDatas[nowPoint].upperWinding;
                float totalTime = 1;
                float x1;
                float z1;
                float x2;
                float z2;
                if (reverseWinding)
                {
                    windingPer = pointDatas[nowPoint - 1].windingPer;
                    x2 = player.transform.position.x;
                    z2 = player.transform.position.z;
                    x1 = pointDatas[nowPoint - 1].transform.position.x;
                    z1 = pointDatas[nowPoint - 1].transform.position.z;
                }
                else
                {
                    windingPer = pointDatas[nowPoint].windingPer;
                    x1 = player.transform.position.x;
                    z1 = player.transform.position.z;
                    x2 = pointDatas[nowPoint + 1].transform.position.x;
                    z2 = pointDatas[nowPoint + 1].transform.position.z;
                }
                if ((Mathf.Sqrt((x2 - x1) * (x2 - x1) + (z2 - z1) * (z2 - z1)) > 0.01f && !finished) && saikoro != 0)
                {
                    if (pointDatas[nowPoint].windingPer == 0)
                    {
                        player.transform.position += ((pointDatas[nowPoint + 1].transform.position - nowPos) / totalTime) * Time.deltaTime;
                    }
                    else
                    {
                        float speed = 3;
                        Vector3 pointA = pointDatas[nowPoint].transform.position;
                        Vector3 pointB;
                        if (reverseWinding)
                        {
                            pointB = pointDatas[nowPoint - 1].transform.position;
                        }
                        else
                        {
                            pointB = pointDatas[nowPoint + 1].transform.position;
                        }
                        Vector3 mypos = player.transform.position;
                        Vector3 center = Vector3.zero;
                        center = new Vector3(pointB.x, pointA.y, pointA.z);
                        float l = Vector3.Distance(pointA, center);
                        float m = Vector3.Distance(pointB, center);
                        float nowAngle = 0;

                        int startValue = 0;
                        if (!pointDatas[nowPoint].isLower)
                        {
                            if (right)
                            {
                                startValue = 1;
                                if (reverseWinding)
                                {
                                    startValue = 3 + windingPer - 1;
                                }
                            }
                            else
                            {
                                startValue = 2;
                                if (reverseWinding)
                                {
                                    startValue = 4 + windingPer - 1;
                                }
                            }
                        }
                        else
                        {
                            if (right)
                            {
                                startValue = 5;
                                if (reverseWinding)
                                {
                                    startValue = 7 + windingPer - 1;
                                }
                            }
                            else
                            {
                                startValue = 6;
                                if (reverseWinding)
                                {
                                    startValue = 8 + windingPer - 1;
                                }
                            }
                        }
                        startValue %= 8;

                        switch (startValue)
                        {
                            case 1:
                                direction = -1;
                                nowAngle = (float)(Math.PI) * -1 + timer * speed * direction;//1

                                if ((nowAngle <= (float)(Math.PI) * -1 &&
                                    nowAngle >= (float)(Math.PI) * -1 * 3 / 2 - (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 2:
                                direction = 1;
                                nowAngle = (float)(Math.PI) * -1 + timer * speed * direction;//2

                                if ((nowAngle >= (float)(Math.PI) * -1 &&
                                    nowAngle <= (float)(Math.PI) * -1 / 2 + (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 3:
                                direction = 1;
                                nowAngle = (float)(Math.PI) * -1 * 3 / 2 + timer * speed * direction;//3

                                if ((nowAngle >= (float)(Math.PI) * -1 * 3 / 2 &&
                                    nowAngle <= (float)(Math.PI) * -1 + (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 4:
                                direction = -1;
                                nowAngle = (float)(Math.PI) * -1 / 2 + timer * speed * direction;//4

                                if ((nowAngle <= (float)(Math.PI) * -1 / 2 &&
                                    nowAngle >= (float)(Math.PI) * -1 - (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 5:
                                direction = 1;
                                nowAngle = (float)(Math.PI) * -1 / 2 + timer * speed * direction;//3

                                if ((nowAngle >= (float)(Math.PI) * -1 / 2 &&
                                    nowAngle <= 0 + (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 6:
                                direction = 1;
                                nowAngle = 0 + timer * speed * direction;//3

                                if ((nowAngle >= 0 &&
                                    nowAngle <= (float)(Math.PI) * 1 / 2 + (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 7:
                                direction = -1;
                                nowAngle = 0 - timer * speed * direction;//3

                                if ((nowAngle <= 0 &&
                                    nowAngle >= (float)(Math.PI) * -1 / 2 - (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                            case 8:
                                direction = -1;
                                nowAngle = (float)(Math.PI) * 1 / 2 - timer * speed * direction;//3

                                if ((nowAngle <= (float)(Math.PI) * 1 / 2 &&
                                    nowAngle >= 0 - (float)(Math.PI / 2) * windingPer))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                                break;
                        }

                        player.transform.position = mypos;
                        timer += Time.deltaTime;
                    }
                }
                else if (saikoro != 0)
                {
                    finished = false;
                    direction = 0;
                    timer = 0;
                    nowPos = player.transform.position;
                    if (minusPoint)
                    {
                        if (nowPoint - 1 > 0)
                        {
                            nowPoint--;
                        }
                        else
                        {
                            saikoro = 0;
                        }
                    }
                    else
                    {
                        nowPoint++;
                    }
                    if ((pointDatas[nowPoint].upperWinding && !right) || (!pointDatas[nowPoint].upperWinding && right))
                    {
                        direction = -1;
                    }
                    else if ((!pointDatas[nowPoint].upperWinding && !right) || (pointDatas[nowPoint].upperWinding && right))
                    {
                        direction = 1;
                    }
                    saikoro--;
                    if (nowPoint + 1 == pointDatas.Count) GameWave = すごろく.Goal;
                }
                else
                {
                    minusPoint = false;
                    timer = 0;
                    isPushed = false;
                    GameWave++;
                }
                break;
            case すごろく.Stop:
                GameWave += pointDatas[nowPoint].pointEffect;
                text.text += "\n何かのマスにとまった!!!";
                break;
            case すごろく.NormalPoint:
                GameWave = すごろく.Start;
                text.text += "何もなかった。";
                break;
            case すごろく.PlusPoint:
                saikoro++;
                GameWave = すごろく.Move;
                text.text += "\nプラスマス。1歩前進!!!";
                break;
            case すごろく.MinusPoint:
                minusPoint = true;
                GameWave = すごろく.Start;
                text.text += "\nマイナスマス。サイコロの分後退...";
                break;
            case すごろく.Goal:
                text.text = "GOAL!!!";
                break;
        }
    }

}
