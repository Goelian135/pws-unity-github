using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //default keybinds
        keybinds["MoveLeft"] = KeyCode.A;
        keybinds["MoveRight"] = KeyCode.D;
        keybinds["Up"] = KeyCode.W;
        keybinds["Down"] = KeyCode.S;
        keybinds["Jump"] = KeyCode.Space;
        keybinds["Dash"] = KeyCode.LeftShift;
        keybinds["Heal"] = KeyCode.LeftControl;

        keybinds["Attack"] = KeyCode.J;
        keybinds["BloodRythm"] = KeyCode.K;
    }

    public bool GetKey(string action)
    {
        if (keybinds.ContainsKey(action))
        {
            return Input.GetKey(keybinds[action]);
        }
        return false;
    }

    public bool GetKeyDown(string action)
    {
        if (keybinds.ContainsKey(action))
        {
            return Input.GetKeyDown(keybinds[action]);
        }
        return false;
    }

    public bool GetKeyUp(string action)
    {
        if (keybinds.ContainsKey(action))
        {
            return Input.GetKeyUp(keybinds[action]);
        }
        return false;
    }
}
