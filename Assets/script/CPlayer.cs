using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayer
{
    string m_name;
    uint m_socket;
    uint m_addr;
    ushort m_port;
    int m_boss;
    int m_ready;
    int m_number;
    int m_team;
    string m_addrStr;
    bool m_udpConnect;
    Vector3 m_position;
    float m_rotation;
    int m_action;

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
    public void SetNumber(int _number) { m_number = _number; }

    public void SetTeam(int _team) { m_team = _team; }
    public int GetTeam() { return m_team; }
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
    public void SetPosition(Vector3 _position, float _rotation)
    {
        m_position = _position;
        m_rotation = _rotation;
        Debug.Log(m_rotation);
    }
    public Vector3 GetPosition() { return m_position; }
    public float GetRotation() { return m_rotation; }
    public void SetAction(int _action) { m_action = _action; }
    public int GetAction() { return m_action; }

    public void SetRotation(float _rotation) { m_rotation = _rotation; }
}
