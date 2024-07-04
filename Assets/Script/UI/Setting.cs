using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Setting : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button btRETURN_TO_GAME;
    [SerializeField] private Button btSAVED_SYSTEMS;
    [SerializeField] private Button btBASIC_RESPAWN;
    [SerializeField] private Button btMISSIONS_STORY;
    [SerializeField] private Button btMISSIONS_CHALLENGES;
    [SerializeField] private Button btGOD_OPTIONS;
    [SerializeField] private Button btACHIEVEMENTS;
    [SerializeField] private Button btCONTROLS;
    [SerializeField] private Button btOPTIONS;
    [SerializeField] private Button btSAVE_AND_EXIT;

    [SerializeField] List<Button> bts;
    int index;
    int currentIndex;

    public void AddIndex(int number)
    {

        index += number;
        if (index >= bts.Count)
        {
            index -= bts.Count;
        }
        else if (index < 0)
        {
            index += bts.Count;
        }
    }

    public void ChangeIndex(int number)
    {
        index = number;
    }

    public void OnChangeIndex(int newIndex)
    {
        if (currentIndex != newIndex)
        {
            HightlightButton(newIndex);
        }

        currentIndex = newIndex;
    }
    private void OnEnable()
    {
        index = 0;
        currentIndex = index;
        HightlightButton(currentIndex);

    }

    private void Update()
    {
        if (GameManager.instance.IsGameState(GameState.Menu))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AddIndex(-1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AddIndex(1);
            }

            OnChangeIndex(index);
        }
    }

    public void HightlightButton(int number)
    {
        for (int i = 0; i < bts.Count; i++)
        {
            if (i == number)
            {
                Button bt = bts[i];
                ColorBlock colorblock = bt.colors;
                Color color = colorblock.normalColor;
                color.a = 255f;
                colorblock.normalColor = color;
                bt.colors = colorblock;
            }
            else
            {
                Button bt = bts[i];
                ColorBlock colorblock = bt.colors;
                Color color = colorblock.normalColor;
                color.a = 0;
                colorblock.normalColor = color;
                bt.colors = colorblock;
            }
        }

    }


}
