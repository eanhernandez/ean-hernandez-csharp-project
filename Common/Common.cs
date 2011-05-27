﻿using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace Common
{
    public static class CommsTools
    {
        public static void sendOrderDataToOME(String s, Socket mdpSocket, IPEndPoint mcastEp)
        {
            if (!Tools.ValidateOrderRequest(s)) { Tools.ThrowBadOrderInputException(); }
            else
            { SendMCastData(s, mdpSocket, mcastEp); }
        }
        public static void SendTradeDataToTicker(String s, Socket mdpSocket, IPEndPoint mcastEp)
        {
            if (!Tools.ValidateTickerInput(s)){Tools.ThrowBadTickerInputException();}
            else
            {SendMCastData(s, mdpSocket, mcastEp);}
        }
        public static void SendMCastData(String s, Socket mdpSocket, IPEndPoint mcastEp)
        {
            byte[] sendBuffer = new byte[512];
            sendBuffer = Encoding.ASCII.GetBytes(s);
            try
            {
                mdpSocket.SendTo(sendBuffer, mcastEp);
            }
            catch (SocketException e)
            {
                Tools.ThrowCommsException("socket exception trying to send CVS data to OME. " + e.Message);
            }
            
        }
        public static Socket SetUpMCastSendSocket()
        {
            Socket mdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 3);
            return mdpSocket;
        }
        public static Socket SetUpMcastListenSocket(int port)
        {
            IPHostEntry entry = Dns.GetHostByName(Dns.GetHostName());
            EndPoint localEp = new IPEndPoint(entry.AddressList[0], port);
            Socket mdcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mdcSocket.Bind(localEp);
            IPAddress groupAddress = IPAddress.Parse("224.5.6.7");
            MulticastOption mcastOption = new MulticastOption(groupAddress);
            mdcSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            return mdcSocket;
        }
    }
    public static class Tools
    {
        // checks that the CSV data is in the right format 
        public static bool ValidateOrderRequest(string s)
        {
            string[] vals = s.Split(',');

            if (
                (Regex.IsMatch(vals[0], @"[^A-Z]"))             // symbol is not letters only
            || (!vals[1].Equals("Regular"))                    // type is not "Regular"
            || (Regex.IsMatch(vals[2], @"[^BS]"))              // order type is not B or S
            || (!Regex.IsMatch(vals[3], @"^[0-9]*[.]?[0-9]+$"))// price is not a float
            || (!Regex.IsMatch(vals[4], @"^[0-9]"))            // quantity is not numbers
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void ThrowBadOrderInputException()
        {
            throw new BadOrderInput("the order data read in from CSV was in the wrong format.");
        }
        public static bool ValidateTickerInput(string s)
        {
            string[] vals = s.Split(' ');
            if ((Regex.IsMatch(vals[0], @"[^A-Z]")))// symbol is not letters only
            {
                return false;
            }
            vals = vals[1].Split('/');
            if ((!Regex.IsMatch(vals[0], @"^[0-9]*[.]?[0-9]+$")) // top buy != float or -
                && (!Regex.IsMatch(vals[0], @"[-]")))
            {
                return false;
            }
            if ((!Regex.IsMatch(vals[1], @"^[0-9]*[.]?[0-9]+$"))// top sell != float or -
                && (!Regex.IsMatch(vals[1], @"[-]")))
            {
                return false;
            }
            return true;
        }
        public static void ThrowBadTickerInputException()
        {
            throw new BadTickerInput("the trade data passed to the ticker was in the wrong format.");
        }
        public static void ThrowCommsException(string s)
        {
            throw new CommsException(s);
        }
    }
}
