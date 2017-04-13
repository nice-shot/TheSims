using UnityEngine;

namespace TeamZapocalypse {
/// <summary>
/// A text element that has a limit to how fast it can change. This makes it
/// easier to see what happened.
/// </summary>
public class TextBubble : MonoBehaviour {
    [SerializeField] TextMesh textField;
    [SerializeField] float minTimeToShowText = 0.5f;

    private float nextTextChangeTime;
    private string nextText;

    /// <summary>
    /// Called when adding this component to an object.
    /// </summary>
    protected void Reset() {
        textField = GetComponentInChildren<TextMesh>();
    }

    protected void Update() {
        if (!string.IsNullOrEmpty(nextText)) {
            SetText(nextText);
        }
    }

    public void SetText(string text) {
        if (textField.text == text) {
            nextText = null;
            enabled = false;
            return;
        }
        var now = Time.realtimeSinceStartup;
        // Check if we should change the text now or wait.
        if (now < nextTextChangeTime) {
            // Need to wait.
            nextText = text;
            enabled = true;
            return;
        }
        nextText = null;
        enabled = false;
        nextTextChangeTime = now + minTimeToShowText;
        textField.text = text;
    }
}
}