using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GerenciadordeDialogos : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nomeNpc;
    [SerializeField] TextMeshProUGUI _texto;
    [SerializeField] TextMeshProUGUI _btnNext;

    [SerializeField] GameObject _textBox, Raoni, Yakecan, textBoxPaje;

    int _contador = 0;
    private Dialogo _dialogoAtual;
    public void Inicializa(Dialogo dialogo)
    {
        _contador = 0;
        _dialogoAtual = dialogo;
        ProximaFrase();
        
    }

    private void Update()
    {
        if(_contador == 4)
        {
            Raoni.SetActive(true);
            Yakecan.SetActive(true);
        }
        if(_contador >= 5)
        {
            _nomeNpc.text = "Yakecan";
        }
        if(_contador > 10)
        {
            _nomeNpc.text = "Narrador";
        }

        if(_contador == 16)
        {
            SceneManager.LoadScene("Principal");
            _contador = 0;
        }
    }

    public void ProximaFrase()
    {
        if(_dialogoAtual == null)
            return;
            if (_contador == _dialogoAtual.GetFrases().Length)
            {
                _textBox.gameObject.SetActive(false);
                _dialogoAtual = null;
                _contador = 0;
                return;
            }
        

        _nomeNpc.text = _dialogoAtual.GetNomeNpc();
        _texto.text = _dialogoAtual.GetFrases()[_contador].GetFrase();
        _btnNext.text = _dialogoAtual.GetFrases()[_contador].GetBotaoNext();
        _textBox.gameObject.SetActive(true);
        _contador++;
    }
}
