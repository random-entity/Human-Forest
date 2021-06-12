using System;
using System.Collections.Generic;
using UnityEngine;

public class SVDisplayManager : MonoBehaviour
{
    private HumanForest humanForest;
    private List<Person> realAndImageSociety;

    [SerializeField] private Transform SVBoard;
    private Vector3 boardCenter;
    private float boardWidth, boardHeight;
    private float SVDisplayWidth = 1f;
    [SerializeField] private float padding = 1f;

    [SerializeField] private Transform SVDisplayPrefab;
    private Dictionary<Person, SVDisplay> SVDisplays; // RealOrImagePerson p => Quad for displaying U(p)

    private void Start()
    {
        humanForest = HumanForest.instance;

        boardCenter = SVBoard.position;
        boardWidth = SVBoard.localScale.x;
        boardHeight = SVBoard.localScale.y;

        realAndImageSociety = HumanForest.instance.RealAndImagesSociety;
        SVDisplays = new Dictionary<Person, SVDisplay>();

        int index = 0;
        foreach (Person p in realAndImageSociety)
        {
            Transform svTransform = Instantiate(SVDisplayPrefab);

            SVDisplay svd = svTransform.GetComponent<SVDisplay>();

            List<cloat2> sv = new List<cloat2>();
            foreach (Matter m in Enum.GetValues(typeof(Matter)))
            {
                sv.Add(humanForest.PM2SV[p][m]);
            }
            svd.SVList = sv;

            Debug.Log(object.ReferenceEquals(svd.SVList[0], humanForest.PM2SV[p][Matter.Test1]));
            Debug.Log(object.ReferenceEquals(svd.SVList[1], humanForest.PM2SV[p][Matter.Test2]));
            Debug.Log(object.ReferenceEquals(svd.SVList[2], humanForest.PM2SV[p][Matter.Test3]));
            Debug.Log(object.ReferenceEquals(svd.SVList[0], humanForest.PM2SV[p][Matter.Test2]));

            //            svd.SVList =;
            SVDisplays.Add(p, svd); // cloat2는 레퍼런스 타입이니까 Initialization 때 한 번만 불러도 충분하다는 희망이 있다. 그 희망은 실현되었다. 
            //됐는데 왜 안 되지. <- List를 새로 만들어서 svd.SVList 만들었고 hf.PM2SV[p][m]은 Dictionary...
            // <- object.ReferenceEquals로 검사하니 reference 같다는데?

            svTransform.gameObject.name = "SVDisp(" + p.gameObject.name + ")";

            svTransform.position = normXY2V3((float)(index % humanForest.InitialPersonCount + 0.5f) / (float)humanForest.InitialPersonCount, 0f, -1f);
            svTransform.SetParent(SVBoard);

            if (!p.IsReal) svTransform.gameObject.SetActive(false);

            index++;
        }

        UpdateSVDisplaySize();
    }

    #region OnUpdate SVDisplay Transform // SVDisplay가 갖고 있는 SVList의 Update에 의한 변화는 SVDisplay 클래스에서 처리할 것이다.
    public void UpdateSVDisplaySize()
    {
        foreach (Person p in humanForest.RealAndImagesSociety)
        {
            SVDisplay svd = SVDisplays[p];

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