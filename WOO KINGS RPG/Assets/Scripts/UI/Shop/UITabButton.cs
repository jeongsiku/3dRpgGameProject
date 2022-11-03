using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour
{
    Transform normal;
    Transform highlight;
    Button button;
    [SerializeField]
    EquipType equipType;

    public EquipType EquipType { get { return equipType; } }

    public void Init()
    {
        normal = transform.Find("Normal");
        highlight = transform.Find("Highlight");
        button = GetComponent<Button>();
    }

    public void SetActiveHighlight(bool state)
    {
        highlight.gameObject.SetActive(state);
    }

    public void SetListener(System.Action<UITabButton> action)
    {
        if(button != null)
            button.onClick.AddListener(()=> { action(this); });
    }
}
