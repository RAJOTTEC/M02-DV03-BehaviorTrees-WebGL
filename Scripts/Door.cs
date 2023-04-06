using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Text closedText;
    public Button closedButton;
    public Text lockedText;
    public Button lockedButton;

    public bool isClosed = false;
    public bool isLocked = false;

    Vector3 closedRotation = new Vector3(0, 0, 0);
    Vector3 openRotation = new Vector3(0, 80, 0);

    void Start()
    {
        if (isClosed)
        {
            transform.eulerAngles = closedRotation;
        }
        else
        {
            transform.eulerAngles = openRotation;
        }
    }

    void Update()
    {
        closedText.text = "Door closed = " + isClosed.ToString();
        lockedText.text = "Door locked = " + isLocked.ToString();
    }

    public void Closed()
    {
        if(isClosed == false)
        {
            isClosed = true;
            transform.eulerAngles = closedRotation;
        }
        else
        {
            isClosed = false;
            transform.eulerAngles = openRotation;
        }
    }

    public void Locked()
    {
        if (isLocked == false)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
    }

    public bool Open()
    {
        if (isClosed && !isLocked)
        {
            isClosed = false;
            transform.eulerAngles = openRotation;
            return true;
        }

        return false;
    }

    public bool Close()
    {
        if (!isClosed)
        {
            transform.eulerAngles = closedRotation;
            isClosed = true;
        }
        return true;
    }
}
