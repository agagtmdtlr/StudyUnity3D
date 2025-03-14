using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    NodeGraphEditor editor;

    static int id;

    public Rect rect;

    private string title;
    private string content = "content";
    private Vector2 offset;


    // 연결된 노드 리스트
    public List<Node> connectedNodes = new List<Node>();
    private bool isEditing = false;

    private const float HEADER_HEIGHT = 20;
    private const float BODY_HEIGHT = 60;
    private const float BOTTOM_HEIGHT = 20;

    private Color headerColor = new Color(0.3f, 0.5f, 0.8f); // 헤더 색상
    private Color bodyColor = new Color(0.5f, 0.7f, 0.5f);   // 본문 색상
    private Color bottomColor = new Color(0.8f, 0.3f, 0.3f); // 바닥 색상

    public Node(NodeGraphEditor editor, Vector2 position)
    {
        this.editor = editor;
        rect = new Rect(position.x, position.y, 100, 100);
        title = $"Node {id++}";
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
