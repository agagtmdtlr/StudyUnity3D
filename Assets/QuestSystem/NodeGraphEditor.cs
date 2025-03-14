using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;
using Unity.VisualScripting;


public class NodeGraphEditor : EditorWindow
{
    public enum EditContext
    {
        Default,
        Connect
    }

    private List<Node> nodes = new List<Node>();
    public Node selectedNodeForMove;
    public Node selectedNode;
    private Node selectedNodeForConnection;
    private Node selectedNodeForEditing;

    private float doubleClickTime = 0.3f;
    private float lastClickTime;

    public EditContext currentContext { get; private set; } = EditContext.Default;


    [MenuItem("Window/NodeGraphEditor")]
    public static void ShowWindow()
    {
        NodeGraphEditor wnd = GetWindow<NodeGraphEditor>();
        wnd.titleContent = new GUIContent("Node Graph");
        wnd.Show();
    }

    public void OnEnable()
    {
        // VisualElement 초기화
        var root = rootVisualElement;

        // 클릭 이벤트 처리
        root.RegisterCallback<MouseDownEvent>(OnMouseDown);
        root.RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }
    private void OnMouseMove(MouseMoveEvent evt)
    {
        if (selectedNode != null)
        {
            selectedNode.OnMouseDrag(evt.localMousePosition);
        }
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (evt.button == 0) // Left mouse button
        {
            Vector2 mousePos = evt.localMousePosition;

            foreach (var node in nodes)
            {
                if (node.rect.Contains(mousePos))
                {
                    // 헤더 클릭
                    if (node.IsHeaderClicked(mousePos))
                    {
                        selectedNode = node;
                        node.OnMouseDown(evt.localMousePosition);
                    }
                    // 본문 클릭
                    else if (node.IsBodyClicked(mousePos))
                    {
                        node.StartEditing();
                    }
                    // 바닥 클릭
                    else if (node.IsBottomClicked(mousePos))
                    {
                        selectedNodeForConnection = node;
                    }

                    evt.StopPropagation();
                    return;
                }
            }

            Node newNode = new Node(new Rect(mousePos.x, mousePos.y, 100, 100));
            nodes.Add(newNode);
            var root = rootVisualElement;
            root.Add(newNode.GetVisualElement());
            evt.StopPropagation();
        }
    }

    private void OnGUI()
    {
        DrawConnections();
        DrawNodes();

        ProcessEvents(Event.current);
        Repaint();
    }

    private void DrawNodes()
    {
        foreach (var node in nodes)
        {
            node.Draw();
        }
    }

    private void DrawConnections()
    {
        foreach (var node in nodes)
        {
            foreach (var connectedNode in node.connectedNodes)
            {
                // 선 그리기
                Handles.DrawLine(node.rect.center, connectedNode.rect.center);
            }
        }
    }

    public void CreateNode(Vector2 pos)
    {
        Rect rect = new Rect(pos, new Vector2(100, 100));
        Node newNode = new Node(rect);
        nodes.Add(newNode);
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.KeyDown:
                switch (e.keyCode)
                {
                    case KeyCode.Space:
                        CreateNode(e.mousePosition);
                        break;
                    case KeyCode.Return:
                        if (selectedNodeForEditing is not null)
                        {
                            selectedNodeForEditing.StopEditing();
                            selectedNodeForEditing = null;
                        }
                        break;
                }
                e.Use();
                break;
            case EventType.MouseDown:
                if (e.button == 0) // Left mouse button
                {
                    var mousePos = e.mousePosition;
                    // Check if we clicked on a node
                    foreach (var node in nodes)
                    {
                        if (node.rect.Contains(mousePos))
                        {
                            // 헤더 클릭
                            if (node.IsHeaderClicked(mousePos))
                            {
                                selectedNodeForMove = node;
                                node.OnMouseDown(mousePos);
                            }
                            // 본문 클릭
                            else if (node.IsBodyClicked(mousePos))
                            {
                                if (selectedNodeForEditing is not null)
                                    selectedNodeForEditing.StopEditing();

                                node.StartEditing();
                                selectedNodeForEditing = node;
                            }
                            // 바닥 클릭
                            else if (node.IsBottomClicked(mousePos))
                            {
                                selectedNodeForConnection = node; // 연결할 노드 선택
                            }
                            e.Use();
                            return;
                        }
                    }

                    if (selectedNodeForEditing is not null)
                    {
                        selectedNodeForEditing.StopEditing();
                        selectedNodeForEditing = null;
                    }
                    lastClickTime = Time.realtimeSinceStartup; // 마지막 클릭 시간 업데이트
                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (selectedNodeForMove != null)
                {
                    selectedNodeForMove.OnMouseDrag(e.mousePosition);
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (selectedNodeForMove != null)
                {
                    selectedNodeForMove.OnMouseUp();
                    selectedNodeForMove = null;
                    e.Use();
                }
                break;
        }
    }

    
}


