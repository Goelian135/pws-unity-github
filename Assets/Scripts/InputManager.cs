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
        keybinds["Jump"] = KeyCode.W;
        keybinds["Attack"] = KeyCode.LeftControl;
        keybinds["Dash"] = KeyCode.LeftShift;
        keybinds["Heal"] = KeyCode.H;

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
}
