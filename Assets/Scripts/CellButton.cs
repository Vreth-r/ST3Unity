using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;
    private int outerIndex;
    private int innerIndex;

    public void Init(int oIndex, int iIndex)
    {
        outerIndex = oIndex;
        innerIndex = iIndex;
        button.onClick.AddListener(OnClick);
        SetText("-");
    }

    void OnClick() => GameManager.Instance.OnCellPressed(outerIndex, innerIndex, this);

    public void SetText(string val) => text.text = val;
    public void SetColor(Color c) => button.image.color = c;
    public void SetInteractable(bool v) => button.interactable = v;
    public bool IsEmpty() => string.IsNullOrEmpty(text.text);
}