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

enum PIPer
{
    PI2,//90*
    PI,//180*
    PIMinus2,//-90*
    PIMinus1//-180*
}
public class GameController : MonoBehaviour
{
    float timer = 0;
    bool finished = false;
    bool plus = true;
    bool isPushed = false;
    int Angle;
    int a = 1;
    int minusLower = 0;
    int nowPoint = 0;
    int saikoro;
    すごろく GameWave;
    PIPer pIPer;
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
                    saikoro = UnityEngine.Random.Range(0, 6) + 1;
                    text.text += "\nサイコロの目は" + (saikoro);
                    if (nowPoint == pointDatas.Count)
                    {
                        GameWave = すごろく.Goal;
                    }
                    else
                    {
                        if (!pointDatas[nowPoint].isUpper)
                        {
                            minusLower = -1;
                            if (pointDatas[nowPoint].totalAngle < 0)
                            {
                                plus = true;

                            }
                            else
                            {
                                minusLower = 1;
                                plus = false;
                            }
                        }
                        else
                        {
                            minusLower = 1;
                            if (pointDatas[nowPoint].totalAngle < 0)
                            {
                                plus = true;

                            }
                            else
                            {
                                minusLower = -1;
                                plus = false;
                            }
                        }
                        /*
                        int i = 0;
                        if (pointDatas[nowPoint].totalAngle < 0)
                        {
                            PICount = -1;
                            minus = true;
                            while (i < 4 && !(pointDatas[nowPoint].totalAngle <= 90 * -i))
                            {
                                PICount--;
                                i++;
                            }
                        }
                        else
                        {
                            PICount = 1;
                            minus = false;
                            while (i < 4 && !(pointDatas[nowPoint].totalAngle >= 90 * i))
                            {
                                PICount++;
                                i++;
                            }
                        }
                        */
                        GameWave = すごろく.Move;
                        nowPos = player.transform.position;
                        text.text += "\n移動中...";
                    }
                }
                break;
            case すごろく.Move:
                Angle = Mathf.Abs(pointDatas[nowPoint].totalAngle) / 90;
                float totalTime = 1;
                float x1 = player.transform.position.x;
                float z1 = player.transform.position.z;
                float x2 = pointDatas[nowPoint + 1].transform.position.x;
                float z2 = pointDatas[nowPoint + 1].transform.position.z;
                if ((Mathf.Sqrt((x2 - x1) * (x2 - x1) + (z2 - z1) * (z2 - z1)) > 0.01f && !finished) && saikoro != 0)
                {
                    if (pointDatas[nowPoint].totalAngle == 0)
                    {
                        player.transform.position += ((pointDatas[nowPoint + 1].transform.position - nowPos) / totalTime) * Time.deltaTime;
                    }
                    else
                    {
                        float speed = 2;
                        Vector3 pointA = pointDatas[nowPoint].transform.position;
                        Vector3 pointB = pointDatas[nowPoint + 1].transform.position;
                        Vector3 mypos = player.transform.position;
                        Vector3 center = Vector3.zero;
                        if (pointDatas[nowPoint].isUpper)
                        {
                            center = new Vector3(pointB.x, pointA.y, pointA.z);
                        }
                        else
                        {
                            center = new Vector3(pointA.x, pointA.y, pointB.z);
                        }
                        float l = Vector3.Distance(pointA, center);
                        float m = Vector3.Distance(pointB, center);
                        float nowAngle = 0;
                        float pi = 0;

                        if (!pointDatas[nowPoint].isUpper)
                        {
                            if (plus)
                            {
                                switch (a)
                                {
                                    case 1:
                                        pi = (float)(Math.PI * -1) * 3 / 2; break;
                                    case 2:
                                        pi = (float)(Math.PI * -1); break;
                                    case 3:
                                        pi = (float)(Math.PI * -1) / 2; break;
                                    default:
                                        pi = (float)(Math.PI * -1) * 3 / 2; break;
                                }
                                nowAngle = (float)(Math.PI * -1) + timer * speed * minusLower;
                                if (nowAngle <= (float)(Math.PI) * -1 && nowAngle >= pi)
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else if (a != Angle)
                                {
                                    a++;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                            else
                            {
                                switch (a)
                                {
                                    case 1:
                                        pi = (float)(Math.PI * -1) / 2; break;
                                    case 2:
                                        pi = (float)(Math.PI * 1); break;
                                    case 3:
                                        pi = (float)(Math.PI * 1) * 3 / 2; break;
                                    default:
                                        pi = (float)(Math.PI * -1) / 2; break;
                                }
                                nowAngle = (float)(Math.PI * -1) + timer * speed * minusLower;
                                if (nowAngle >= (float)(Math.PI) * -1 && nowAngle <= pi)
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else if (a != Angle)
                                {
                                    a++;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                        }
                        else
                        {
                            if (plus)
                            {
                                switch (a)
                                {
                                    case 1:
                                        pi = (float)(Math.PI * -1) * 3 / 2; break;
                                    case 2:
                                        pi = (float)(Math.PI * -1); break;
                                    case 3:
                                        pi = (float)(Math.PI * -1) / 2; break;
                                    default:
                                        pi = (float)(Math.PI * -1) * 3 / 2; break;
                                }
                                nowAngle = (float)(Math.PI * -1) + timer * speed * minusLower;
                                if (nowAngle <= (float)(Math.PI) * -1 && nowAngle >= pi)
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else if (a != Angle)
                                {
                                    a++;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                            else
                            {
                                switch (a)
                                {
                                    case 1:
                                        pi = (float)(Math.PI * -1) / 2; break;
                                    case 2:
                                        pi = (float)(Math.PI * 1); break;
                                    case 3:
                                        pi = (float)(Math.PI * 1) * 3 / 2; break;
                                    default:
                                        pi = (float)(Math.PI * -1) / 2; break;
                                }
                                nowAngle = (float)(Math.PI * -1) + timer * speed * minusLower;
                                if (nowAngle >= (float)(Math.PI) * -1 && nowAngle <= pi)
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else if (a != Angle)
                                {
                                    a++;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                        }
                        player.transform.position = mypos;
                        timer += Time.deltaTime;
                    }
                }
                else if (saikoro != 0)
                {
                    a = 1;
                    finished = false;
                    minusLower = 0;
                    timer = 0;
                    nowPos = player.transform.position;
                    nowPoint++;
                    int i = 0;
                    if (pointDatas[nowPoint].totalAngle < 0)
                    {
                        minusLower = -1;
                        plus = true;
                        while (i < 4 && !(pointDatas[nowPoint].totalAngle <= 90 * -i))
                        {
                            minusLower--;
                            i++;
                        }
                    }
                    else
                    {
                        minusLower = 1;
                        plus = false;
                        while (i < 4 && !(pointDatas[nowPoint].totalAngle >= 90 * i))
                        {
                            minusLower++;
                            i++;
                        }
                    }
                    if (nowPoint < 0)
                    {
                        if (plus)
                        {
                            plus = false;
                        }
                        else
                        {
                            plus = true;
                        }
                    }
                    saikoro--;
                    if (nowPoint + 1 == pointDatas.Count) GameWave = すごろく.Goal;
                }
                else
                {
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
                saikoro++;
                nowPoint -= 2;
                GameWave = すごろく.Move;
                text.text += "\nマイナスマス。1歩後退...\n移動中...";
                break;
            case すごろく.Goal:
                text.text = "GOAL!!!";
                break;
        }
    }

}
