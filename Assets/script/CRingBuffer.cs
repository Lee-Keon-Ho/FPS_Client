using System.IO;
public class CRingBuffer
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

    private bool IsFull()
    {
        return (remainDataSize >= bufferSize);
    }

    public int GetWriteBufferSize()
    {
        if (IsFull()) return 0;

        if (writeStream.Position >= readStream.Position) return bufferSize - (int)writeStream.Position;

        return (int)(readStream.Position - writeStream.Position);
    }

    public void Write(int _recvSize)
    {
        int size = remainDataSize + _recvSize;

        if(size >= bufferSize)
        {
            writeStream.Position = 0;
        }
        else
        {
            writeStream.Position += _recvSize;
            if (writeStream.Position > bufferSize) writeStream.Position = 0; // 지우고 새로 만들고
        }
        remainDataSize += _recvSize;
    }

    public void Read(int _size)
    {
        if(remainDataSize >= _size)
        {
            int endSize = bufferSize - (int)readStream.Position;

            if(endSize < _size)
            {
                endSize = _size - endSize;
                readStream.Position = endSize;
            }

            remainDataSize -= _size;
            readStream.Position += _size;
        }
    }

    public int GetSize()
    {
        return bufferSize;
    }

    public int GetRemainSize()
    {
        return remainDataSize;
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
