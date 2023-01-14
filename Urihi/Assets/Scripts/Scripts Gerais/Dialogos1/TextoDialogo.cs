using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TextoDialogo
{
    [SerializeField] [TextArea(1,4)] private string _frase;
    [SerializeField] private string _btnNext;

    public string GetFrase()
    {
        return _frase;
    }

    public string GetBotaoNext()
    {
        return _btnNext;
    }
}
