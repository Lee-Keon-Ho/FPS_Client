using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CRingBuffer : MonoBehaviour
{
    byte[] buffer;
    int bufferSize;
    int remainDataSize;

    MemoryStream writeStream;
    MemoryStream readStream;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;

    public CRingBuffer(int _size)
    {
        bufferSize = _size;

        buffer = new byte[bufferSize];

        writeStream = new MemoryStream(buffer);
        readStream = new MemoryStream(buffer);

        binaryWriter = new BinaryWriter(writeStream);
        binaryReader = new BinaryReader(readStream);
    }

    public int GetSize()
    {
        return bufferSize;
    }
    public byte[] GetBuffer()
    {
        return buffer;
    }

    public int GetWritePos()
    {
        return (int)writeStream.Position;
    }

    public int GetReadPos()
    {
        return (int)readStream.Position;
    }
}
