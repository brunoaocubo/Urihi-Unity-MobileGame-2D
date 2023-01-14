using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controleVolume : MonoBehaviour
{ 
    public float volumeGeral, volumeMusica, volumeFx;
    public Slider sliderGeral, sliderMusica, sliderFx;
    public AudioSource[] fx;
    // Start is called before the first frame update
    void Start()
    {
        //codigo para pegar o valor do slider e deixar salvo
        sliderGeral.value = PlayerPrefs.GetFloat("Geral");
        sliderMusica.value = PlayerPrefs.GetFloat("Musica");
        sliderFx.value = PlayerPrefs.GetFloat("Fx");
    }

    public void VolumeGeral(float volume)
    {
        volumeGeral = volume;
        AudioListener.volume = volumeGeral;

        //pegar o valor salvo do slide e executar o mesmo valor quando iniciar o jogo novamente
        PlayerPrefs.SetFloat("Geral", volumeGeral);
    }

    public void VolumeMusica(float volume)
    {
        volumeMusica = volume;
        GameObject[] Musicas = GameObject.FindGameObjectsWithTag("Musica");
        for (int i = 0; i < Musicas.Length; i++)
        {
            Musicas[i].GetComponent<AudioSource>().volume = volumeMusica;
        }

        //pegar o valor salvo do slide e executar o mesmo valor quando iniciar o jogo novamente
        PlayerPrefs.SetFloat("Musica", volumeMusica);
    }
    public void VolumeFx(float volume)
    {
        volumeFx = volume;
        for (int i = 0; i < fx.Length; i++)
        {
            fx[i].GetComponent<AudioSource>().volume = volumeFx;
        }

        //pegar o valor salvo do slide e executar o mesmo valor quando iniciar o jogo novamente
        PlayerPrefs.SetFloat("Fx", volumeFx);
    }
}
