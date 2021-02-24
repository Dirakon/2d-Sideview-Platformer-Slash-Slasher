using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeValue : MonoBehaviour
{
    public GameObject BGMusic;  //Ваш объект с фоновой музыкой
    private AudioSource audioSrc;
    public GameObject[] objs1;

    void Awake()
    {
        objs1 = GameObject.FindGameObjectsWithTag("Sound"); //не забываем задать тег Sound для префаба с музыкой
        if (objs1.Length == 0)
        {
            BGMusic = Instantiate(BGMusic); //создаем объект из префаба
            BGMusic.name = "BGMusic";  //необязательно, просто внешний вид улучшает:)
            DontDestroyOnLoad(BGMusic.gameObject); //Ответ на Ваш вопрос
        }
        else
        {
            BGMusic = GameObject.Find("BGMusic"); //обращаемся к объекту, если он уже существует.
        }
    }
    void Start()
    {
        audioSrc = BGMusic.GetComponent<AudioSource>();
    }


    void Update()
    {
    }

}