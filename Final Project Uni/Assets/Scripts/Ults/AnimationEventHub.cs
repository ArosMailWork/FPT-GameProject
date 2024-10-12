using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class AnimationEventHub : SerializedMonoBehaviour
{
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    public Dictionary<string ,UnityEvent> eventDic;

    public void CallEventByID(string idInput)
    {
        if (eventDic.ContainsKey(idInput))
            eventDic[idInput].Invoke();
        else
            Debug.LogWarning($"No event found with AnimEventID: {idInput}");
    }
}
