using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
enum �����낭
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
    bool upperWinding = true;
    bool isPushed = false;
    float timer = 0;
    float windingPer = 1;
    int minusLower = 0;
    int nowPoint = 0;
    int saikoro;
    �����낭 GameWave;
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
        GameWave = �����낭.Lobby;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameWave)
        {
            default:
                GameWave = �����낭.NormalPoint;
                break;
            case �����낭.Lobby:
                if (Input.GetAxis("Jump") > 0)
                {
                    GameWave++;
                    text.text = "Start!!!";
                }
                break;
            case �����낭.Start:
                if (isPushed) return;
                if (Input.GetKeyUp(KeyCode.Space)) isPushed = true;
                if (isPushed)
                {
                    saikoro = UnityEngine.Random.Range(0, 6) + 1;
                    text.text += "\n�T�C�R���̖ڂ�" + (saikoro);
                    if (nowPoint == pointDatas.Count)
                    {
                        GameWave = �����낭.Goal;
                    }
                    else
                    {
                        if (!pointDatas[nowPoint].isLower)
                        {
                            minusLower = -1;
                            if (pointDatas[nowPoint].totalAngle < 0)
                            {
                                upperWinding = true;

                            }
                            else
                            {
                                minusLower = 1;
                                upperWinding = false;
                            }
                        }
                        else
                        {
                            minusLower = 1;
                            if (pointDatas[nowPoint].totalAngle < 0)
                            {
                                upperWinding = true;

                            }
                            else
                            {
                                minusLower = -1;
                                upperWinding = false;
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
                        GameWave = �����낭.Move;
                        nowPos = player.transform.position;
                        text.text += "\n�ړ���...";
                    }
                }
                break;
            case �����낭.Move:
                if (pointDatas[nowPoint].totalAngle > 90 || pointDatas[nowPoint].totalAngle < -90)
                {
                    windingPer = pointDatas[nowPoint].totalAngle / 360;
                }
                else
                {
                    windingPer = 0;
                }
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
                        if (pointDatas[nowPoint].isLower)
                        {
                            center = new Vector3(pointB.x, pointA.y, pointA.z);
                        }
                        else
                        {
                            center = new Vector3(pointB.x, pointA.y, pointA.z);
                        }
                        float l = Vector3.Distance(pointA, center);
                        float m = Vector3.Distance(pointB, center);
                        float nowAngle = 0;

                        if (!pointDatas[nowPoint].isLower)
                        {
                            if (upperWinding)//-PI�𒆐S�Ƃ����E��ɉ�]
                            {
                                nowAngle = (float)(Math.PI * -1) + timer * speed * minusLower;
                                if (nowAngle <= (float)(Math.PI) * -1 && nowAngle >= (float)(Math.PI) * -1 * 3 / 2 - (windingPer * (float)(Math.PI * 2)))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                            else//-PI�𒆐S�Ƃ����E���ɉ�]
                            {
                                nowAngle = (float)(Math.PI * -1) + timer * speed * minusLower;
                                if (nowAngle >= (float)(Math.PI) * -1 && nowAngle <= (float)(Math.PI) * -1 / 2 + (windingPer * (float)(Math.PI * 2)))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                        }
                        else
                        {
                            if (upperWinding)//0�𒆐S�Ƃ�������ɉ�]
                            {
                                nowAngle = (float)(Math.PI * 0) + timer * speed * minusLower;
                                if (nowAngle >= (float)(Math.PI) * 0 && nowAngle <= (float)(Math.PI) * 1 / 2 + (windingPer * (float)(Math.PI * 2)))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
                                }
                                else
                                {
                                    finished = true;
                                }
                            }
                            else//0�𒆐S�Ƃ��������ɉ�]
                            {
                                nowAngle = (float)(Math.PI * 0) + timer * speed * minusLower;
                                if (nowAngle <= (float)(Math.PI) * 0 && nowAngle <= (float)(Math.PI) * -1 / 2 - (windingPer * (float)(Math.PI * 2)))
                                {
                                    mypos.x = l * Mathf.Cos(nowAngle) + center.x;
                                    mypos.z = m * Mathf.Sin(nowAngle) + center.z;
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
                    windingPer = 1;
                    finished = false;
                    minusLower = 0;
                    timer = 0;
                    nowPos = player.transform.position;
                    nowPoint++;
                    int i = 0;
                    if (pointDatas[nowPoint].totalAngle < 0)
                    {
                        minusLower = -1;
                        upperWinding = true;
                        while (i < 4 && !(pointDatas[nowPoint].totalAngle <= 90 * -i))
                        {
                            minusLower--;
                            i++;
                        }
                    }
                    else
                    {
                        minusLower = 1;
                        upperWinding = false;
                        while (i < 4 && !(pointDatas[nowPoint].totalAngle >= 90 * i))
                        {
                            minusLower++;
                            i++;
                        }
                    }
                    if (nowPoint < 0)
                    {
                        if (upperWinding)
                        {
                            upperWinding = false;
                        }
                        else
                        {
                            upperWinding = true;
                        }
                    }
                    saikoro--;
                    if (nowPoint + 1 == pointDatas.Count) GameWave = �����낭.Goal;
                }
                else
                {
                    timer = 0;
                    isPushed = false;
                    GameWave++;
                }
                break;
            case �����낭.Stop:
                GameWave += pointDatas[nowPoint].pointEffect;
                text.text += "\n�����̃}�X�ɂƂ܂���!!!";
                break;
            case �����낭.NormalPoint:
                GameWave = �����낭.Start;
                text.text += "�����Ȃ������B";
                break;
            case �����낭.PlusPoint:
                saikoro++;
                GameWave = �����낭.Move;
                text.text += "\n�v���X�}�X�B1���O�i!!!";
                break;
            case �����낭.MinusPoint:
                saikoro++;
                nowPoint -= 2;
                GameWave = �����낭.Move;
                text.text += "\n�}�C�i�X�}�X�B1�����...\n�ړ���...";
                break;
            case �����낭.Goal:
                text.text = "GOAL!!!";
                break;
        }
    }

}
