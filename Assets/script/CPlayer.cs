using System.Collections;
using System.Collections.Generic;

public class CPlayer
{
    string m_name;
    int m_boss;

    public bool Init()
    {
        m_name = "";
        m_boss = 0;

        if (m_name == null) return false;

        return true;
    }

    public void SetName(string _name)
    {
        m_name = _name;
    }

    public string GetName() { return m_name; }
}
