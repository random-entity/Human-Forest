using System;
using System.Collections.Generic;
using UnityEngine;

public class SVDisplayManager : MonoBehaviour
{
    private HumanForest hf;

    [SerializeField] private Transform SVBoard;
    private Vector3 boardCenter;
    private float boardWidth, boardHeight;
    private float SVDisplayWidth = 1f;
    [SerializeField] private float padding = 1f;

    [SerializeField] private Transform SVDisplayPrefab;
    public Dictionary<Person, SVDisplay> SVDisplays_U_p; // RealOrImagePerson p => Quad for displaying U(p)

    private void Start()
    {
        hf = HumanForest.instance;

        boardCenter = SVBoard.position;
        boardWidth = SVBoard.localScale.x;
        boardHeight = SVBoard.localScale.y;

        SVDisplays_U_p = new Dictionary<Person, SVDisplay>();

        int index = 0;
        foreach (Person p in hf.RealAndImagesSociety)
        {
            Transform svTransform = Instantiate(SVDisplayPrefab);
            svTransform.gameObject.name = "SVDisp(" + p.gameObject.name + ")";
            svTransform.position = normXY2V3((float)(index % hf.InitialPersonCount + 0.5f) / (float)hf.InitialPersonCount, 0f, -1f);
            svTransform.SetParent(SVBoard);

            SVDisplay svd = svTransform.GetComponent<SVDisplay>();

            List<cloat2> sv = new List<cloat2>();
            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                sv.Add(hf.PM2SV[p][m]);
            }
            svd.SVList = sv;

            SVDisplays_U_p.Add(p, svd);

            if (!p.IsReal) svTransform.gameObject.SetActive(false);

            index++;
        }

        UpdateSVDisplaySize();
    }

    #region OnUpdate SVDisplay Transform 
    // SVDisplay가 갖고 있는 SVList의 Update에 의한 변화는 SVDisplay 클래스에서 처리할 것이다.
    public void UpdateSVDisplaySize()
    {
        foreach (Person p in hf.RealAndImagesSociety)
        {
            SVDisplay svd = SVDisplays_U_p[p];

            svd.BorderBottomLeft.position = svd.transform.position - new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
            svd.BorderTopRight.position = svd.transform.position + new Vector3(0.5f * SVDisplayWidth, 0.5f * SVDisplayWidth, 0f);
        }
    }
    #endregion

    #region Translation: Normalized space to Vector3 space
    private Vector3 normXY2V3(float x, float y, float worldV3z)
    {
        float vx = Mathf.Lerp(boardCenter.x - boardWidth * 0.5f + padding, boardCenter.x + boardWidth * 0.5f - padding, x);
        float vy = Mathf.Lerp(boardCenter.y, boardCenter.y + boardHeight * 0.5f - padding, y);

        return new Vector3(vx, vy, worldV3z);
    }
    #endregion
}