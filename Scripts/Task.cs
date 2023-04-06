using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public abstract void run();
    public bool succeeded;

    protected int eventId;
    const string EVENT_NAME_PREFIX = "FinishedTask";
    public string TaskFinished
    {
        get
        {
            return EVENT_NAME_PREFIX + eventId;
        }
    }
    public Task()
    {
        eventId = EventBus.GetEventID();
    }
}

public class IsTrue : Task
{
    bool varToTest;

    public IsTrue(bool someBool)
    {
        varToTest = someBool;

    }

    public override void run()
    {
        succeeded = varToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}


public class IsFalse : Task
{
    bool varToTest;

    public IsFalse(bool someBool)
    {
        varToTest = someBool;
    }

    public override void run()
    {
        succeeded = !varToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class OpenDoor : Task
{
    Door door;

    public OpenDoor(Door someDoor)
    {
        door = someDoor;
    }

    public override void run()
    {
        succeeded = door.Open();
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class BargeDoor : Task
{
    Rigidbody door;

    public BargeDoor(Rigidbody someDoor)
    {
        door = someDoor;
    }

    public override void run()
    {
        door.AddForce(-10f, 0, 0, ForceMode.VelocityChange);
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class ReadyToBreak : Task
{
    GameObject thing;

    public ReadyToBreak(GameObject someThing)
    {
        thing = someThing;
    }

    public override void run()
    {
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Pause : Task
{
    float pauseTime;

    public Pause(float time)
    {
        pauseTime = time;
    }

    public override void run()
    {
        succeeded = true;
        EventBus.ScheduleTrigger(TaskFinished, pauseTime);
    }
}

public class MoveToObject : Task
{
    Arriver Mover;
    GameObject Target;

    public MoveToObject(Kinematic mover, GameObject target)
    {
        Mover = mover as Arriver;
        Target = target;
    }

    public override void run()
    {
        Mover.OnArrived += MoverArrived;
        Mover.myTarget = Target;
    }

    public void MoverArrived()
    {
        Mover.OnArrived -= MoverArrived;
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Sequence : Task
{
    List<Task> children;
    Task currentTask;
    int currentTaskIndex = 0;

    public Sequence(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        currentTask = children[currentTaskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        if (currentTask.succeeded)
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            currentTaskIndex++;
            if (currentTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                succeeded = true;
                EventBus.TriggerEvent(TaskFinished);
            }

        }
        else
        {
            succeeded = false;
            EventBus.TriggerEvent(TaskFinished);
        }
    }
}

public class Selector : Task
{
    List<Task> children;
    Task currentTask;
    int currentTaskIndex = 0;

    public Selector(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        currentTask = children[currentTaskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        if (currentTask.succeeded)
        {
            succeeded = true;
            EventBus.TriggerEvent(TaskFinished);
        }
        else
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            currentTaskIndex++;
            if (currentTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                succeeded = false;
                EventBus.TriggerEvent(TaskFinished);
            }
        }
    }
}

