using System.Collections;
using System.Collections.Generic;

public class CPlayer
{
    string m_name;
    int m_boss;
    int m_ready;
    int m_number;
    public bool Init()
    {
        m_name = "";
        m_boss = 0;
        m_ready = 0;
        
        if (m_name == null) return false;

        return true;
    }

    public void SetName(string _name) { m_name = _name;}
    public string GetName() { return m_name; }
    public void SetBoss(int _boss) { m_boss = _boss; }
    public int GetBoss() { return m_boss; }
    public void SetReady(int _ready) { m_ready = _ready; }
    public int GetReady() { return m_ready; }
    public void SetNumber(int _number) 
    {
        m_number = _number;
        CGameManager.Instance.number = _number;
    } 
    public int GetNumber() { return m_number; }
}
