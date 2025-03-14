using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using JetBrains.Annotations;


public class NodeGraphEditor : EditorWindow
{
    public enum EditContext
    {
        Default,
        Connect
    }

    private List<Node> nodes = new List<Node>();
    public Node selectedNodeForMove;
    private Node selectedNodeForConnection;
    private Node selectedNodeForEditing;

    private float doubleClickTime = 0.3f;
    private float lastClickTime;

    public EditContext currentContext { get; private set; } = EditContext.Default;


    [MenuItem("Window/NodeGraphEditor")]
    public static void ShowWindow()
    {
        GetWindow<NodeGraphEditor>("NodeGraphEditor");
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
                // �� �׸���
                Handles.DrawLine(node.rect.center, connectedNode.rect.center);
            }
        }
    }

    public void CreateNode(Vector2 pos)
    {
        Node newNode = new Node(this, pos);
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
                            // ��� Ŭ��
                            if (node.IsHeaderClicked(mousePos))
                            {
                                selectedNodeForMove = node;
                                node.OnMouseDown(mousePos);
                            }
                            // ���� Ŭ��
                            else if (node.IsBodyClicked(mousePos))
                            {
                                if (selectedNodeForEditing is not null)
                                    selectedNodeForEditing.StopEditing();

                                node.StartEditing();
                                selectedNodeForEditing = node;
                            }
                            // �ٴ� Ŭ��
                            else if (node.IsBottomClicked(mousePos))
                            {
                                selectedNodeForConnection = node; // ������ ��� ����
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
                    lastClickTime = Time.realtimeSinceStartup; // ������ Ŭ�� �ð� ������Ʈ
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


