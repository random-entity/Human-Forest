using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private HumanForest humanForest;

    [SerializeField] private Transform svDisplayPrefab;
    private Dictionary<Person, SVDisplay> svDisplays; // RealOrImagePerson p => Quad for displaying U(p)
    private List<Person> realAndImageSociety;

    [SerializeField] private Transform GUIBoard;
    private Vector3 boardCenter;
    private float boardWidth, boardHeight;
    [SerializeField] private float padding = 1f;

    private void Start()
    {
        humanForest = HumanForest.instance;

        boardCenter = GUIBoard.position;
        boardWidth = GUIBoard.localScale.x;
        boardHeight = GUIBoard.localScale.y;

        realAndImageSociety = HumanForest.instance.RealAndImagesSociety;
        svDisplays = new Dictionary<Person, SVDisplay>();

        int index = 0;
        foreach (Person p in realAndImageSociety)
        {
            Transform svTransform = Instantiate(svDisplayPrefab);
            svDisplays.Add(p, svTransform.GetComponent<SVDisplay>());

            svTransform.position = norm22V3(index / 12f, 0f, -1f);
            svTransform.SetParent(GUIBoard);

            UpdateSVList(p);

            index++;
        }
    }

    private void UpdateSVList(Person p)
    {
        SVDisplay svd = svDisplays[p];

        List<cloat2> psv = new List<cloat2>();

        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            // float s = humanForest.PM2State[p][m];
            // float v = humanForest.PM2Value[p][m];
            // psv.Add(new cloat2(s, v));
        }

        svd.SVList = psv;

    }

    private Vector3 norm22V3(float x, float y, float worldV3z)
    {
        float vx = Mathf.Lerp(boardCenter.x - boardWidth * 0.5f + padding, boardCenter.x + boardWidth * 0.5f - padding, x);
        float vy = Mathf.Lerp(boardCenter.y, boardCenter.y + boardHeight * 0.5f - padding, y);

        return new Vector3(vx, vy, worldV3z);
    }
}