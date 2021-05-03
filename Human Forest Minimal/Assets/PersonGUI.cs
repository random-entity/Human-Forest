using UnityEngine;
using UnityEngine.UI;

public class PersonGUI : MonoBehaviour
{
    public Image EmotionBar;
    [SerializeField] private Person person;

    private void UpdateEmotionBar()
    {
        EmotionBar.fillAmount = person.Emotion;
    }

    void Update()
    {
        UpdateEmotionBar();
    }
}
