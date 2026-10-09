using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Small helper that builds the menu UI (Canvas, text and buttons) from code
public static class MenuUI
{
    public static readonly Color TextColor = Color.white;
    public static readonly Color HighlightColor = new Color(1f, 0.82f, 0.25f);
    public static readonly Color PressedColor = new Color(1f, 0.55f, 0.15f);

    static Font font;

    static Font MenuFont
    {
        get
        {
            if (font == null) font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            return font;
        }
    }

    // Create a full-screen overlay canvas with a background panel
    public static RectTransform CreateCanvas(Transform parent, string name, Color background, int sortingOrder)
    {
        GameObject canvasObject = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObject.transform.SetParent(parent, false);

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject panel = new GameObject("Background", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvasObject.transform, false);
        Stretch(panel.GetComponent<RectTransform>());
        panel.GetComponent<Image>().color = background;

        EnsureEventSystem();
        return panel.GetComponent<RectTransform>();
    }

    // Add a text label anchored at the given point (0-1 in both axes)
    public static Text CreateText(RectTransform parent, string content, int fontSize, Vector2 anchor, TextAnchor alignment, Vector2 size)
    {
        GameObject textObject = new GameObject(content, typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(parent, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(alignment == TextAnchor.MiddleLeft ? 0f : alignment == TextAnchor.MiddleRight ? 1f : 0.5f, 0.5f);
        rect.sizeDelta = size;

        Text text = textObject.GetComponent<Text>();
        text.text = content;
        text.font = MenuFont;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = TextColor;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }

    // Add a text-only button that highlights when hovered or selected
    public static Button CreateButton(RectTransform parent, string label, int fontSize, Vector2 anchor, TextAnchor alignment, UnityAction onClick)
    {
        Text text = CreateText(parent, label, fontSize, anchor, alignment, new Vector2(900, fontSize * 1.4f));
        text.raycastTarget = true;

        Button button = text.gameObject.AddComponent<Button>();
        button.targetGraphic = text;

        // Text is white, so the tint colors give the final hover/press colors
        ColorBlock colors = button.colors;
        colors.normalColor = TextColor;
        colors.highlightedColor = HighlightColor;
        colors.selectedColor = HighlightColor;
        colors.pressedColor = PressedColor;
        colors.fadeDuration = 0.08f;
        button.colors = colors;

        button.onClick.AddListener(onClick);
        return button;
    }

    // Link buttons so the arrow keys move up and down the list
    public static void LinkVertically(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation navigation = new Navigation { mode = Navigation.Mode.Explicit };
            navigation.selectOnUp = buttons[(i - 1 + buttons.Length) % buttons.Length];
            navigation.selectOnDown = buttons[(i + 1) % buttons.Length];
            buttons[i].navigation = navigation;
        }
    }

    static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    // UI clicks and keyboard navigation need an EventSystem using the new Input System
    static void EnsureEventSystem()
    {
        if (Object.FindAnyObjectByType<EventSystem>() != null) return;
        new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
    }
}
