using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance;

    private bool showConsole;
    private bool showHelp;

    private string input;


    public DebugCommand HELP;
    public DebugCommand HEAL;
    public DebugCommand MUTE;
    public DebugCommand NOCLIP;
    public DebugCommand<int> GOTO;

    public List<object> commandList;


    private Vector2 scroll;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        HELP = new DebugCommand("help", "��ɾ� ����Ʈ�� �����ݴϴ�.", "help", () =>
        {
            showHelp = true;
        });

        HEAL = new DebugCommand("heal", "���� ü���� �ִ�� ä��ϴ�.", "heal", () =>
        {
            PlayerManager.Instance.PlayerChar.SetHP(PlayerManager.Instance.PlayerChar.MaxHP);
        });

        MUTE = new DebugCommand("mute", "���ҰŸ� �մϴ�.", "mute", () =>
        {
            SoundManager.Instance.Mute();
        });

        NOCLIP = new DebugCommand("noclip", "�÷��̾ �����̵����� �ٲߴϴ�.", "noclip", () =>
        {
            PlayerManager.Instance.PlayerCtrl.Noclip();
        });

        GOTO = new DebugCommand<int>("goto", "������ ��ȣ�� üũ����Ʈ�� �̵��մϴ�.", "goto <index>", (x) =>
        {
            PlayerManager.Instance.MovePlayer(x);
        });

        commandList = new List<object>
        {
            HELP,
            HEAL,
            MUTE,
            NOCLIP,
            GOTO
        };
    }

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if(showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 90), scroll, viewport);

            for(int i=0;i<commandList.Count;i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);

            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);


        Event e = Event.current;
        if (e.keyCode == KeyCode.Return)
            OnReturn();
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }
}
