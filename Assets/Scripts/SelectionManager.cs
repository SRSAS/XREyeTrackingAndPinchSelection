using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static DebugUIBuilder;
using UnityEngine.UI;

// Class to manage the interaction between selectable targets and the user input, as well as to differentiate between the many targets
public class SelectionManager : MonoBehaviour
{
    // Selectors
    [SerializeField]
    private EyeHoverer dominantEye;
    [SerializeField]
    private EyeHoverer nonDominantEye;

    [SerializeField]
    private bool hovering = false;
    [SerializeField]
    private GameObject hovered;

    public float selectionDisplayTime = 1f;

    public Color defaultColor = Color.white;
    public Color hoverColor = Color.blue;
    public Color selectColor = Color.green;

    public UnityEvent onClick = new();

    void Start()
    {
        // On detecting a pinch from either hand, tell selectable targets to select if they are hovered
        transform.GetChild(0).gameObject.GetComponent<PinchDetector>().OnActivate.AddListener(PinchDetected);
        transform.GetChild(1).gameObject.GetComponent<PinchDetector>().OnActivate.AddListener(PinchDetected);
    }

    // Update is called once per frame
    void Update()
    {
        Unhover();
        if (dominantEye.hitSomething)
            Hover(dominantEye.hitObject);
        else if (nonDominantEye.hitSomething)
            Hover(nonDominantEye.hitObject);
    }

    private void PinchDetected()
    {
        onClick.Invoke();
        hovering = false;
        hovered = null;
    }

    void Hover(GameObject g)
    {
        Selectable s = g.GetComponent<Selectable>();
        if (s != null)
        {
            s.Hover();
            hovering = true;
            hovered = g;
        }
    }

    void Unhover()
    {
        if (hovering)
        {
            hovering = false;
            hovered.GetComponent<Selectable>().Unhover();
            hovered = null;
        }
    }
}
