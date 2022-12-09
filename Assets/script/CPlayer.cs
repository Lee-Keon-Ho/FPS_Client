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
    long m_sourceAddr;
    bool m_udpConnect;
    Vector3 m_position;
    float m_rotation;
    int m_action;
    int m_key;
    int m_kill;
    int m_Death;

    private int m_HP;

    public bool Init()
    {
        m_name = "";
        m_boss = 0;
        m_ready = 0;
        m_udpConnect = false;
        m_HP = 100;
        m_kill = 0;
        m_Death = 0;

        m_position = Vector3.zero;

        return true;
    }

    public void SetName(string _name) { m_name = _name;}
    public string GetName() { return m_name; }
    public void SetBoss(int _boss) { m_boss = _boss; }
    public int GetBoss() { return m_boss; }
    public void SetReady(int _ready) { m_ready = _ready; }
    public int GetReady() { return m_ready; }
    public void SetNumber(int _number) { m_number = _number; }
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
    public void SetSourceAddr(long _addr) { m_sourceAddr = _addr; }
    public long GetSourceAddr() { return m_sourceAddr; }
    public void SetUdpConnect(bool _udp) { m_udpConnect = _udp; }
    public bool GetUdpconnect() { return m_udpConnect; }
    public void SetPosition(Vector3 _position) {   m_position = _position; }
    public void SetRotation(float _rotation) { m_rotation = _rotation; }
    public Vector3 GetPosition() { return m_position; }
    public float GetRotation() { return m_rotation; }
    public void SetAction(int _action) { m_action = _action; }
    public int GetAction() { return m_action; }
    public void SetKey(int _key) { m_key = _key; }
    public int GetKey() { return m_key; }
    public int GetHp() { return m_HP; }
    public void SetHp(int _hp) { m_HP = _hp; }
    public int GetKill() { return m_kill; }
    public int GetDeath() { return m_Death; }
    public void AddKill() { m_kill += 1; }
    public void AddDeath() { m_Death += 1; }
    public void SetKill(int _kill) { m_kill = _kill; }
    public void SetDeath(int _Death) { m_Death = _Death; }
}
