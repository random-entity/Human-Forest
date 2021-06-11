using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Transform Quad_U_p_prefab;
    private Dictionary<Person, Transform> P2Quad_U; // RealOrImagePerson p => Quad for displaying U(p)
    private List<Person> realAndImageSociety;

    [SerializeField] private Transform GUIBoard;
    private Vector3 boardCenter;
    private float boardWidth, boardHeight;
    [SerializeField] private float padding = 1f;

    private void Start()
    {
        boardCenter = GUIBoard.position;
        boardWidth = GUIBoard.localScale.x;
        boardHeight = GUIBoard.localScale.y;

        realAndImageSociety = HumanForest.instance.RealAndImagesSociety;
        P2Quad_U = new Dictionary<Person, Transform>();

        int index = 0;
        foreach (Person p in realAndImageSociety)
        {
            Transform quad = Instantiate(Quad_U_p_prefab);
            P2Quad_U.Add(p, quad);
            quad.position = norm22V3(index / 12f, 0f, -1f);

            index++;
        }
    }

    private Vector3 norm22V3(float x, float y, float worldV3z)
    {
        float vx = Mathf.Lerp(boardCenter.x - boardWidth * 0.5f + padding, boardCenter.x + boardWidth * 0.5f - padding, x);
        float vy = Mathf.Lerp(boardCenter.y, boardCenter.y + boardHeight * 0.5f - padding, y);

        return new Vector3(vx, vy, worldV3z);
    }
}