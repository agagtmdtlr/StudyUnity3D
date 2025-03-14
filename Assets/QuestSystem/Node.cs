using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    
    static int id;

    public Rect rect;

    private string title;
    private string content = "content";
    private Vector2 offset;

    private VisualElement visualElement;


    // 연결된 노드 리스트
    public List<Node> connectedNodes = new List<Node>();
    private bool isEditing = false;

    private const float HEADER_HEIGHT = 20;
    private const float BODY_HEIGHT = 60;
    private const float BOTTOM_HEIGHT = 20;

    private Color headerColor = new Color(0.3f, 0.5f, 0.8f); // 헤더 색상
    private Color bodyColor = new Color(0.5f, 0.7f, 0.5f);   // 본문 색상
    private Color bottomColor = new Color(0.8f, 0.3f, 0.3f); // 바닥 색상

    public Node( Rect position)
    {
        rect = position;
        title = $"Node {id++}";

        // VisualElement 생성
        visualElement = new VisualElement
        {
            style =
            {
                width = rect.width,
                height = rect.height,
                position = Position.Absolute,
                left = rect.x,
                top = rect.y,
            }
        };

        visualElement.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
        visualElement.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        visualElement.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }

    public VisualElement GetVisualElement()
    {
        UpdateVisualElement();
        return visualElement;
    }

    public void UpdateVisualElement()
    {
        visualElement.Clear();

        // 헤더 그리기
        var header = new VisualElement
        {
            style =
            {
                backgroundColor = headerColor,
                height = HEADER_HEIGHT,
            }
        };
        header.Add(new Label(title) { style = { color = Color.white } });

        // 본문 그리기
        var body = new VisualElement
        {
            style =
            {
                backgroundColor = bodyColor,
                height = BODY_HEIGHT,
            }
        };

        // 본문 영역에 대한 텍스트 필드 또는 레이블
        if (isEditing)
        {
            var textField = new TextField
            {
                value = title,
                style =
                {
                    marginLeft = 10,
                    marginTop = 10,
                    marginRight = 10,
                    height = BODY_HEIGHT - 20,
                }
            };
            textField.RegisterValueChangedCallback(e => title = e.newValue);
            body.Add(textField);
        }
        else
        {
            body.Add(new Label(title) { style = { marginLeft = 10, marginTop = 10 } });
        }

        // 바닥 그리기
        var bottom = new VisualElement
        {
            style =
            {
                backgroundColor = bottomColor,
                height = BOTTOM_HEIGHT,
            }
        };

        visualElement.Add(header);
        visualElement.Add(body);
        visualElement.Add(bottom);
    }

    public void OnMouseDownEvent(MouseDownEvent evt)
    {
        offset = evt.localMousePosition; // 마우스 위치
    }

    public void OnMouseMoveEvent(MouseMoveEvent evt)
    {
        if (evt.pressedButtons == 1) // Left mouse button
        {
            rect.position += evt.mouseDelta; // 위치 이동
            UpdateVisualElement(); // 위치 업데이트
        }
    }

    public void OnMouseUpEvent(MouseUpEvent evt)
    {
        // 추가 로직이 필요한 경우 여기에 작성
    }

    public void Connect(Node targetNode)
    {
        if (!connectedNodes.Contains(targetNode))
        {
            connectedNodes.Add(targetNode);
        }
    }

    public void StartEditing()
    {
        isEditing = true;
    }

    public void StopEditing()
    {
        isEditing = false;
    }

    public void Draw()
    {

        // 헤더 그리기
        GUI.backgroundColor = headerColor;
        GUI.Box(new Rect(rect.x, rect.y, rect.width, HEADER_HEIGHT), title);

        // 본문 그리기
        GUI.backgroundColor = bodyColor;
        GUI.Box(new Rect(rect.x, rect.y + HEADER_HEIGHT, rect.width, BODY_HEIGHT), "");

        // 바닥 그리기
        GUI.backgroundColor = bottomColor;
        GUI.Box(new Rect(rect.x, rect.y + HEADER_HEIGHT + BODY_HEIGHT, rect.width, BOTTOM_HEIGHT), "");

        // 본문 영역에 대한 텍스트 필드 또는 레이블
        if (isEditing)
        {
            content = GUI.TextField(new Rect(rect.x + 10, rect.y + HEADER_HEIGHT + 10, rect.width - 20, BODY_HEIGHT), content);
        }
        else
        {
            GUI.Label(new Rect(rect.x + 10, rect.y + HEADER_HEIGHT + 10, rect.width - 20, BODY_HEIGHT), content);
        }

        // 원래 색상으로 복원
        GUI.backgroundColor = Color.white;
    }

    public void OnMouseDown(Vector2 mousePosition)
    {


        offset = mousePosition - new Vector2(rect.x, rect.y);

    }

    public void OnMouseDrag(Vector2 mousePosition)
    {
        rect.position = mousePosition - offset;
    }

    public void OnMouseUp()
    {
        // Additional logic can be added here if needed
    }

    public bool IsHeaderClicked(Vector2 mousePosition)
    {
        return new Rect(rect.x, rect.y, rect.width, HEADER_HEIGHT).Contains(mousePosition);
    }

    public bool IsBodyClicked(Vector2 mousePosition)
    {
        return new Rect(rect.x, rect.y + HEADER_HEIGHT, rect.width, BODY_HEIGHT).Contains(mousePosition);
    }

    public bool IsBottomClicked(Vector2 mousePosition)
    {
        return new Rect(rect.x, rect.y + HEADER_HEIGHT + BODY_HEIGHT, rect.width, BOTTOM_HEIGHT).Contains(mousePosition);
    }

}
