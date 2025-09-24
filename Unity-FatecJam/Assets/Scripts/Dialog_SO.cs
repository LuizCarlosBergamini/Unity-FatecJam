using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct DialogEntity
{
    public string name;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "New Dialog")]
public class Dialog_SO : ScriptableObject
{


    [Serializable]
    public struct Dialog
    {
        public DialogEntity entity;
        public string text;
    }

    public List<DialogEntity> entities = new();
    public List<Dialog> dialogs;
}
