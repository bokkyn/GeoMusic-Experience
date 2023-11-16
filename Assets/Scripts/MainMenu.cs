using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Canvas canvas;
    public Button mainMenuButton;

    public Canvas canvasAbout;
    public Button aboutButton;

    public Canvas canvasNear;
    public Button nearButton;

    public Canvas canvasData;
    public Button dataButton;


    private void Start()
    {
      
        canvas.enabled = false;
        canvasAbout.enabled = false;
        canvasNear.enabled = false;
        canvasData.enabled = false;


        Button btn = mainMenuButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        Button btn2 = aboutButton.GetComponent<Button>();
        btn2.onClick.AddListener(AboutChange);

        Button btn3 = nearButton.GetComponent<Button>();
        btn3.onClick.AddListener(NearChange);

        Button btn4 = dataButton.GetComponent<Button>();
        btn4.onClick.AddListener(DataChange);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        canvas.enabled = !canvas.enabled;
    }

    void AboutChange()
    {
        canvasAbout.enabled = !canvasAbout.enabled;
        canvasNear.enabled = false;
        canvasData.enabled = false;
    }

    void NearChange()
    {
        canvasNear.enabled = !canvasNear.enabled;
        canvasAbout.enabled = false;
        canvasData.enabled = false;
    }

    void DataChange()
    {
        canvasData.enabled = !canvasData.enabled;
        canvasNear.enabled = false;
        canvasAbout.enabled = false;
    }


}