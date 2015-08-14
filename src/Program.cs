﻿//2015, MIT, EngineKit
using System;
using System.Net;
using System.Text;
using SharpConnect.WebServer;
using SharpConnect.Internal;

namespace SharpConnect
{
    static class Program
    {
        static bool runApp = true;

        static void Main(string[] args)
        {
            Main2();
        }
        static void Main2()
        {


            TestApp testApp = new TestApp();
            try
            {
                //1. create  
                HttpServer webServer = new HttpServer(4444, testApp.HandleRequest);
                webServer.Start();

                //run 
                //main thread
                while (runApp)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // close the stream for test file writing
                try
                {
#if DEBUG

#endif
                }
                catch
                {
                    Console.WriteLine("Could not close log properly.");
                }
            }
        }
        static void Main1()
        {

            SocketServerSettings serverSetting;
            TestApp testApp;
            int port = 4444;
            int maxNumberOfConnections = 1000;
            int excessSaeaObjectsInPool = 200;
            int backlog = 100;
            int maxSimultaneousAcceptOps = 100;
            testApp = new TestApp();

            try
            {
                // Get endpoint for the listener.                
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
#if DEBUG
                dbugLOG.WriteSetupInfo(localEndPoint);
#endif
                //This object holds a lot of settings that we pass from Main method
                //to the SocketListener. In a real app, you might want to read
                //these settings from a database or windows registry settings that
                //you would create. 

                //---------------------------------------------------------------------------------
                //instantiate the SocketListener,
                //and Run Listen Loop in ctor *** 
                HttpSocketServerSetting setting = new HttpSocketServerSetting(
                    maxNumberOfConnections,
                    excessSaeaObjectsInPool,
                    backlog,
                    maxSimultaneousAcceptOps,
                    localEndPoint);
                setting.RequestHandler += testApp.HandleRequest;
                serverSetting = setting;
                //serverSetting = new CustomSocketServerSetting(maxNumberOfConnections,
                //   excessSaeaObjectsInPool,
                //   backlog,
                //   maxSimultaneousAcceptOps,
                //   testBufferSize,
                //   opsToPreAlloc,
                //   localEndPoint);
                //create and run until stop...
                SocketServer socketServer = new SocketServer(serverSetting);
                //run  
                //--------------------------------------------------------------------------------- 
#if DEBUG
                dbugLOG.ManageClosing(socketServer);
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // close the stream for test file writing
                try
                {
                    //#if DEBUG
                    //                    dbugLOG.testWriter.Close();
                    //#endif
                }
                catch
                {
                    Console.WriteLine("Could not close log properly.");
                }
            }
        }
    }
}