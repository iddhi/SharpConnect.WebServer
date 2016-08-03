﻿//2015-2016, MIT, EngineKit 
/* The MIT License
*
* Copyright (c) 2012-2015 sta.blockhead
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using SharpConnect.Internal;

namespace SharpConnect.WebServers
{

    public class WebSocketRequest
    {
        //-----------------------
        byte[] data;
        List<byte[]> moreFrames = new List<byte[]>(); 
        internal WebSocketRequest()
        { 
        } 
        public Opcode OpCode
        {
            get;
            internal set;
        }
        internal void SetRawBuffer(byte[] data)
        {
            this.data = data;
        }
        internal void AddNewFrame(byte[] newFrame)
        {
            //append data
            moreFrames.Add(newFrame);
        } 
        public string ReadAsString()
        {
            if (data != null && this.OpCode == Opcode.Text)
            {
                if (moreFrames.Count == 0)
                {
                    return System.Text.Encoding.UTF8.GetString(data);
                }
                else
                {
                    //merge
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(data, 0, data.Length);
                        int j = moreFrames.Count;
                        for (int i = 0; i < j; ++i)
                        {
                            byte[] f = moreFrames[i];
                            ms.Write(f, 0, f.Length);
                        }
                        ms.Flush();

                        string strResult = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        ms.Close();
                        return strResult;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        public char[] ReadAsChars()
        {
            if (data != null && this.OpCode == Opcode.Text)
            {
                if (moreFrames.Count == 0)
                {
                    return System.Text.Encoding.UTF8.GetChars(data);
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(data, 0, data.Length);
                        int j = moreFrames.Count;
                        for (int i = 0; i < j; ++i)
                        {
                            byte[] f = moreFrames[i];
                            ms.Write(f, 0, f.Length);
                        }
                        ms.Flush();

                        char[] charBuffer = System.Text.Encoding.UTF8.GetChars(ms.ToArray());
                        ms.Close();
                        return charBuffer;
                    }
                }
            }
            else
            {
                return null;
            }
        } 
        public void Clear()
        {
            moreFrames.Clear();
            data = null;
             
        }
    }
}