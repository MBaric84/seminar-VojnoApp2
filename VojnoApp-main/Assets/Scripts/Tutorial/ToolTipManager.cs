using System.Collections;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField]
    private GameObject toolTipPanel;

    [SerializeField]
    private TextMeshProUGUI toolTipText;

    [SerializeField]
    private Camera mainCamera;

    private string[] data;
    private int toolTipCounter = 0;
    private ToolTipFlags toolTipFlags;
    private bool isToolTipSet = false;

    //struct Tool Tip flags
    private struct ToolTipFlags
    {
        public bool isWPressed;
        public bool isSPressed;
        public bool isAPressed;
        public bool isDPressed;
        public bool isCPressed;
        public bool isSpacePressed;
        public bool isLeftMousePressed;
        public bool isRightMousePressed;
        public bool isRPressed;
        public bool isMosueMovedUP;
        public bool isMosueMovedDown;
    }

    // Start is called before the first frame update
    void Start()
    {
        toolTipPanel.SetActive(true);
        TextAsset txtData = Resources.Load<TextAsset>("ToolTipData");
        data = txtData.text.Split(new char[] { '\n' });
        toolTipCounter = 0;
        SetToolTip(toolTipCounter);
        //set tool tip flags to false
        toolTipFlags = new ToolTipFlags();
        ResetToolTipFlags();
    }

    private void ResetToolTipFlags()
    {
        toolTipFlags.isWPressed = false;
        toolTipFlags.isSPressed = false;
        toolTipFlags.isAPressed = false;
        toolTipFlags.isDPressed = false;
        toolTipFlags.isCPressed = false;
        toolTipFlags.isSpacePressed = false;
        toolTipFlags.isLeftMousePressed = false;
        toolTipFlags.isRightMousePressed = false;
        toolTipFlags.isRPressed = false;
        toolTipFlags.isMosueMovedUP = false;
        toolTipFlags.isMosueMovedDown = false;
    }



    // Update is called once per frame
    void Update()
    {
        if(isToolTipSet)
        {
            KeyChecker();
        }
    }

    private void KeyChecker()
    {
        if (toolTipCounter == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                toolTipFlags.isWPressed = true;
                return;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                toolTipFlags.isSPressed = true;
                return;
            }

            if (toolTipFlags.isWPressed && toolTipFlags.isSPressed)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 1)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                toolTipFlags.isAPressed = true;
                return;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                toolTipFlags.isDPressed = true;
                return;
            }

            if (toolTipFlags.isAPressed && toolTipFlags.isDPressed)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 2)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                toolTipFlags.isCPressed = true;
                return;
            }

            if (toolTipFlags.isCPressed)
            {
                FinishToolTip(3f);
                return;
            }
        }


        if (toolTipCounter == 3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                toolTipFlags.isSpacePressed = true;
                return;
            }

            if (toolTipFlags.isSpacePressed)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 4)
        {
            if (Input.GetMouseButtonDown(0))
            {
                toolTipFlags.isLeftMousePressed = true;
                return;
            }

            if (toolTipFlags.isLeftMousePressed)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 5)
        {
            if (Input.GetMouseButtonDown(1))
            {
                toolTipFlags.isRightMousePressed = true;
                return;
            }

            if (toolTipFlags.isRightMousePressed)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 6)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                toolTipFlags.isRPressed = true;
                return;
            }

            if (toolTipFlags.isRPressed)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 7)
        {
            if (Vector3.Angle(mainCamera.transform.forward, Vector3.up) > 70f)
            {
                toolTipFlags.isMosueMovedUP = true;
                return;
            }

            if (toolTipFlags.isMosueMovedUP)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 8)
        {
            if (Vector3.Angle(mainCamera.transform.forward, Vector3.up) < 50f)
            {
                toolTipFlags.isMosueMovedDown = true;
                return;
            }

            if (toolTipFlags.isMosueMovedDown)
            {
                FinishToolTip(3f);
                return;
            }
        }

        if (toolTipCounter == 9)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                toolTipCounter = -1;
                ResetToolTipFlags();
                FinishToolTip(0.5f);
                return;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                toolTipPanel.SetActive(false);
                isToolTipSet = false;
                return;
            }
        }
    }


    private void SetToolTip(int counter) {         
        for (int i = 0; i < data.Length; i++)
        {
            toolTipText.text = data[counter];
            isToolTipSet = true;
        }
    }

    private void FinishToolTip(float time)
    {
        isToolTipSet = false;
        toolTipCounter++;  
        StartCoroutine(ShowToolTipAfterTime(time));
    }

    private IEnumerator ShowToolTipAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SetToolTip(toolTipCounter);
    }
}
