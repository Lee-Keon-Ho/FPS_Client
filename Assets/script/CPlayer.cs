using System.Collections;
using System.Collections.Generic;

public class CPlayer
{
    string m_name;
    uint m_socket;
    uint m_addr;
    ushort m_port;
    int m_boss;
    int m_ready;
    int m_number;
    string m_addrStr;
    bool m_udpConnect;

    public bool Init()
    {
        m_name = "";
        m_boss = 0;
        m_ready = 0;
        m_udpConnect = false;

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

    public void SetSocet(uint _socket) { m_socket = _socket; }
    public uint GetSocket() { return m_socket; }
    public int GetNumber() { return m_number; }
    public void SetAddr(uint _addr, ushort _port) 
    { 
        m_addr = _addr;
        m_port = _port;
    }
    public ushort GetPort() { return m_port; }
    public uint GetAddr() { return m_addr; }
    public void SetAddrStr(string _str) { m_addrStr = _str; }
    public string GetAddrStr() { return m_addrStr; }
    public void SetUdpConnect(bool _udp) { m_udpConnect = _udp; }
    public bool GetUdpconnect() { return m_udpConnect; }
}
