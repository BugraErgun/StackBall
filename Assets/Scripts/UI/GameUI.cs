using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI;
    public GameObject allButtons;

    private bool btns;

    [Header("PreGame")]
    public Button soundBtn;
    public Sprite soundsOn, soundsOff;

    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevelImage;
    public Image nextLevelImage;

    private Material playerMat;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerMat = FindObjectOfType<Player>().transform.GetChild(0).GetComponent<MeshRenderer>().material;

        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color+Color.gray;
        levelSlider.color = playerMat.color;

        currentLevelImage.color = playerMat.color;
        nextLevelImage.color = playerMat.color;

        soundBtn.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
    }


    void Update()
    {
        if (player.playerState==Player.PlayerState.Prepare)
        {
            if (SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite !=soundsOn)
            {
                soundBtn.GetComponent<Image>().sprite = soundsOn;
            }
            else if(!SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundsOff)
            {
                soundBtn.GetComponent<Image>().sprite = soundsOff;
            }

        }

        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && player.playerState==Player.PlayerState.Prepare)
        {
            player.playerState = Player.PlayerState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);

        }
    }

    public bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<Ignore>() !=null)
            {
                raycastResults.RemoveAt(i);
                i--;

            }
        }

        print(raycastResults.Count);

        return raycastResults.Count > 0;
    }
    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void Settings()
    {
        btns = !btns;
        allButtons.SetActive(btns);

    } 
}
