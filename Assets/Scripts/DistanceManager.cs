using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Image Tracking manager that detects tracked images")]
    private ImageRecognition imageRecognitionManager;

    public ImageRecognition ImageRecognitionManager
    {
        get => imageRecognitionManager;
        set => this.imageRecognitionManager = value;
    }
    
    public bool IsItOver { get; private set; } = false;

    [SerializeField] private float consideredDistance = 0.01f;

    private static readonly Color Green = Color.green;
    private static readonly Color Red = Color.red;
    private readonly List<bool> _okConn = new List<bool> {false, false, false, false, false, false, false};

    private void Update()
    {
        if (_okConn.All(conn => conn))
        {
            IsItOver = true;
            return;
        }
        
        var spawnedObjects = imageRecognitionManager.SpawnedObjects;

        spawnedObjects.TryGetValue("BottomLeft", out var bl);
        spawnedObjects.TryGetValue("BottomMid", out var bm);
        spawnedObjects.TryGetValue("BottomRight", out var br);
        spawnedObjects.TryGetValue("UpperLeft", out var ul);
        spawnedObjects.TryGetValue("UpperMid", out var um);
        spawnedObjects.TryGetValue("UpperRight", out var ur);

        if (bl.activeSelf)
        {
            if (bm.activeSelf)
            {
                _okConn[0] = DistanceHandler(bl, bm, "Right");
            }

            if (ul.activeSelf)
            {
                _okConn[1] = DistanceHandler(bl, ul, "Up");
            }
        }

        if (bm.activeSelf)
        {
            if (um.activeSelf)
            {
                _okConn[2] = DistanceHandler(bm, um, "Up");
            }

            if (br.activeSelf)
            {
                _okConn[3] = DistanceHandler(bm, br, "Right");
            }
        }

        if (br.activeSelf && ur.activeSelf)
        {
            _okConn[4] = DistanceHandler(br, ur, "Up");
        }

        if (um.activeSelf)
        {
            if (ul.activeSelf)
            {
                _okConn[5] = DistanceHandler(um, ul, "Left");
            }

            if (ur.activeSelf)
            {
                _okConn[6] = DistanceHandler(um, ur, "Right");
            }
        }
    }

    private bool DistanceHandler(GameObject g1, GameObject g2, string direction)
    {
        var g1d = GetDirectionalSphere(g1, direction);
        var g2d = GetDirectionalSphere(g2, GetOpposite(direction));

        var distance = Vector3.Distance(g1d.transform.position, g2d.transform.position);

        var ok = distance <= consideredDistance;
        SetColor(g1d, g2d, ok);
        return ok;
    }

    private static string GetOpposite(string direction)
    {
        return direction switch
        {
            "Right" => "Left",
            "Left" => "Right",
            "Up" => "Down",
            "Down" => "Up",
            _ => throw new Exception("")
        };
    }

    private static void SetColor(GameObject go, GameObject go2, bool ac)
    {
        go.GetComponent<MeshRenderer>().material.color = ac ? Green : Red;
        go2.GetComponent<MeshRenderer>().material.color = ac ? Green : Red;
    }

    private static GameObject GetDirectionalSphere(GameObject imgPanel, string direction)
    {
        return imgPanel.transform.Find(direction).gameObject;
    }
}