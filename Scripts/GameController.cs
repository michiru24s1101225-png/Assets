using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
enum すごろく
{
    Lobby,
    Start,
    Move,
    Next,
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
                        text.text += "\n移動中...";
                    }
                }
                break;
            case すごろく.Move:
                if (!minusPoint)
                {
                    reverseWinding = false;
                }
                if (saikoro == 0)
                {
                    GameWave++;
                    break;
                }
                float totalTime = 1;
                windingPer = pointDatas[nowPoint].windingPer;
                Vector3 pointA = pointDatas[nowPoint].transform.position;
                Vector3 pointB = Vector3.zero;
                if (reverseWinding)
                {
                    pointB = pointDatas[nowPoint - 1].transform.position;
                    right = pointDatas[nowPoint - 1].upperWinding;
                }
                else
                {
                    pointB = pointDatas[nowPoint + 1].transform.position;
                    right = pointDatas[nowPoint].upperWinding;
                }
                nowPos = player.transform.position;
                if ((Vector3.Distance(pointB, nowPos) > 0.05f && !finished) && saikoro != 0)
                {
                    if (pointDatas[nowPoint].windingPer == 0)
                    {
                        player.transform.position += ((pointB - nowPos) / totalTime) * Time.deltaTime;
                    }
                    else
                    {
                        float speed = 3;
                        Vector3 center = Vector3.zero;
                        center = new Vector3(pointB.x, pointA.y, pointA.z);
                        float l = Vector3.Distance(pointA, center);
                        float m = Vector3.Distance(pointB, center);
                        float nowAngle = 0;

                        /*
                         * (not reverse)startValue:1,2,3,4
                         * 1:nowAngle=スタート地点が-πかつ進行方向が-direction
                         * 2:nowAngle=スタート地点が-πかつ進行方向が+direction
                         * 3:nowAngle=スタート地点が-1/2πかつ進行方向が+direction
                         * 4:nowAngle=スタート地点が0かつ進行方向が+direction
                         * (reverse...not reverse時の1,2,3,4と進行方向が反対になる)
                         * startValue:1,2,3,4
                         * 1:nowAngle=スタート地点が-3/2πかつ進行方向が+direction
                         * 2:nowAngle=スタート地点が-1/2πかつ進行方向が-direction
                         * 3:nowAngle=スタート地点が0かつ進行方向が-direction
                         * 4:nowAngle=スタート地点が1/2πかつ進行方向が-direction
                         */

                        int startValue = 0;
                        if ((!pointDatas[nowPoint].isLower && !reverseWinding))//単位円の右側か左側か
                        {
                            if (right)
                            {
                                startValue = 1;
                            }
                            else
                            {
                                startValue = 2;
                            }
                        }
                        else
                        {
                            if (right)
                            {
                                startValue = 3;
                            }
                            else
                            {
                                startValue = 4;
                            }
                        }
                        if (reverseWinding)
                        {
                            startValue = windingPer;
                        }
                        Debug.Log("sv"+startValue+"&np"+nowPoint);
                        if (reverseWinding)
                        {
                            switch (startValue)
                            {
                                case 1:
                                    direction = 1;
                                    nowAngle = (float)(Math.PI * -1) * 3 / 2 + timer * speed * direction;

                                    if ((nowAngle >= (float)(Math.PI * -1) * 3 / 2 &&
                                        nowAngle <= (float)(Math.PI * -1) + (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                                case 2:
                                    direction = -1;
                                    nowAngle = (float)(Math.PI * -1) / 2 + timer * speed * direction;

                                    if ((nowAngle <= (float)(Math.PI * -1) / 2 &&
                                        nowAngle >= (float)(Math.PI * -1) - (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                                case 3:
                                    direction = -1;
                                    nowAngle = 0 + timer * speed * direction;

                                    if ((nowAngle <= 0 &&
                                        nowAngle >= (float)(Math.PI * -1) / 2 - (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                                case 4:
                                    direction = -1;
                                    nowAngle = (float)(Math.PI * 1) / 2 + timer * speed * direction;

                                    if ((nowAngle <= (float)(Math.PI * 1) / 2 &&
                                        nowAngle >= 0 - (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (startValue)
                            {
                                case 1:
                                    direction = -1;
                                    nowAngle = (float)(Math.PI * -1) + timer * speed * direction;

                                    if ((nowAngle <= (float)(Math.PI * -1) &&
                                        nowAngle >= (float)(Math.PI * -1) * 3 / 2 -
                                        (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                                case 2:
                                    direction = 1;
                                    nowAngle = (float)(Math.PI * -1) + timer * speed * direction;

                                    if ((nowAngle >= (float)(Math.PI * -1) &&
                                        nowAngle <= (float)(Math.PI * -1) / 2 +
                                        (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                                case 3:
                                    direction = 1;
                                    nowAngle = (float)(Math.PI * -1) / 2 + timer * speed * direction;

                                    if ((nowAngle >= (float)(Math.PI * -1) / 2 &&
                                        nowAngle <= 0 + (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                                case 4:
                                    direction = 1;
                                    nowAngle = 0 + timer * speed * direction;

                                    if ((nowAngle >= 0 &&
                                        nowAngle <= (float)(Math.PI * 1) / 2 +
                                        (float)(Math.PI / 2) * windingPer))
                                    {
                                        nowPos.x = l * Mathf.Cos(nowAngle) + center.x;
                                        nowPos.z = m * Mathf.Sin(nowAngle) + center.z;
                                    }
                                    else
                                    {
                                        finished = true;
                                    }
                                    break;
                            }
                        }

                        player.transform.position = nowPos;
                        timer += Time.deltaTime;
                    }
                }
                else
                {
                    GameWave++;
                }
                break;
            case すごろく.Next:
                if (saikoro != 0)
                {
                    finished = false;
                    direction = 0;
                    timer = 0;
                    nowPos = player.transform.position;
                    if (minusPoint)
                    {
                        nowPoint--;
                        Debug.Log("n" + nowPoint);
                    }
                    else
                    {
                        nowPoint++;
                        Debug.Log("n" + nowPoint);
                    }
                    saikoro--;
                    Debug.Log("s" + saikoro);
                    if (nowPoint + 1 == pointDatas.Count) GameWave = すごろく.Goal;
                    GameWave--;
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
