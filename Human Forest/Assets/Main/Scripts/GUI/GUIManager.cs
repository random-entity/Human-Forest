using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private HumanForest humanForest;

    [SerializeField] private Transform SVDisplayPrefab;
    private Dictionary<Person, SVDisplay> svDisplays; // RealOrImagePerson p => Quad for displaying U(p)
    private List<Person> realAndImageSociety;

    [SerializeField] private Transform GUIBoard;
    private Vector3 boardCenter;
    private float boardWidth, boardHeight;
    private float svDisplayWidth = 1f;
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
            Transform svTransform = Instantiate(SVDisplayPrefab);
            svDisplays.Add(p, svTransform.GetComponent<SVDisplay>());

            svTransform.position = norm22V3(index / 12f, 0f, -1f);
            svTransform.SetParent(GUIBoard);

            SetSVListSize(p);
            UpdateSVList(p);

            index++;
        }
    }

    private void SetSVListSize(Person p)
    {
        SVDisplay svd = svDisplays[p];

        svd.BorderBottomLeft.position = svd.transform.position - new Vector3(0.5f * svDisplayWidth, 0.5f * svDisplayWidth, 0f);
        svd.BorderTopRight.position = svd.transform.position + new Vector3(0.5f * svDisplayWidth, 0.5f * svDisplayWidth, 0f);
    }

    private void UpdateSVList(Person p) // cloat2는 레퍼런스 타입이니까 Initialization 때 한 번만 불러도 충분하다는 희망이 있다.
    {
        SVDisplay svd = svDisplays[p];

        List<cloat2> psv = new List<cloat2>();

        foreach (Matter m in Enum.GetValues(typeof(Matter)))
        {
            // float s = humanForest.PM2State[p][m];
            // float v = humanForest.PM2Value[p][m];
            psv.Add(humanForest.PM2SV[p][m]);
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