using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

public class StatusesSystem : CharacterSystem
{
    public Dictionary<String, Status> statuses = new Dictionary<String, Status>();
    public List<String> statusesNames = new List<String>();
    public Dictionary<String, Counter> counters = new Dictionary<String, Counter>();
    public int size = 0;

    public void PutStatus(Status status)
    {
        statuses.Add(status.name, status);
        if (status.hasDuration)
        {
            Counter c = new Counter(status.name, status.duration);
            counters.Add(status.name, c);

        }
    }
    public void RemoveStatus(Status status)
    {
        if (statusesNames.Contains(status.name))
        {
            statuses.Remove(status.name);
            if (status.hasDuration)
            {
                counters.Remove(status.name);
            }
        }
    }
    void Update()
    {
        if (counters.Count > 0)
        {
            foreach (Counter c in counters.Values)
            {
                c.Execute();
                if (!c.IsRunning)
                {
                    RemoveStatus(statuses[c.name]);
                }
            }
        }

    }
}
