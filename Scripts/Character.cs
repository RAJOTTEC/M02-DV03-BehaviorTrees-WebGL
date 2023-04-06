using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Door theDoor;
    public GameObject objective;
    public GameObject test;
    bool executingBehavior = false;
    Task myCurrentTask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!executingBehavior)
            {
                executingBehavior = true;
                myCurrentTask = BuildTask_GetObjective();

                EventBus.StartListening(myCurrentTask.TaskFinished, OnTaskFinished);
                myCurrentTask.run();
            }
        }
    }

    void OnTaskFinished()
    {
        EventBus.StopListening(myCurrentTask.TaskFinished, OnTaskFinished);
        executingBehavior = false;
    }

    Task BuildTask_GetObjective()
    {
        List<Task> taskList = new List<Task>();

        Task isDoorNotLocked = new IsFalse(theDoor.isLocked);
        Task pause = new Pause(0.25f);
        Task openDoor = new OpenDoor(theDoor);
        taskList.Add(isDoorNotLocked);
        taskList.Add(pause);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(theDoor.isClosed);
        Task readyToBreak = new ReadyToBreak(this.gameObject);
        Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorClosed);
        taskList.Add(pause);
        taskList.Add(readyToBreak);
        taskList.Add(pause);
        taskList.Add(bargeDoor);
        taskList.Add(pause);
        Sequence bargeClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        taskList = new List<Task>();
        Task moveToDoor = new MoveToObject(this.GetComponent<Kinematic>(), theDoor.gameObject);
        Task moveToObjective = new MoveToObject(this.GetComponent<Kinematic>(), objective.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(pause);
        taskList.Add(openTheDoor);
        taskList.Add(pause);
        taskList.Add(moveToObjective);
        Sequence getObjectiveBehindClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(theDoor.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToObjective);
        Sequence getObjectiveBehindOpenDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(getObjectiveBehindOpenDoor);
        taskList.Add(getObjectiveBehindClosedDoor);
        Selector getObjective = new Selector(taskList);

        return getObjective;
    }
}
