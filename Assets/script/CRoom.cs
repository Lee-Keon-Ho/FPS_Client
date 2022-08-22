using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CRoom : MonoBehaviour
{
    public Button[] m_button;
    public TextMeshProUGUI[] m_teamA;
    public TextMeshProUGUI[] m_teamB;

    private int m_team_A_Count;
    private int m_team_B_Count;
    private void Awake()
    {
        m_team_A_Count = 0;
        m_team_B_Count = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
