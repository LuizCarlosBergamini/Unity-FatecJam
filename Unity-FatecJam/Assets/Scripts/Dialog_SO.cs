using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogEntity
{
    [HideInInspector] public string name; // não deixa o nome ser editado
    public Sprite icon;                   // mas o ícone fica editável
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "New Dialog")]
public class Dialog_SO : ScriptableObject
{
    [Serializable]
    public struct Dialog
    {
        public DialogEntity entity;
        public string text;
        public float duration;
    }

    public List<DialogEntity> entities = new();

    public List<Dialog> dialogs;

    private static readonly string[] fixedNames =
    {
        "Alma Vermelha",
        "Alma Dourada",
        "Alma Azul",
        "Corvo",
        "Caronte"
    };

    private void OnValidate()
    {
        // Garante que existam sempre 5 entidades fixas
        for (int i = 0; i < fixedNames.Length; i++)
        {
            if (entities.Count <= i)
                entities.Add(new DialogEntity());

            entities[i] = new DialogEntity
            {
                name = fixedNames[i],         // trava o nome
                icon = entities[i].icon       // preserva o ícone editado
            };
        }

        // Se tiver mais que 5, remove o excesso
        if (entities.Count > fixedNames.Length)
            entities.RemoveRange(fixedNames.Length, entities.Count - fixedNames.Length);
    }
}
