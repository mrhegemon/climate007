using Maestro;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MaestroBLEUI : MonoBehaviour
{
    private static MaestroBLEUI instance;

    public Transform watcher, left, right, process, leftConnected, rightConnected;

    private float elapsed;
    public float tick = 0.1f;

    [DllImport("MaestroAPI")]
    public static extern bool is_ble_processing();

    [DllImport("MaestroAPI")]
    public static extern bool is_ble_watcher_running();

    [DllImport("MaestroAPI")]
    public static extern bool is_ble_left_connecting();

    [DllImport("MaestroAPI")]
    public static extern bool is_ble_right_connecting();

    public int history = 5;
    private List<Vector3> rightHistory, leftHistory;

    private MaestroHand rightHand, leftHand;
    private MaestroGloveBehaviour rightGlove, leftGlove;
    private Camera mainCamera;

    private GameObject leftPanel, rightPanel;
    private TextMesh leftText, rightText;

    public bool showPanels = true;
    private bool overrideRightText = false, overrideLeftText = false;
    public Vector3 panelOffset = new Vector3(0, 0.2f, 0);
    public GameObject panelPrefab;

    private int ellipsisTicks = 2, currentTicks = 0, currentEllipsis = 3;

    public static void SetLeftText(string text)
    {
        instance.overrideLeftText = true;
        SetText(text, instance.leftText);
    }

    public static void SetRightText(string text)
    {
        instance.overrideRightText = true;
        SetText(text, instance.rightText);
    }
    
    private static void SetText(string text, TextMesh panel)
    {
        if (panel != null)
            panel.text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        leftHistory = new List<Vector3>();
        rightHistory = new List<Vector3>();

        watcher.gameObject.SetActive(false);
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        process.gameObject.SetActive(false);
        leftConnected.gameObject.SetActive(false);
        rightConnected.gameObject.SetActive(false);

        // Find active left/right hands
        MaestroHand[] hands = GameObject.FindObjectsOfType<MaestroHand>();
        foreach(MaestroHand hand in hands)
        {
            if (hand.whichHand == WhichHand.LeftHand) {
                if (leftHand == null)
                    leftHand = hand;
                else continue;
            } else {
                if (rightHand == null)
                    rightHand = hand;
                else continue;
            }
        }

        // Get gloves
        rightGlove = rightHand.gameObject.GetComponentInParent<MaestroGloveBehaviour>();
        leftGlove = leftHand.gameObject.GetComponentInParent<MaestroGloveBehaviour>();

        // Find main camera
        mainCamera = GameObject.FindObjectOfType<Camera>();
        if (mainCamera == null)
            Debug.LogError("Main Camera not found!");

        // Spawn panels
        leftPanel = Instantiate(panelPrefab);
        rightPanel = Instantiate(panelPrefab);

        // Get Text meshes
        leftText = leftPanel.GetComponentInChildren<TextMesh>();
        leftText.text = "Please wait...\r\nLeft Connecting";
        rightText = rightPanel.GetComponentInChildren<TextMesh>();

        // Init history
        RecordHistory();

        // Init panel positions
        OrientPanel(leftPanel, getAverage(leftHistory));
        OrientPanel(rightPanel, getAverage(rightHistory));
    }

    private void RecordHistory()
    {
        rightHistory.Add(rightHand.Palm.transform.position);
        while (rightHistory.Count > history)
            rightHistory.RemoveAt(0);

        leftHistory.Add(leftHand.Palm.transform.position);
        while (leftHistory.Count > history)
            leftHistory.RemoveAt(0);
    }
    
    private void UpdateUI()
    {
        watcher.gameObject.SetActive(is_ble_watcher_running());
        left.gameObject.SetActive(is_ble_left_connecting());
        right.gameObject.SetActive(is_ble_right_connecting());
        process.gameObject.SetActive(is_ble_processing());

        leftConnected.gameObject.SetActive(leftGlove.Connected);
        rightConnected.gameObject.SetActive(rightGlove.Connected);
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= tick)
        {
            elapsed = 0f;
            currentTicks++;
            if (currentTicks > ellipsisTicks)
            {
                currentEllipsis++;
                if (currentEllipsis > 3)
                    currentEllipsis = 0;
                currentTicks = 0;
            }

            UpdateUI();
        }

        // Always update panels
        if (showPanels || overrideRightText || overrideLeftText)
        {
            RecordHistory();

            OrientPanel(leftPanel, getAverage(leftHistory));
            OrientPanel(rightPanel, getAverage(rightHistory));

            string ellipsis = getEllipsis(currentEllipsis);

            bool connected = leftGlove.Connected;
            leftPanel.SetActive((showPanels && !connected) || overrideLeftText);
            if (!connected && !overrideLeftText)
                leftText.text = string.Format("Please wait{0}\r\nLeft Connecting", ellipsis);

            connected = rightGlove.Connected;
            rightPanel.SetActive((showPanels && !connected) || overrideRightText);
            if (!connected && !overrideRightText)
                rightText.text = string.Format("Please wait{0}\r\nRight Connecting", ellipsis);
        }
    }

    private string getEllipsis(int num)
    {
        //return "".PadRight(num, '.').PadRight(3, ' ');
        return "".PadRight(num, '.');
    }

    private Vector3 getAverage(List<Vector3> hist)
    {
        Vector3 result = Vector3.zero;

        foreach (Vector3 v in hist)
            result += v;

        result /= hist.Count;

        return result;
    }

    private void OrientPanel(GameObject panel, Vector3 position)
    {
        if (panel)
        {
            panel.transform.position = position + 0.2f * mainCamera.transform.up;
            panel.transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - panel.transform.position);
        }
    }
}
